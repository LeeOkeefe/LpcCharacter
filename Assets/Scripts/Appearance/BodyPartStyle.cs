using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animation;
using Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D.Animation;

namespace Appearance
{
    [RequireComponent(typeof(SpriteRenderer))]
    internal sealed class BodyPartStyle : MonoBehaviour
    {
        [SerializeField] private BodyPart bodyPart;    
        [SerializeField] private Color[] colours;
        [SerializeField] private BodyPartEventChannelSO onBodyPartChanged;

        private SpriteRenderer _spriteRenderer;
        private AsyncOperationHandle<IList<SpriteLibraryAsset>> _handle;
        private IList<SpriteLibraryAsset> _spriteSequences;

        private Gender _gender;
        
        private int _currentSpritesId;
        private int _currentColourId;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private async void Start()
        {
            await LoadSprites();
            
            UpdateSprites();
        }

        public void SetNextSprites()
        {
            _currentSpritesId = GetNextId(_currentSpritesId, _spriteSequences);
            
            UpdateSprites();
        }

        public void SetPreviousSprites()
        {
            _currentSpritesId = GetPreviousId(_currentSpritesId, _spriteSequences);
            
            UpdateSprites();
        }

        public void SetNextColour()
        {
            _currentColourId = GetNextId(_currentColourId, colours);
            UpdateColour();
        }
        
        public void SetPreviousColour()
        {
            _currentColourId = GetPreviousId(_currentColourId, colours);
            UpdateColour();
        }
        
        private void UpdateSprites()
        {
            onBodyPartChanged.RaiseEvent(_spriteSequences[_currentSpritesId]);
        }
        
        private void UpdateColour()
        {
            _spriteRenderer.color = colours[_currentColourId];
        }

        public async void UpdateGender()
        {
            _gender = _gender == Gender.Male ? Gender.Female : Gender.Male;
            
            Addressables.Release(_handle);

            await LoadSprites();
            
            if (_currentSpritesId >= _spriteSequences.Count)
            {
                _currentSpritesId = _spriteSequences.Count - 1;
            }
            
            UpdateSprites();
            UpdateColour();
        }

        private async Task LoadSprites()
        {
            var key = $"{_gender.ToString()} {bodyPart.ToString()}";
            
            _handle = Addressables.LoadAssetsAsync<SpriteLibraryAsset>(key, null);

            _spriteSequences = await _handle.Task;
        }
        
        private static int GetPreviousId<T>(int id, IEnumerable<T> collection)
        {
            var previousId = id - 1;
            
            if (previousId < 0)
            {
                previousId = collection.Count() - 1;
            }

            return previousId;
        }
        
        private static int GetNextId<T>(int id, IEnumerable<T> collection)
        {
            var nextId = id + 1;

            var items = collection.ToArray();
            if (nextId >= items.Length)
            {
                nextId = 0;
            }

            return nextId;
        }
    }
}
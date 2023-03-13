using System.Collections.Generic;
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
    internal sealed class BodyPartCustomizer : MonoBehaviour
    {
        [SerializeField] private BodyPart bodyPart;    
        [SerializeField] private Color[] colours;
        [SerializeField] private CosmeticEventChannelSO onBodyPartChanged;

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
            
            UpdateSprites(_currentSpritesId);
        }

        public void UpdateSprites(int id)
        {
            _currentSpritesId = id;
            onBodyPartChanged.RaiseEvent(_spriteSequences[id]);
        }

        public void UpdateColour(int id)
        {
            _currentColourId = id;
            _spriteRenderer.color = colours[id];
        }

        public async void UpdateGender(int gender)
        {
            _gender = (Gender)gender;
            
            Addressables.Release(_handle);

            await LoadSprites();
            
            UpdateSprites(_currentSpritesId);
            UpdateColour(_currentColourId);
        }

        private async Task LoadSprites()
        {
            var key = $"{_gender.ToString()} {bodyPart.ToString()}";
            
            _handle = Addressables.LoadAssetsAsync<SpriteLibraryAsset>(key, null);

            _spriteSequences = await _handle.Task;
        }
    }
}
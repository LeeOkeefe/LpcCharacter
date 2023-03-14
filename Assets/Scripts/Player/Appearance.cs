using System;
using System.Linq;
using System.Threading.Tasks;
using Events;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Player
{
    internal sealed class Appearance : MonoBehaviour
    {
        private readonly string[] _keys = { "Head", "Body", "Hair", "Torso", "Legs", "Feet" };
        
        private Gender CurrentGender => _currentGenderId == 0 ? Gender.Male : Gender.Female;
        
        private CosmeticRepository _cosmeticRepository;

        [SerializeField] private BodyPartEventChannelSO headChangedEventChannel;
        [SerializeField] private BodyPartEventChannelSO bodyChangedEventChannel;
        [SerializeField] private BodyPartEventChannelSO hairChangedEventChannel;
        [SerializeField] private BodyPartEventChannelSO torsoChangedEventChannel;
        [SerializeField] private BodyPartEventChannelSO legsChangedEventChannel;
        [SerializeField] private BodyPartEventChannelSO feetChangedEventChannel;

        private int _currentGenderId;
        private int _currentHairId;
        private int _currentTorsoId;
        private int _currentLegsId;
        private int _currentFeetId;

        private async void Start()
        {
            _cosmeticRepository = new CosmeticRepository(_keys.Select(k => $"{CurrentGender.ToString()} {k}"));
            await _cosmeticRepository.InitializeAsync();

            UpdateAll();
        }

        public async void UpdateGender()
        {
            _currentGenderId = _currentGenderId == 0 ? 1 : 0;

            _cosmeticRepository.Dispose();
            _cosmeticRepository = new CosmeticRepository(_keys.Select(k => $"{CurrentGender.ToString()} {k}"));
            await _cosmeticRepository.InitializeAsync();

            UpdateAll();
        }
        
        public async void UpdateHair()
        {
            var hair = await GetCosmeticItem(_currentHairId, _cosmeticRepository.GetHairByIdAsync);
            
            _currentHairId = hair.id;
            
            hairChangedEventChannel.RaiseEvent(hair.sprites);
        }

        public async void UpdateTorso()
        {
            var torso = await GetCosmeticItem(_currentTorsoId, _cosmeticRepository.GetTorsoByIdAsync);
            
            _currentTorsoId = torso.id;
            
            torsoChangedEventChannel.RaiseEvent(torso.sprites);
        }

        public async void UpdateLegs()
        {
            var legs = await GetCosmeticItem(_currentLegsId, _cosmeticRepository.GetLegsByIdAsync);
            
            _currentLegsId = legs.id;
            
            legsChangedEventChannel.RaiseEvent(legs.sprites);
        }

        public async void UpdateFeet()
        {
            var feet = await GetCosmeticItem(_currentFeetId, _cosmeticRepository.GetFeetByIdAsync);
            
            _currentFeetId = feet.id;
            
            feetChangedEventChannel.RaiseEvent(feet.sprites);
        }

        private async void UpdateAll()
        {
            var head = await GetCosmeticItem(0, _cosmeticRepository.GetHeadByIdAsync);
            var body = await GetCosmeticItem(0, _cosmeticRepository.GetBodyByIdAsync);

            headChangedEventChannel.RaiseEvent(head.sprites);
            bodyChangedEventChannel.RaiseEvent(body.sprites);
            
            UpdateHair();
            UpdateTorso();
            UpdateLegs();
            UpdateFeet();
        }

        private static async Task<(int id, SpriteLibraryAsset sprites)> GetCosmeticItem(int id, Func<int, Task<SpriteLibraryAsset>> request)
        {
            var sprites = await request.Invoke(id);

            if (sprites is not null)
            {
                id++;
                return (id, sprites);
            }
            
            id = 0;
            sprites = await request.Invoke(id);
            id++;
            
            return (id, sprites);
        }
    }
}

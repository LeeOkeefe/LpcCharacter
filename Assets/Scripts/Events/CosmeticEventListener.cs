using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.Animation;

namespace Events
{
    [Serializable]
    public class CosmeticEventSO : UnityEvent<SpriteLibraryAsset>
    {
    }
    
    public class CosmeticEventListener : MonoBehaviour
    {
        [SerializeField] private CosmeticEventChannelSO channel;
        
        public CosmeticEventSO onEventRaised;

        private void OnEnable()
        {
            if (onEventRaised is not null)
            {
                channel.OnEventRaised += Respond;
            }
        }

        private void OnDisable()
        {
            if (onEventRaised is not null)
            {
                channel.OnEventRaised -= Respond;
            }
        }

        private void Respond(SpriteLibraryAsset cosmeticSprites)
        {
            onEventRaised?.Invoke(cosmeticSprites);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.Animation;

namespace Events
{
    [Serializable]
    public class BodyPartEventSO : UnityEvent<SpriteLibraryAsset>
    {
    }
    
    public class BodyPartEventListener : MonoBehaviour
    {
        [SerializeField] private BodyPartEventChannelSO channel;
        
        public BodyPartEventSO onEventRaised;

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
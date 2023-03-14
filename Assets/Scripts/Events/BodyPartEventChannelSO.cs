using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.Animation;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Body Part Event Channel")]
    public class BodyPartEventChannelSO : ScriptableObject
    {
        public UnityAction<SpriteLibraryAsset> OnEventRaised;
	
        public void RaiseEvent(SpriteLibraryAsset cosmeticSprites)
        {
            OnEventRaised?.Invoke(cosmeticSprites);
        }
    }
}
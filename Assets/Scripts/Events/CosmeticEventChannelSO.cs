using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.Animation;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Cosmetic Event Channel")]
    public class CosmeticEventChannelSO : ScriptableObject
    {
        public UnityAction<SpriteLibraryAsset> OnEventRaised;
	
        public void RaiseEvent(SpriteLibraryAsset cosmeticSprites)
        {
            OnEventRaised?.Invoke(cosmeticSprites);
        }
    }
}
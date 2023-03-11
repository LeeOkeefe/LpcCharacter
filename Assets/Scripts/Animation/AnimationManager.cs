using UnityEngine;

namespace Animation
{
    internal sealed class AnimationManager : MonoBehaviour
    {
        [SerializeField] private int movementFramesPerSecond;
        [SerializeField] private float actionInterval;

        private BodyPartAnimator[] _bodyPartAnimators;

        private void Awake()
        {
            _bodyPartAnimators = GetComponentsInChildren<BodyPartAnimator>();
        }

        public void PlayAnimation(AnimationState state, Direction direction)
        {
            foreach (var animator in _bodyPartAnimators)
            {
                if (IsLoopingAnimation(state))
                {
                    animator.AnimateLoop(state, direction, movementFramesPerSecond);
                }
                else
                {
                    animator.AnimateSingle(state, direction, actionInterval);
                }
            }
        }

        public void PauseAnimation(bool pause)
        {
            foreach (var animator in _bodyPartAnimators)
            {
                animator.SetIsIdle(pause);
            }
        }

        private static bool IsLoopingAnimation(AnimationState state)
        {
            return state == AnimationState.Walk;
        }
    }
}
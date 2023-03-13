using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    internal sealed class BodyPartAnimator : MonoBehaviour
    {
        private SpriteLibraryAsset _spriteLibrary;
        private SpriteRenderer _spriteRenderer;
        private Sprite[] _currentSpriteSequence;
    
        private float _animationInterval = 0.3f;
        private bool _isRunning;

        private bool _isRunningLoop;
        private bool _isRunningSingle;
        private bool _isIdle;

        private AnimationState _lastState;
        private Direction _lastDirection;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_isRunningSingle || _isIdle || _currentSpriteSequence is null)
            {
                return;
            }
        
            if (_isRunningLoop)
            {
                var frame = (int)(Time.time * _animationInterval);
        
                frame %= _currentSpriteSequence.Length;

                _spriteRenderer.sprite = _currentSpriteSequence[frame];
            }
        }

        public void AnimateLoop(AnimationState state, Direction direction, int interval)
        {
            if (_isRunningSingle)
            {
                return;
            }

            _animationInterval = interval;
            _lastState = state;
            _lastDirection = direction;
        
            _currentSpriteSequence = GetSpriteSequence(state, direction);
            _isRunningLoop = true;
        }

        public void AnimateSingle(AnimationState state, Direction direction, float interval)
        {
            if (_isRunningSingle)
            {
                return;
            }
        
            _animationInterval = interval;
            _lastState = state;
            _lastDirection = direction;
            
            _currentSpriteSequence = GetSpriteSequence(state, direction);
            StartCoroutine(Animate());
        }

        public void SetIsIdle(bool isIdle)
        {
            if (_currentSpriteSequence is null || _currentSpriteSequence.Length == 0 || isIdle == _isIdle)
            {
                return;
            }

            _isIdle = isIdle;

            _spriteRenderer.sprite = _currentSpriteSequence[0];
        }

        public void UpdateAnimationLibrary(SpriteLibraryAsset cosmeticSprites)
        {
            _spriteLibrary = cosmeticSprites;
            _currentSpriteSequence = GetSpriteSequence(_lastState, _lastDirection);
            _spriteRenderer.sprite = _currentSpriteSequence[0];
        }

        public void UpdateColour(Color color)
        {
            _spriteRenderer.color = color;
        }
    
        private Sprite[] GetSpriteSequence(AnimationState state, Direction direction)
        {
            var stateName = state.ToString();
            var sprites = _spriteLibrary.GetCategoryLabelNames(stateName)
                .Select(label => _spriteLibrary.GetSprite(stateName, label))
                .ToArray();

            var rowLength = sprites.Length / 4;

            if (rowLength == 1)
            {
                return sprites;
            }

            return direction switch
            {
                Direction.Up => sprites[..rowLength],
                Direction.Left => sprites[rowLength..(rowLength * 2)],
                Direction.Down => sprites[(rowLength * 2)..(rowLength * 3)],
                Direction.Right => sprites[(rowLength * 3)..(rowLength * 4)],
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    
        private IEnumerator Animate()
        {
            if (_isRunningSingle)
            {
                yield break;
            }

            _isRunningSingle = true;

            var index = 0;
        
            while (_isRunningSingle)
            {
                _spriteRenderer.sprite = _currentSpriteSequence[index];

                yield return new WaitForSeconds(_animationInterval);

                index++;

                if (index < _currentSpriteSequence.Length)
                {
                    continue;
                }

                _isRunningSingle = false;
            }
        }
    }
}
using Animation;
using UnityEngine;
using AnimationState = Animation.AnimationState;

namespace Player
{
    internal sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 1.5f;

        private Rigidbody2D _rb;
        private AnimationManager _animationManager;
        private Vector2 _movement;

        private Direction _lastDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animationManager = GetComponent<AnimationManager>();
        }

        private void FixedUpdate()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            _rb.MovePosition(_rb.position + _movement * speed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            Animate();
        }

        private void Animate()
        {
            if (_movement.x == 0 && _movement.y == 0)
            {
                _animationManager.PauseAnimation(true);
                return;
            }
        
            _animationManager.PauseAnimation(false);
        
            if (_movement.x > 0 && _movement.y == 0)
            {
                MovementAnimation(Direction.Right);
            }
        
            if (_movement.x < 0 && _movement.y == 0)
            {
                MovementAnimation(Direction.Left);
            }
        
            if (_movement.x == 0 && _movement.y > 0)
            {
                MovementAnimation(Direction.Up);
            }
        
            if (_movement.x == 0 && _movement.y < 0)
            {
                MovementAnimation(Direction.Down);
            }
        }

        private void MovementAnimation(Direction direction)
        {
            _animationManager.PlayAnimation(AnimationState.Walk, direction);
            _lastDirection = direction;
        }

        // private void UpdateAnimatorState(AnimationState state, Direction direction)
        // {
        //     foreach (var animator in _animators)
        //     {
        //         animator.UpdateState(state, direction);
        //     }
        //
        //     _lastDirection = direction;
        // }
    }
}

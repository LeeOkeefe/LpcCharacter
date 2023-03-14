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
        private bool IsIdle => _movement is { x: 0, y: 0 };

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
            _lastDirection = GetDirection;

            if (IsIdle)
            {
                _animationManager.PauseAnimation(true);
                return;
            }
            
            _animationManager.PauseAnimation(false);
            _animationManager.PlayAnimation(AnimationState.Walk, _lastDirection);
        }
        
        private Direction GetDirection => (_movement.x, _movement.y) switch
        {
            (> 0f, 0f) => Direction.Right,
            (< 0f, 0f) => Direction.Left,
            (0f, > 0f) => Direction.Up,
            (0f, < 0f) => Direction.Down,
            _ => _lastDirection
        };
    }
}

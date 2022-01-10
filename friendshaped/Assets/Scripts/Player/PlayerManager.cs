using UnityEngine;

namespace Player {

    public enum MovementStates { NONE, WALKING }
    public class PlayerManager : MonoBehaviour {
        
        [SerializeField] private MovementStates playerState;
        private PlayerControls controls;
        private float speed;
        [SerializeField] [Range(1f,10)] private float speedModifier;

        private bool isMovingLeft;
        private bool isMovingRight;

        public MovementStates PlayerState { get => playerState; set => playerState = value;}

        private Rigidbody2D rb2D;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Vector2 velocity;

        private bool hasMoved = false;

        // Start is called before the first frame update
        void Awake() {
            rb2D = GetComponent<Rigidbody2D>();
            controls = new PlayerControls();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            
            // Input callbacks
            controls.movement.walkLeft.started += ctx => isMovingLeft = true;
            controls.movement.walkRight.started += ctx => isMovingRight = true;
            
            controls.movement.walkLeft.canceled += ctx => isMovingLeft = false;
            controls.movement.walkRight.canceled += ctx => isMovingRight = false;
        }

        private void FixedUpdate() {
            switch (isMovingLeft, isMovingRight)
            {
                // Left
                case (true, false):
                    playerState = MovementStates.WALKING;
                    velocity = new Vector2(-1f * speedModifier, 0f);
                    spriteRenderer.flipX = true;
                    animator.Play("playerMove");
                    break;
                // Right
                case (false, true):
                    playerState = MovementStates.WALKING;
                    velocity = new Vector2(speedModifier, 0f);
                    spriteRenderer.flipX = false;
                    animator.Play("playerMove");
                    break;
                // Not moving
                default:
                    playerState = MovementStates.NONE;
                    velocity = new Vector2(0f, 0f);
                    animator.Play("playerIdle");
                    break;
            }

            rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);

            if (!hasMoved && playerState == MovementStates.WALKING)
            {
                hasMoved = !hasMoved;
                GameManager.Instance.NextPrompt();
            }
        }

        // Required for the input system.
        void OnEnable() {
            controls.Enable();
        }

        void OnDisable() {
            controls.Disable();
        }
    }
}   

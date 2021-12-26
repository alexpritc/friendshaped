using UnityEngine;

namespace Player {
    public class PlayerManager : MonoBehaviour {
        
        public enum MovementStates { NONE, WALKING}
        
        [SerializeField] private MovementStates playerState;
        private PlayerControls controls;
        private float speed;
        [SerializeField] [Range(1f,10)] private float speedModifier;

        private bool isMovingLeft;
        private bool isMovingRight;

        [SerializeField] private Item newItem;

        public MovementStates PlayerState { get => playerState; set => playerState = value;}

        private Rigidbody2D rb2D;
        private Vector2 velocity;

        private bool hasMoved = false;

        // Start is called before the first frame update
        void Awake() {
            rb2D = GetComponent<Rigidbody2D>();
            controls = new PlayerControls();

            // Input callbacks
            controls.movement.walkLeft.started += ctx => isMovingLeft = true;
            controls.movement.walkRight.started += ctx => isMovingRight = true;
            
            controls.movement.walkLeft.canceled += ctx => isMovingLeft = false;
            controls.movement.walkRight.canceled += ctx => isMovingRight = false;

            controls.inventory.pickUpItem.performed += ctx => PickUpItem();
        }

        private void FixedUpdate() {
            switch (isMovingLeft, isMovingRight)
            {
                case (true, false):
                    playerState = MovementStates.WALKING;
                    velocity = new Vector2(-1f * speedModifier, 0f);
                    break;
                case (false, true):
                    playerState = MovementStates.WALKING;
                    velocity = new Vector2(speedModifier, 0f);
                    break;
                default:
                    playerState = MovementStates.NONE;
                    velocity = new Vector2(0f, 0f);
                    break;
            }

            rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);

            if (!hasMoved && playerState == MovementStates.WALKING)
            {
                hasMoved = !hasMoved;
                GameManager.Instance.NextPrompt();
            }
        }

        void PickUpItem() {
            if (newItem == null) return;
            GameManager.Instance.PickUpItem(newItem);
            GameManager.Instance.MakeItemCarryOver(newItem);
            newItem = null;
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

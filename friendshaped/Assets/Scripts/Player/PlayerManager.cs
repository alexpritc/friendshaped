using UnityEngine;

namespace Player {
    public class PlayerManager : MonoBehaviour {
        
        public enum MovementStates { NONE, WALKING}
        
        [SerializeField] private MovementStates playerState;
        private PlayerControls controls;
        private float speed;
        [SerializeField] [Range(1f,2.5f)] private float speedModifier;

        private bool isMovingLeft;
        private bool isMovingRight;

        [SerializeField] private Item newItem;

        public MovementStates PlayerState { get => playerState; set => playerState = value;}

        // Start is called before the first frame update
        void Awake() {
            controls = new PlayerControls();

            // Input callbacks
            controls.movement.walkLeft.started += ctx => isMovingLeft = true;
            controls.movement.walkRight.started += ctx => isMovingRight = true;
            
            controls.movement.walkLeft.canceled += ctx => isMovingLeft = false;
            controls.movement.walkRight.canceled += ctx => isMovingRight = false;

            controls.inventory.pickUpItem.performed += ctx => PickUpItem();
        }

        private void Update() {

            switch (isMovingLeft, isMovingRight) {
                case (true,false):
                    playerState = MovementStates.WALKING; 
                    speed = -0.01f * speedModifier;
                    break;
                case (false,true):
                    playerState = MovementStates.WALKING; 
                    speed = 0.01f * speedModifier;
                    break;
                default:
                    playerState = MovementStates.NONE; 
                    speed = 0f * speedModifier;
                    break;
            }
            
            Move(speed);
        }
 
        void Move(float x) {
            transform.position += new Vector3(x, 0f, 0f);
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

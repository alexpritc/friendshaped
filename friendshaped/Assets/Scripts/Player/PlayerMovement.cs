using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class PlayerMovement : MonoBehaviour {
        
        public enum MovementStates { NONE, WALKING}
        
        [SerializeField] private MovementStates playerState;
        private PlayerControls controls;
        private float speed;

        private bool isMovingLeft;
        private bool isMovingRight;
    
        // Start is called before the first frame update
        void Awake() {
            controls = new PlayerControls();

            // Input callbacks
            controls.movement.walkLeft.started += ctx => isMovingLeft = true;
            controls.movement.walkRight.started += ctx => isMovingRight = true;
            controls.movement.walkLeft.canceled += ctx => isMovingLeft = false;
            controls.movement.walkRight.canceled += ctx => isMovingRight = false;
        }

        private void Update() {

            switch (isMovingLeft, isMovingRight) {
                case (true,false):
                    playerState = MovementStates.WALKING; 
                    speed = -0.01f;
                    break;
                case (false,true):
                    playerState = MovementStates.WALKING; 
                    speed = 0.01f;
                    break;
                default:
                    playerState = MovementStates.NONE; 
                    speed = 0f;
                    break;
            }
            
            Move(speed);
        }
 
        void Move(float x) {
            transform.position += new Vector3(x, 0f, 0f);
        }

        // Required for the input system.
        void OnEnable() {
            controls.movement.Enable();
        }

        void OnDisable() {
            controls.movement.Disable();
        }
    }   

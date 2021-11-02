using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CameraMovement : MonoBehaviour {
        
        [SerializeField] private Transform target;
        private Vector3 targetPosition;

        // Update is called once per frame
        void Update() {
            targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.005f);
        }
    }   

//-----------------------------------------------------------------------------
// MoveCamera.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Basic camera movement
//-----------------------------------------------------------------------------

using UnityEngine;

namespace ParallaxiumBeta
{
    public class MoveCamera : MonoBehaviour
    {
        public float speed = 0.05f;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 500);
            }
        }
    }
}

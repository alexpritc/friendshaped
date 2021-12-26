using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Room roomOne;
        [SerializeField] private Room roomTwo;

        // TODO: Change to OnTriggerStay2D
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // TODO: Add user interaction rather than
            if (collision.tag == "Player"){
                ToggleFog(roomOne, roomTwo);
                MovePlayer(collision, roomTwo);
            }
        }

        // Changes which room is visible
        private void ToggleFog(Room roomA, Room roomB)
        {

            if (roomA.IsRevealed && !roomB.IsRevealed)
            {

                roomB.RevealRoom();
                roomA.HideRoom();
            }
            else
            {
                roomA.RevealRoom();
                roomB.HideRoom();
            }
        }

        // Move Player
        private void MovePlayer(Collider2D collision, Room roomB)
        {
            Vector2 newPos;

            if (gameObject.name.Contains("Left"))
            {
                newPos = new Vector2(roomB.rightDoor.position.x, collision.gameObject.transform.position.y);
            }
            else
            {
                newPos = new Vector2(roomB.leftDoor.position.x, collision.gameObject.transform.position.y);
            }

            collision.transform.position = newPos;
            Camera.main.transform.position = new Vector3(collision.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
    }
}
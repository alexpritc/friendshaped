using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Room roomOne;
        [SerializeField] private Room roomTwo;

        [SerializeField] private GameObject player;

        public void UseDoor()
        {
            ToggleFog(roomOne, roomTwo);
            MovePlayer(player, roomTwo);
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
        private void MovePlayer(GameObject player, Room roomB)
        {
            Vector2 newPos;

            if (gameObject.name.Contains("Left"))
            {
                newPos = new Vector2(roomB.rightDoor.position.x, player.gameObject.transform.position.y);
            }
            else
            {
                newPos = new Vector2(roomB.leftDoor.position.x, player.gameObject.transform.position.y);
            }

            player.transform.position = newPos;
            Camera.main.transform.position = new Vector3(player.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
    }
}
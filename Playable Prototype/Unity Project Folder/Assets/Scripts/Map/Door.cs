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


        public SpriteRenderer doorwayLineSprite;
        public Color highlightedColour = new Color(0.86f, 0.65f, 0.19f, 1f);


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

        private void Start()
        {
            doorwayLineSprite = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            doorwayLineSprite.color = highlightedColour;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            doorwayLineSprite.color = Color.white;
        }
    }
}
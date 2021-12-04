using UnityEngine;

namespace Map {
    public class FogManager : MonoBehaviour {
        [SerializeField] private Room roomOne;
        [SerializeField] private Room roomTwo;
    
        // Changes which room is visible
        public void ToggleFog(Room roomA, Room roomB) {

            if (roomOne.IsRevealed && !roomTwo.IsRevealed) {
                
                roomTwo.RevealRoom();
                roomOne.HideRoom();
            }
            else {
                roomOne.RevealRoom();
                roomTwo.HideRoom();
            }
        }
    }
}

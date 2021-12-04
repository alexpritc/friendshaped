using UnityEngine;

namespace Map {
    public class FogManager : MonoBehaviour {
        [SerializeField] private Room roomOne;
        [SerializeField] private Room roomTwo;
    
        // Changes which room is visible
        public void CheckFog() {

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

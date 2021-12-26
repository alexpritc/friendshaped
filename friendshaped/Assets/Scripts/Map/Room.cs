using UnityEngine;

namespace Map {
    public class Room : MonoBehaviour {
        [SerializeField] private bool isRevealed;
        [SerializeField] private GameObject outerCar;

        public Transform leftDoor;
        public Transform rightDoor;
        public bool IsRevealed { get => isRevealed; set => isRevealed = value; }

        public void RevealRoom() {
            outerCar.SetActive(false);
            IsRevealed = true;
        }
        
        public void HideRoom() {
            outerCar.SetActive(true);
            IsRevealed = false;
        }
    }
}

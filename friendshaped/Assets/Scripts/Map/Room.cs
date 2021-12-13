using UnityEngine;

namespace Map {
    public class Room : MonoBehaviour {
        [SerializeField] private bool isRevealed;
        [SerializeField] private GameObject outerCar;
        public bool IsRevealed { get => isRevealed; set => isRevealed = value; }

        public void RevealRoom() {
            outerCar.SetActive(true);
            IsRevealed = true;
        }
        
        public void HideRoom() {
            outerCar.SetActive(false);
            IsRevealed = false;
        }
    }
}

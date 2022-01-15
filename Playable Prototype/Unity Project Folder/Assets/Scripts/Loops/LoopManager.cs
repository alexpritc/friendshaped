using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Loops {
    public class LoopManager : MonoBehaviour {

        private int actionsTaken = 0;
        private int actionsLimit = 1;
    
        void Awake() {
            GameManager.Instance.onLoopComplete += OnLoopComplete;
            GameManager.Instance.onAllActionsComplete += OnAllActionsComplete;
            GameManager.Instance.onActionCompleted += OnActionCompleted;
        }
    
        private void OnLoopComplete() {
            Debug.Log("restarting loop...");
        
            // restart loop but with carry over item
            Inventory.Instance.HeldItems = new List<Item> {Inventory.Instance.CarryOverItem};
            Debug.Log(string.Format("Player has carried over a {0}",Inventory.Instance.CarryOverItem.name));
        }
    
        private void OnAllActionsComplete() {
            Debug.Log("all actions completed");
            OnLoopComplete();
        }

        private void OnActionCompleted() {
            actionsTaken++;
            Debug.Log(string.Format("completed action #{0}", actionsTaken));
            if (actionsTaken >= actionsLimit) {
                OnAllActionsComplete();
            }
        }

        private void OnDestroy() {
            GameManager.Instance.onLoopComplete -= OnLoopComplete;
            GameManager.Instance.onAllActionsComplete -= OnAllActionsComplete;
            GameManager.Instance.onActionCompleted -= OnActionCompleted;
        }
    }
}

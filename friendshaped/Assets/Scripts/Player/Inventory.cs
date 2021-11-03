using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Inventory : MonoBehaviour {
        [SerializeField] private List<Item> heldItems;
        [SerializeField] private Item carryOverItem;

        void Start() {
            GameManager.Instance.onPickUpItem += PickUpItem;
        }

        private void PickUpItem(Item item) {
            heldItems.Add(item);
        }
    }
}

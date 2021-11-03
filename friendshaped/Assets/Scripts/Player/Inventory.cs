using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Inventory : MonoBehaviour {
        
        public static Inventory instance;
        public static Inventory Instance { get => instance; set => instance = value; }
        
        private List<Item> heldItems;
        public List<Item> HeldItems { get => heldItems; set => heldItems = value; }
        
        private Item carryOverItem;
        public Item CarryOverItem { get => carryOverItem; set => carryOverItem = value; }

        private void Awake() {
            if(instance != null) {
                Destroy(instance.gameObject);
            }
            instance = this;
            heldItems = new List<Item>();
        }

        void Start() {
            GameManager.Instance.onPickUpItem += PickUpItem;
            GameManager.Instance.onMakeItemCarryOver += MakeItemCarryOver;
        }

        private void PickUpItem(Item item) {
            heldItems.Add(item);
        }

        void MakeItemCarryOver(Item item) {
            carryOverItem = item;
        }
    }
} 

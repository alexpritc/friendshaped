using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Player
{
    public class Interactions : MonoBehaviour
    {
        PlayerControls controls;

        bool canInteract;

        private GameObject interactWith;

        [SerializeField] private TextAsset script;

        void Awake()
        {
            controls = new PlayerControls();
            controls.interactions.interact.performed += ctx => Interact();
        }

        private void Interact()
        {
            if (canInteract)
            {
                switch (interactWith.tag)
                {
                    case "Door":
                        interactWith.GetComponent<Door>().UseDoor();
                        break;
                    case "NPC":
                        // Pick a script to play
                        
                        // Start dialogue
                        GameManager.Instance.TalkToNPC(script);
                        break;
                    case "Item":
                        GameManager.Instance.PickUpItem(interactWith.GetComponent<Item>());
                        Destroy(interactWith);
                        // Start interaction
                        break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            canInteract = true;

            interactWith = collision.gameObject;

            if (collision.tag == "Door")
            {
                GameManager.Instance.CreatePrompt(transform.position + new Vector3(1, 2, 0), "Use Door", PromptKeys.E);
            }
            else if (collision.tag == "NPC")
            {
                GameManager.Instance.CreatePrompt(transform.position + new Vector3(1, 2, 0), "Talk", PromptKeys.E);
            }
            else if (collision.tag == "Item")
            {
                GameManager.Instance.CreatePrompt(transform.position + new Vector3(1, 2, 0), "Inspect", PromptKeys.E);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameManager.Instance.RemovePrompt();
            canInteract = false;
            interactWith = null;
        }


        // Required for the input system.
        void OnEnable()
        {
            controls.Enable();
        }

        void OnDisable()
        {
            controls.Disable();
        }
    }
}
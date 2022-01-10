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

        private NPC npc;

        void Awake()
        {
            controls = new PlayerControls();
            controls.interactions.interact.performed += ctx => Interact();
        }

        private void Interact()
        {
            if (canInteract && !GameManager.Instance.isChatWindowActive)
            {
                switch (interactWith.tag)
                {
                    case "Door":
                        interactWith.GetComponent<Door>().UseDoor();
                        break;
                    case "NPC":
                        npc = interactWith.GetComponent<NPC>();
                        GameManager.Instance.TalkToNPC(npc.myIntro, npc.chatWindowBackground, npc.chatWindowSprite);
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
                GameManager.Instance.CreatePrompt(transform.position + new Vector3(0.5f, 3, 0), "Use Door", PromptKeys.E);
            }
            else if (collision.tag == "NPC")
            {
                GameManager.Instance.CreatePrompt(transform.position + new Vector3(0.5f, 3, 0), "Talk", PromptKeys.E);
            }
            else if (collision.tag == "Item")
            {
                GameManager.Instance.CreatePrompt(transform.position + new Vector3(0.5f, 3, 0), "Inspect", PromptKeys.E);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private GameObject chatWindowPrefab;
    [SerializeField] private Sprite playerSprite;

    void Start() {
        GameManager.Instance.onTalkToNPC += InstantiateChatInstance;
        GameManager.Instance.onStopTalkingToNPC += CloseChatInstance;
        
    }

    private void InstantiateChatInstance(TextAsset script, Sprite chatBackground, Sprite chatSprite)
    {
        GameObject instance = Instantiate(chatWindowPrefab);
        
        dialogueManager = instance.GetComponent<DialogueManager>();
        dialogueManager.inkJSONAsset = script;
        dialogueManager.SetImages(chatBackground, playerSprite, chatSprite);
    }

    private void CloseChatInstance(GameObject window)
    {
        Destroy(window);
    }
}

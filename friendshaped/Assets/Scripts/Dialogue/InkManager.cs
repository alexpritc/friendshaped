using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private GameObject chatWindowInstance;
    [SerializeField] private Sprite playerSprite;
    public TextAsset script;

    void Start() {
        GameManager.Instance.onTalkToNPC += OpenChatInstance;
        GameManager.Instance.onStopTalkingToNPC += CloseChatInstance;
        
        //script = Resources.Load<TextAsset>("Ink Scripts/AllAboard.json");
        dialogueManager = chatWindowInstance.GetComponent<DialogueManager>();
        dialogueManager.inkJSONAsset = script;
    }

    // private void InstantiateChatInstance(TextAsset script, String inkKnot, Sprite chatBackground, Sprite chatSprite)
    // {
    //     GameObject instance = Instantiate(chatWindowPrefab);
    //     
    //     dialogueManager = instance.GetComponent<DialogueManager>();
    //     dialogueManager.SetImages(chatBackground, playerSprite, chatSprite);
    //     dialogueManager.inkJSONAsset = script;
    //     dialogueManager.story.ChoosePathString(inkKnot);
    //     dialogueManager.RefreshView();
    // }

    private void OpenChatInstance(String inkKnot, Sprite chatBackground, Sprite chatSprite)
    {
        chatWindowInstance.SetActive(true);
        dialogueManager.SetImages(chatBackground, playerSprite, chatSprite);
        dialogueManager.story.ChoosePathString(inkKnot);
        dialogueManager.RefreshView();
    }
    
    private void CloseChatInstance(GameObject window)
    {
        chatWindowInstance.SetActive(false);
    }
}

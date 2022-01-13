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
        
        dialogueManager = chatWindowInstance.GetComponent<DialogueManager>();
        dialogueManager.inkJSONAsset = script;
    }
    
    private void OpenChatInstance(String inkKnot, Sprite chatBackground, Sprite chatSprite, AudioClip chatBackgroundMusic)
    {
        chatWindowInstance.SetActive(true);
        chatWindowInstance.GetComponent<AudioSource>().clip = chatBackgroundMusic;
        chatWindowInstance.GetComponent<AudioSource>().Play();
        dialogueManager.SetImages(chatBackground, playerSprite, chatSprite);
        dialogueManager.story.ChoosePathString(inkKnot);
        dialogueManager.RefreshView();
    }
    
    private void CloseChatInstance(GameObject window)
    {
        chatWindowInstance.SetActive(false);
    }
}

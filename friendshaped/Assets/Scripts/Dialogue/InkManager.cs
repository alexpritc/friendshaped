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
    public GameObject bgMusic;

    void Start() {
        GameManager.Instance.onTalkToNPC += OpenChatInstance;
        GameManager.Instance.onStopTalkingToNPC += CloseChatInstance;
        
        dialogueManager = chatWindowInstance.GetComponent<DialogueManager>();
        dialogueManager.inkJSONAsset = script;
    }
    
    private void OpenChatInstance(String inkKnot, Sprite chatBackground, Sprite chatSprite, AudioClip chatBackgroundMusic)
    {
        chatWindowInstance.SetActive(true);
        UpdateRectTransform();
        chatWindowInstance.GetComponent<AudioSource>().clip = chatBackgroundMusic;
        bgMusic.SetActive(false);
        chatWindowInstance.GetComponent<AudioSource>().Play();
        dialogueManager.SetImages(chatBackground, playerSprite, chatSprite);
        dialogueManager.story.ChoosePathString(inkKnot);
        dialogueManager.RefreshView();
    }
    
    private void CloseChatInstance(GameObject window)
    {
        chatWindowInstance.GetComponent<AudioSource>().Pause();
        bgMusic.SetActive(true);
        chatWindowInstance.SetActive(false);
    }

    private void UpdateRectTransform()
    {
        RectTransform rect = chatWindowInstance.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localScale = Vector3.one;
        rect.sizeDelta = new Vector2 (100, 100);
        
    }
}

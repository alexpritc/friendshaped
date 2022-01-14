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
        RectTransform thisRect = chatWindowInstance.GetComponent<RectTransform>();
        thisRect.pivot = new Vector2(0.5f, 0.5f);
        thisRect.localScale = Vector3.one;
        thisRect.sizeDelta = new Vector2 (100, 100);
        thisRect.ForceUpdateRectTransforms();
        Debug.Log("position:" + thisRect.position);
        Debug.Log("localScale:" + thisRect.localScale);
        Debug.Log("sizeDelta:" + thisRect.sizeDelta);
        Debug.Log("pivot:" + thisRect.pivot);
        Debug.Log(thisRect.gameObject.name + ":");
        Debug.Log("-----------------------------");
        Debug.Log("-----------------------------");
        
        foreach (RectTransform rect in chatWindowInstance.GetComponentInChildren<RectTransform>())
        {
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localScale = Vector3.one;
            rect.sizeDelta = new Vector2 (100, 100);
            rect.ForceUpdateRectTransforms();
            Debug.Log("position:" + rect.position);
            Debug.Log("localPosition:" + rect.localPosition);
            Debug.Log("localScale:" + rect.localScale);
            Debug.Log("sizeDelta:" + rect.sizeDelta);
            Debug.Log("pivot:" + rect.pivot);
            Debug.Log(rect.gameObject.name + ":");
            Debug.Log("-----------------------------");
            Debug.Log("-----------------------------");
        }
    }
}

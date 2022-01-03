using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private GameObject chatWindow;
    [SerializeField] private TextAsset[] inkJSONAssets = null;

    void Awake() {
        GameManager.Instance.onTalkToNPC += InstantiateChatInstance;
    }

    private void InstantiateChatInstance()
    {
        
    }
}

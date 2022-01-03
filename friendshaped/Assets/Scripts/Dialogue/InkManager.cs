using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private GameObject chatWindowPrefab;

    void Start() {
        GameManager.Instance.onTalkToNPC += InstantiateChatInstance;
    }

    private void InstantiateChatInstance(TextAsset script)
    {
        GameObject instance = Instantiate(chatWindowPrefab);
        
        dialogueManager = instance.GetComponent<DialogueManager>();
        dialogueManager.inkJSONAsset = script;
    }
}

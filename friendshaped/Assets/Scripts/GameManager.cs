using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum PromptKeys { A, D, E, Right, Left }
[System.Serializable]
public struct KeySprites
{
    public Sprite AKey;
    public Sprite DKey;
    public Sprite EKey;
    public Sprite RightArrow;
    public Sprite LeftArrow;
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }

    [SerializeField]
    private KeySprites keySprites;

    public GameObject Prompt;
    public GameObject OneKeyPrompt;
    public GameObject TwoKeyPrompt;
    public GameObject PromptCanvas;
    public GameObject CurrentPrompt;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Slider loopUI;
    private float timer;
    [SerializeField]
    private float timeModifier = 2f;

    [Header("Objectives")]
    private bool foundMurderer;
    private bool foundWeapon;
    private bool foundLocation;

    void Awake() {
        if(instance != null) {
            Destroy(instance.gameObject);
        }
        instance = this;

        CreatePrompt(player.transform.position + new Vector3(1, 2, 0), "Move", PromptKeys.A, PromptKeys.D);
    }

    private void Update()
    {
        //if (player.GetComponent<PlayerManager>().PlayerState == MovementStates.WALKING)
        //{
            timer += Time.deltaTime * timeModifier;
            loopUI.value = timer;

            if (timer >= loopUI.maxValue)
            {
                OnLoopComplete();
            }
        //}
    }

    private void OnLoopComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextPrompt()
    {
        RemovePrompt();
    }

    public void CreatePrompt(Vector3 position, string message)
    {
        if (CurrentPrompt == null)
        {
            CurrentPrompt = Instantiate(Prompt, PromptCanvas.transform);
            CurrentPrompt.GetComponent<RectTransform>().anchoredPosition = WorldToPromtUI(position);
            TextMeshProUGUI promptText = CurrentPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            promptText.text = message;
        }
    }

    public void CreatePrompt(Vector3 position, string message, PromptKeys keyForPrompt)
    {
        if (CurrentPrompt == null)
        {
            CurrentPrompt = Instantiate(OneKeyPrompt, PromptCanvas.transform);
            CurrentPrompt.GetComponent<RectTransform>().anchoredPosition = WorldToPromtUI(position);
            Image keySpriteImage = CurrentPrompt.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI promptText = CurrentPrompt.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            promptText.text = message;

            switch (keyForPrompt)
            {
                case PromptKeys.A:
                    keySpriteImage.sprite = keySprites.AKey;
                    break;
                case PromptKeys.D:
                    keySpriteImage.sprite = keySprites.DKey;
                    break;
                case PromptKeys.E:
                    keySpriteImage.sprite = keySprites.EKey;
                    break;
                case PromptKeys.Left:
                    keySpriteImage.sprite = keySprites.LeftArrow;
                    break;
                case PromptKeys.Right:
                    keySpriteImage.sprite = keySprites.RightArrow;
                    break;
                default:
                    break;
            }
        }
    }

    public void CreatePrompt(Vector3 position, string message, PromptKeys keyForPromptOne, PromptKeys keyForPromptTwo)
    {
        if (CurrentPrompt == null)
        {
            CurrentPrompt = Instantiate(TwoKeyPrompt, PromptCanvas.transform);
            CurrentPrompt.GetComponent<RectTransform>().anchoredPosition = WorldToPromtUI(position);
            Image keyOneSpriteImage = CurrentPrompt.transform.GetChild(0).GetComponent<Image>();
            Image keyTwoSpriteImage = CurrentPrompt.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI promptText = CurrentPrompt.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            promptText.text = message;

            switch (keyForPromptOne)
            {
                case PromptKeys.A:
                    keyOneSpriteImage.sprite = keySprites.AKey;
                    break;
                case PromptKeys.D:
                    keyOneSpriteImage.sprite = keySprites.DKey;
                    break;
                case PromptKeys.E:
                    keyOneSpriteImage.sprite = keySprites.EKey;
                    break;
                case PromptKeys.Left:
                    keyOneSpriteImage.sprite = keySprites.LeftArrow;
                    break;
                case PromptKeys.Right:
                    keyOneSpriteImage.sprite = keySprites.RightArrow;
                    break;
                default:
                    break;
            }

            switch (keyForPromptTwo)
            {
                case PromptKeys.A:
                    keyTwoSpriteImage.sprite = keySprites.AKey;
                    break;
                case PromptKeys.D:
                    keyTwoSpriteImage.sprite = keySprites.DKey;
                    break;
                case PromptKeys.E:
                    keyTwoSpriteImage.sprite = keySprites.EKey;
                    break;
                case PromptKeys.Left:
                    keyTwoSpriteImage.sprite = keySprites.LeftArrow;
                    break;
                case PromptKeys.Right:
                    keyTwoSpriteImage.sprite = keySprites.RightArrow;
                    break;
                default:
                    break;
            }
        }
    }

    public void RemovePrompt()
    {
        Destroy(CurrentPrompt);
    }

    private Vector2 WorldToPromtUI(Vector3 position)
    {
        RectTransform CanvasRect = PromptCanvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        return WorldObject_ScreenPosition;
    }

    public event Action onLoopComplete;
    public void LoopComplete(){
        if (onLoopComplete != null) {
            onLoopComplete();
        }
    }
    
    public event Action onAllActionsComplete;
    public void AllActionsComplete(){
        if (onAllActionsComplete != null) {
            onAllActionsComplete();
        }
    }
    
    public event Action onActionCompleted;
    public void ActionCompleted(){
        if (onActionCompleted != null) {
            onActionCompleted();
        }
    }
    
    public event Action<Item> onPickUpItem;
    public void PickUpItem(Item item){
        if (onPickUpItem != null) {
            onPickUpItem(item);
        }
    }
    
    public event Action<Item> onMakeItemCarryOver;
    public void MakeItemCarryOver(Item item){
        if (onMakeItemCarryOver != null) {
            onMakeItemCarryOver(item);
        }
    }
    
    public event Action onTalkToNPC;
    public void TalkToNPC(){
        if (onTalkToNPC != null) {
            onTalkToNPC();
        }
    }
}

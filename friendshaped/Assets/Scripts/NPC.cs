using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Chat Window")]
    public Sprite chatWindowSprite;
    public Sprite chatWindowBackground;
    
    public TextAsset script;
    [Header("Ink Knots")]
    public string myIntro;

    //_inkStory.ChoosePathString("LadyIntro");
    //_inkStory.ChoosePathString("myKnotName");
    
    [Header("In-Game")]
    public Sprite mainSprite;
    public Sprite interactionSprite;

    private void Start()
    {
        Resources.Load<TextAsset>("Ink Scripts/AllAboard.json");
    }
}

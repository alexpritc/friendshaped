using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Chat Window")]
    public Sprite chatWindowSprite;
    public Sprite chatWindowBackground;
    
    [Header("Ink Knots")]
    public string myIntro;
    
    [Header("In-Game")]
    public SpriteRenderer outlineSprite;
    public Color highlightedColour = new Color(0.86f,0.65f,0.19f,1f);

    private void Start()
    {
        outlineSprite = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        outlineSprite.color = highlightedColour;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        outlineSprite.color = Color.white;
    }
}

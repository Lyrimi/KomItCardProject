using UnityEngine;
using System.Collections.Generic;
using System;

public class CardSlot : MonoBehaviour
{
    CardOrginiser cardOrginiser;
    Dictionary<string, Sprite> SpriteDictionary;
    SpriteRenderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteDictionary = Globals.SpriteDictionary;
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTexutre(String element)
    {
        rend.sprite = SpriteDictionary[element];
    }
}

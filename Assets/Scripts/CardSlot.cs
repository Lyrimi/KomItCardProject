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
        cardOrginiser = FindFirstObjectByType<CardOrginiser>();
        SpriteDictionary = cardOrginiser.SpriteDictionary;
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTexutre(String element)
    {
        print(SpriteDictionary);
        rend.sprite = SpriteDictionary[element];
    }
}

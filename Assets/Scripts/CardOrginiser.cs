using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardOrginiser : MonoBehaviour
{
    [SerializeField] public List<GameObject> Cards;
    [SerializeField] Transform BasePostion;
    [SerializeField] float Spaceing;
    [SerializeField] private GameObject CardTemplate;
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private string[] Keys;
    Dictionary<string, Sprite> SpriteDictionary;
    public int test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {
        BuildDictionary();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCards()
    {
        int count = Cards.Count;
        float width = 1;
        for (int i = 0; i < count; i++)
        {
            GameObject card = Cards[i];
            float ofset = -((count - 1) * Spaceing / 2 + width * (count - 1) / 2);
            card.transform.position = new Vector2(ofset + (Spaceing + width) * i, BasePostion.position.y);
        }
    }

    public void SetCards(List<String> elements)
    {
        int count = Cards.Count;
        for (int i = 0; i < count; i++)
        {
            RemoveCard(Cards[0]);
        }
        foreach (string element in elements)
        {
            AddCard(element);
        }

        UpdateCards();
    }

    void RemoveCard(GameObject card)
    {
        Cards.Remove(card);
        Destroy(card);
    }

    void AddCard(string element)
    {
        GameObject gameObject = Instantiate(CardTemplate);
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        rend.sprite = SpriteDictionary[element];
        Cards.Add(gameObject);
    }
    

    private void BuildDictionary()
    {
        SpriteDictionary = new Dictionary<string, Sprite>();

        // Safety check
        if (Keys.Length != Sprites.Length)
        {
            Debug.LogError("Keys and Sprites arrays must have the same length!");
            return;
        }

        for (int i = 0; i < Keys.Length; i++)
        {
            if (string.IsNullOrEmpty(Keys[i]))
            {
                Debug.LogWarning($"Key at index {i} is null or empty, skipping.");
                continue;
            }

            if (SpriteDictionary.ContainsKey(Keys[i]))
            {
                Debug.LogWarning($"Duplicate key '{Keys[i]}' found, skipping.");
                continue;
            }

            SpriteDictionary.Add(Keys[i], Sprites[i]);
        }
    }


}

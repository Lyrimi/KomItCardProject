using UnityEngine;
using System.Collections.Generic;
using System;

public class Globals : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private string[] Keys;
    static public Dictionary<string, Sprite> SpriteDictionary;
    static public Dictionary<string, ElementLookup> StringToElement;
    [Header("Elements")]
    public List<Element> Elements;

    public string[] EffectDmgKeys;
    public int[] EffectDmgValues;
    static public Dictionary<string, int> EffectDamageLookUp;

    [System.Serializable]
    public struct ElementReaction
    {
        public string ReactionKey;
        public string EffectValue;
    }

    [System.Serializable]
    public class Element
    {
        public string Name;
        public string DefaultEffect;
        public List<ElementReaction> Reactions;
    }

    public class ElementLookup
    {
        public string Name;
        public string DefaultEffect;
        public Dictionary<string, string> ReactionDict;
        
    }

    public static ElementLookup[] ElementLookups;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BuildDictionarys();
        BuildElements();
        BuildStringToElement();
    }


    void BuildElements()
    {
        ElementLookups = new ElementLookup[Elements.Count];

        for (int i = 0; i < ElementLookups.Length; i++)
        {
            ElementLookups[i] = new ElementLookup();
            ElementLookups[i].DefaultEffect = Elements[i].DefaultEffect;
            ElementLookups[i].Name = Elements[i].Name;
            ElementLookups[i].ReactionDict = new Dictionary<string, string>();
            foreach (ElementReaction reaction in Elements[i].Reactions)
            {
                ElementLookups[i].ReactionDict[reaction.ReactionKey] = reaction.EffectValue;
            }
        }
    }

    void BuildStringToElement()
    {
         StringToElement = new Dictionary<string, ElementLookup>();

        foreach (ElementLookup elementLookup in ElementLookups)
        {
            StringToElement.Add(elementLookup.Name, elementLookup);
        }
    }
    
    private void BuildDictionarys()
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

        EffectDamageLookUp = new Dictionary<string, int>();

         // Safety check
        if (EffectDmgKeys.Length != EffectDmgValues.Length)
        {
            Debug.LogError("Keys and Sprites arrays must have the same length!");
            return;
        }

        for (int i = 0; i < EffectDmgKeys.Length; i++)
        {
            if (string.IsNullOrEmpty(Keys[i]))
            {
                Debug.LogWarning($"Key at index {i} is null or empty, skipping.");
                continue;
            }

            if (EffectDamageLookUp.ContainsKey(Keys[i]))
            {
                Debug.LogWarning($"Duplicate key '{Keys[i]}' found, skipping.");
                continue;
            }

            EffectDamageLookUp.Add(EffectDmgKeys[i], EffectDmgValues[i]);
        }

    }
}

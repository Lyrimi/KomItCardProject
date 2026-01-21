using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject[] Playerslots;
    
    public List<string> elementInHand;
    int selectedPlayerSlot;
    public CardOrginiser cardOrginiser;
    [Header("DONT TOUCH")]
    [SerializeField] string[] elementInSlot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elementInSlot = new string[Playerslots.Length];
        for (int i = 0; i < elementInSlot.Length; i++)
        {
            elementInSlot[i] = "Empty";
        }

        cardOrginiser.SetCards(elementInHand);
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick(GameObject obj)
    {
        if (obj.tag == "Slot")
        {
            for (int i = 0; i < Playerslots.Length; i++)
            {
                if (obj == Playerslots[i])
                {
                    if (elementInSlot[i] == "Empty")
                    {
                        selectedPlayerSlot = i;
                    }
                    else
                    {
                        ClickedUsedSlot(i);
                    }
                    
                }
            }
            UpdatePlayerSlot();
        }

        if (obj.tag == "Card")
        {
            int cardindex = 0;
            for (int i = 0; i < cardOrginiser.Cards.Count; i++)
            {
                if (obj.transform == cardOrginiser.Cards[i].transform)
                {
                    cardindex = i;
                    break;
                }
            }
            print($"Index{cardindex}");
            ClickedCardInHand(cardindex);
        }

        print($"You clicked {obj.name}");
    }

    void ClickedCardInHand(int index)
    {
        if (selectedPlayerSlot != -1 && elementInSlot[selectedPlayerSlot] == "Empty")
        {
            elementInSlot[selectedPlayerSlot] = elementInHand[index];
            elementInHand.RemoveAt(index);
            cardOrginiser.SetCards(elementInHand);
            int count = Playerslots.Length;
            bool empty = false;
            foreach (string element in elementInSlot)
            {
                if (element == "Empty")
                {
                    empty = true;
                }
            }
            if (empty)
            {
                while (elementInSlot[selectedPlayerSlot] != "Empty")
                {
                    if (selectedPlayerSlot + 1 < count)
                    {
                        selectedPlayerSlot += 1;
                    }
                    else
                    {
                        selectedPlayerSlot = 0;
                    }
                }
            }
            else
            {
                selectedPlayerSlot = -1;
            }
            UpdatePlayerSlot();
        }

    }
    
    void ClickedUsedSlot(int index)
    {
        elementInHand.Add(elementInSlot[index]);
        elementInSlot[index] = "Empty";
        selectedPlayerSlot = index;
        cardOrginiser.SetCards(elementInHand);
        UpdatePlayerSlot();
    }

    void UpdatePlayerSlot()
    {
        for (int i = 0; i < Playerslots.Length; i++)
        {
            GameObject slot = Playerslots[i];
            if (selectedPlayerSlot != -1 && (slot == Playerslots[selectedPlayerSlot]))
            {
                slot.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                slot.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
            CardSlot cardSlot = slot.GetComponent<CardSlot>();
            cardSlot.SetTexutre(elementInSlot[i]);
        }
           
    }
}

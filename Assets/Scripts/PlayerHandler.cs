using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject[] Playerslots;
    string[] elementInSlot;
    public List<string> elementInHand;
    int selectedPlayerSlot;
    public CardOrginiser cardOrginiser;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elementInSlot = new string[Playerslots.Length];
        for (int i = 0; i < elementInSlot.Length; i++)
        {
            elementInSlot[i] = "";
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
                if (obj == Playerslots[i] && elementInSlot[i] == "")
                {
                    selectedPlayerSlot = i;
                }
            }
            UpdatePlayerSlotSelected();
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
            ClickedCard(cardindex);
        }

        print($"You clicked {obj.name}");
    }
    
    void ClickedCard(int index)
    {
        elementInHand.RemoveAt(index);
        cardOrginiser.SetCards(elementInHand);
    }

    void UpdatePlayerSlotSelected()
    {
        foreach (GameObject slot in Playerslots)
        {
            if (slot == Playerslots[selectedPlayerSlot])
            {
                slot.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                slot.transform.localScale = new Vector3(0.75f,0.75f,0.75f);
            }
        }
    }
}

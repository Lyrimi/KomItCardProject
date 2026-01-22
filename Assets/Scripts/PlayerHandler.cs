using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject[] Playerslots;
    [SerializeField] public GameObject[] AttackDefendButtons;

    public List<string> elementInHand;
    int selectedPlayerSlot;
    public CardOrginiser cardOrginiser;
    [Header("DONT TOUCH")]
    [SerializeField] public string[] elementInSlot;
    [SerializeField] public int[] AttackDefendSlotMode;

    struct MagicNumbers
    {
        public const int attack = 0;
        public const int defend = 1;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elementInSlot = new string[Playerslots.Length];

        for (int i = 0; i < elementInSlot.Length; i++)
        {
            elementInSlot[i] = "Empty";
        }
        AttackDefendSlotMode = new int[Playerslots.Length];
        if (Playerslots.Length != AttackDefendButtons.Length)
        {
            Debug.LogError("There is not an equal amount of PlayerSlots and AttackDeffend buttons");
            return;
        }

        //Init Textures
        cardOrginiser.SetCards(elementInHand);
        UpdateAttackDeffend();
        UpdatePlayerSlot();
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
                    break;
                    
                }
            }
            UpdatePlayerSlot();
        }

        if (obj.tag == "Card")
        {
            for (int i = 0; i < cardOrginiser.Cards.Count; i++)
            {
                if (obj == cardOrginiser.Cards[i])
                {
                    ClickedCardInHand(i);
                    break;
                }
            }
        }

        if (obj.tag == "AtkDefSlot")
        {
            for (int i = 0; i < AttackDefendButtons.Length; i++)
            {
                if (obj == AttackDefendButtons[i])
                {
                    ClickedAttakDefendButton(i);
                    break;
                }
            }
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
    
    void ClickedAttakDefendButton(int index)
    {
        if (AttackDefendSlotMode[index] == MagicNumbers.attack)
        {
            AttackDefendSlotMode[index] = MagicNumbers.defend;
        }
        else if (AttackDefendSlotMode[index] == MagicNumbers.defend)
        {
            AttackDefendSlotMode[index] = MagicNumbers.attack;
        }
        else
        {
            Debug.LogWarning("Attack Defend not set properly");
        }
        UpdateAttackDeffend();
        
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
    
    void UpdateAttackDeffend()
    {
        for (int i = 0; i < AttackDefendButtons.Length; i++)
        {
            GameObject atkDef = AttackDefendButtons[i];
            AttackDefendButton attackDefendButton = atkDef.GetComponent<AttackDefendButton>();
            attackDefendButton.SetSprite(AttackDefendSlotMode[i]);
        }
    }
}

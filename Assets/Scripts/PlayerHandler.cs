using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject[] Playerslots;
    [SerializeField] public GameObject[] AttackDefendButtons;


    public string[] RngElements;
    [SerializeField] int MaxCards;

    int selectedPlayerSlot;
    public CardOrginiser CardOrginiser;
    [Header("DONT TOUCH")]
    public List<string> ElementInHand;
    [SerializeField] public string[] ElementInSlot;
    [SerializeField] public int[] AttackDefendSlotMode;

    public bool CanPlay = true;
    public bool IsFrozen = false;

    struct MagicNumbers
    {
        public const int attack = 0;
        public const int defend = 1;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ElementInSlot = new string[Playerslots.Length];

        for (int i = 0; i < ElementInSlot.Length; i++)
        {
            ElementInSlot[i] = "Empty";
        }
        AttackDefendSlotMode = new int[Playerslots.Length];
        if (Playerslots.Length != AttackDefendButtons.Length)
        {
            Debug.LogError("There is not an equal amount of PlayerSlots and AttackDeffend buttons");
            return;
        }
        RefreshPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick(GameObject obj)
    {
        if (CanPlay == false)
        {
            return;
        }
        if (obj.tag == "Slot")
        {
            for (int i = 0; i < Playerslots.Length; i++)
            {
                if (obj == Playerslots[i])
                {
                    if (ElementInSlot[i] == "Empty")
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
            for (int i = 0; i < CardOrginiser.Cards.Count; i++)
            {
                if (obj == CardOrginiser.Cards[i])
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

        //print($"You clicked {obj.name}");
    }

    void ClickedCardInHand(int index)
    {
        if (selectedPlayerSlot != -1 && ElementInSlot[selectedPlayerSlot] == "Empty")
        {
            ElementInSlot[selectedPlayerSlot] = ElementInHand[index];
            ElementInHand.RemoveAt(index);
            CardOrginiser.SetCards(ElementInHand);
            int count = Playerslots.Length;
            bool empty = false;
            foreach (string element in ElementInSlot)
            {
                if (element == "Empty")
                {
                    empty = true;
                }
            }
            if (empty)
            {
                while (ElementInSlot[selectedPlayerSlot] != "Empty")
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
        ElementInHand.Add(ElementInSlot[index]);
        ElementInSlot[index] = "Empty";
        selectedPlayerSlot = index;
        CardOrginiser.SetCards(ElementInHand);
        UpdatePlayerSlot();
    }

    void ClickedAttakDefendButton(int index)
    {
        if (AttackDefendSlotMode[index] == MagicNumbers.defend)
        {
            AttackDefendSlotMode[index] = MagicNumbers.attack;
        }

        else if (AttackDefendSlotMode[index] == MagicNumbers.attack)
        {
            for (int i = 0; i < AttackDefendSlotMode.Length; i++)
            {
                AttackDefendSlotMode[i] = MagicNumbers.attack;
            }
            AttackDefendSlotMode[index] = MagicNumbers.defend;
        }

        else
        {
            Debug.LogWarning("Attack Defend not set properly");
        }
        UpdateAttackDeffend();

    }

    public void UpdatePlayerSlot()
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
            if (ElementInSlot[i] == "Empty" && CanPlay)
            {
                cardSlot.SetTexutre("PlayerEmpty");
            }
            if (IsFrozen)
            {
                cardSlot.SetTexutre("EmptyFrozen");
            }
            else
            {
                cardSlot.SetTexutre(ElementInSlot[i]);
            }

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

    void GenerateCards()
    {
        for (int i = 0; i < MaxCards; i++)
        {
            ElementInHand.Add(RngElements[UnityEngine.Random.Range(0, RngElements.Length)]);
        }
    }

    public void RefreshPlayer()
    {
        GenerateCards();
        CardOrginiser.SetCards(ElementInHand);
        UpdateAttackDeffend();
        UpdatePlayerSlot();
    }
}

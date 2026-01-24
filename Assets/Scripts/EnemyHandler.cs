using UnityEngine;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour
{
    public GameObject[] Enemyslots;
    [SerializeField] public GameObject[] AttackDefendButtons;

    public string[] RngElements;

    [Header("Don't touch")]
    public string[] elementInSlot;
    [SerializeField] public int[] AttackDefendSlotMode;
    public bool IsFrozen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elementInSlot = new string[Enemyslots.Length];
        for (int i = 0; i < elementInSlot.Length; i++)
        {
            elementInSlot[i] = "Empty";
            Enemyslots[i].GetComponent<CardSlot>().SetAsEnemyCard();
        }
        AttackDefendSlotMode = new int[Enemyslots.Length];
        if (Enemyslots.Length != AttackDefendButtons.Length)
        {
            Debug.LogError("There is not an equal amount of PlayerSlots and AttackDeffend buttons");
            return;
        }
        NextRound();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateElements()
    {
        for (int i = 0; i < Enemyslots.Length; i++)
        {
            elementInSlot[i] = RngElements[Random.Range(0, RngElements.Length)];
        }
        UpdateEnemySlots();
    }

    public void GenerateAttakDefends()
    {
        for (int i = 0; i < Enemyslots.Length; i++)
        {
            AttackDefendSlotMode[i] = Random.Range(0, 2);
        }
        UpdateAttackDeffend();
    }

    public void UpdateEnemySlots()
    {
        for (int i = 0; i < Enemyslots.Length; i++)
        {
            CardSlot cardSlot = Enemyslots[i].GetComponent<CardSlot>();
            if (IsFrozen)
            {
                cardSlot.SetTexutre("EmptyFrozen");
            }
            else
            {
                cardSlot.SetTexutre(elementInSlot[i]);
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

    public void NextRound()
    {
        GenerateAttakDefends();
        GenerateElements();
    }
}

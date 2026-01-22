using UnityEngine;
using System.Collections.Generic;

public class EnemyHandler : MonoBehaviour
{
    public GameObject[] Enemyslots;

    public string[] RngElements;

    [Header("Don't touch")]
    public string[] elementInSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elementInSlot = new string[Enemyslots.Length];
        for (int i = 0; i < elementInSlot.Length; i++)
        {
            elementInSlot[i] = "Empty";
        }
        GenerateElements();
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

    void UpdateEnemySlots()
    {
        for (int i = 0; i < Enemyslots.Length; i++)
        {
            CardSlot cardSlot = Enemyslots[i].GetComponent<CardSlot>();
            cardSlot.SetTexutre(elementInSlot[i]);
        }
    }
}

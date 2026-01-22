using UnityEngine;
using System.Collections.Generic;
using Element = Globals.ElementLookup;
using Unity.VisualScripting;
using System;
using System.Threading.Tasks;
using TMPro;
using System.IO.IsolatedStorage;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerHandler Player;
    [SerializeField] EnemyHandler Enemy;

    [SerializeField] int MaxPlayerHealth;
    [SerializeField] int MaxEnemyHealth;

    [SerializeField] TextMeshProUGUI PlayerStatusText;
    [SerializeField] TextMeshProUGUI EnemyStatusText;

    [Header("Don't touch debug stuff")]


    [SerializeField] int PlayerHealth;
    [SerializeField] int EnemyHealth;
    [SerializeField] string[] Status;
    struct Ids
    {
        public const int Player = 0;
        public const int Enemy = 1;
    }
    Dictionary<string, Element> StringToElement;
    Dictionary<string, int> EffectDamageLookUp;

    Element[] Elements;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Status.Length < 2)
        {
            Status = new string[2];
        }
        Elements = Globals.ElementLookups;
        StringToElement = Globals.StringToElement;
        EffectDamageLookUp = Globals.EffectDamageLookUp;

        PlayerHealth = MaxPlayerHealth;
        EnemyHealth = MaxEnemyHealth;
        print(GetEffectForTarget(1, "Water"));
        ResolveDamage();
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {

    }

    string GetEffectForTarget(int TargetId, string Element)
    {
        string CurrentStatus = Status[TargetId];
        Element elm = StringToElement[Element];
        if (!string.IsNullOrEmpty(CurrentStatus) && elm.ReactionDict.ContainsKey(CurrentStatus))
        {
            return elm.ReactionDict[CurrentStatus];
        }
        return elm.DefaultEffect;
    }

    async Task AttackTarget(int TargetId, string Element)
    {
        Status[TargetId] = GetEffectForTarget(TargetId, Element);
        await Task.Delay(1000);
    }

    async Task ResolveDamage()
    {
        while (IsReflected() != -1)
        {
            int Reflect = IsReflected();
            if (Reflect == Ids.Enemy)
            {
                await AttackTarget(Ids.Player, "Rock");
                Status[Ids.Enemy] = "";
            }
            if (Reflect == Ids.Player)
            {
                await AttackTarget(Ids.Enemy, "Rock");
                Status[Ids.Player] = "";
            }
        }

        for (int i = 0; i < Status.Length; i++)
        {
            string status = Status[i];
            if (!(!string.IsNullOrEmpty(status) && EffectDamageLookUp.ContainsKey(status)))
            {
                continue;
            }
            if (i == Ids.Player)
            {
                PlayerHealth -= EffectDamageLookUp[status];
            }
            else
            {
                EnemyHealth -= EffectDamageLookUp[status];
            }
        }
    }

    int IsReflected()
    {
        for (int i = 0; i < Status.Length; i++)
        {
            if (Status[i] == "Reflect")
            {
                return i;
            }
        }
        return -1;
    }

    void ApplyDefense()
    {

    }
    
    void UpdateStatus()
    {
        PlayerStatusText.text = Status[Ids.Player];
        EnemyStatusText.text = Status[Ids.Enemy];
    }
}

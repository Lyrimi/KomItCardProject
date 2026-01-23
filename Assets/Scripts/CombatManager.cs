using UnityEngine;
using System.Collections.Generic;
using Element = Globals.ElementLookup;
using Unity.VisualScripting;
using System;
using System.Threading.Tasks;
using TMPro;
using System.IO.IsolatedStorage;
using System.Collections;
using UnityEditor.Rendering;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerHandler Player;
    [SerializeField] EnemyHandler Enemy;

    [SerializeField] int MaxPlayerHealth;
    [SerializeField] int MaxEnemyHealth;

    [SerializeField] TextMeshProUGUI PlayerStatusText;
    [SerializeField] TextMeshProUGUI EnemyStatusText;

    [SerializeField] TextMeshProUGUI PlayerHealthText;
    [SerializeField] TextMeshProUGUI EnemyHealthText;
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
        UpdateHealth();
        UpdateStatus();

    }

    // Update is called once per frame
    void Update()
    {

    }

    string GetEffectForTarget(int TargetId, string Element)
    {
        string CurrentStatus = Status[TargetId];
        if (Element == "Empty")
        {
            return CurrentStatus;
        }
        Element elm = StringToElement[Element];
        if (!string.IsNullOrEmpty(CurrentStatus) && elm.ReactionDict.ContainsKey(CurrentStatus))
        {
            return elm.ReactionDict[CurrentStatus];
        }
        return elm.DefaultEffect;
    }

    void AttackTarget(int TargetId, string Element)
    {
        Status[TargetId] = GetEffectForTarget(TargetId, Element);

    }

    void ResolveDamage()
    {
        while (IsReflected() != -1)
        {
            int Reflect = IsReflected();
            if (Reflect == Ids.Enemy)
            {
                AttackTarget(Ids.Player, "Earth");
                Status[Ids.Enemy] = "";
            }
            if (Reflect == Ids.Player)
            {
                AttackTarget(Ids.Enemy, "Earth");
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
        int[] PlayerModes = Player.AttackDefendSlotMode;
        int[] EnemyModes = Enemy.AttackDefendSlotMode;
        for (int i = 0; i < PlayerModes.Length; i++)
        {
            if (PlayerModes[i] == 1 && Player.ElementInSlot[i] != "Empty")
            {
                Status[Ids.Player] = Player.ElementInSlot[i] + "Def";
                Player.ElementInSlot[i] = "Empty";
                Player.UpdatePlayerSlot();
            }
        }
        for (int i = 0; i < EnemyModes.Length; i++)
        {
            if (EnemyModes[i] == 1 && Enemy.elementInSlot[i] != "Empty")
            {
                Status[Ids.Enemy] = Enemy.elementInSlot[i] + "Def";
                Enemy.elementInSlot[i] = "Empty";
                Enemy.UpdateEnemySlots();
            }
        }
    }

    void UpdateStatus()
    {
        PlayerStatusText.text = Status[Ids.Player];
        EnemyStatusText.text = Status[Ids.Enemy];
    }

    void UpdateHealth()
    {
        PlayerHealthText.text = $"Health {PlayerHealth} / {MaxPlayerHealth}";
        EnemyHealthText.text = $"Health {EnemyHealth} / {MaxEnemyHealth}";
    }

    public void OnClick(GameObject obj)
    {
        print("Turn Ended");
        StartCoroutine(DoCombat());
    }

    IEnumerator DoCombat()
    {
        Player.CanPlay = false;
        // suspend execution for 5 seconds
        bool DoesADefenseExist = false;
        foreach (int AtkDefSlot in Player.AttackDefendSlotMode)
        {
            if (AtkDefSlot == 1)
            {
                DoesADefenseExist = true;
            }
        }
        foreach (int AtkDefSlot in Enemy.AttackDefendSlotMode)
        {
            if (AtkDefSlot == 1)
            {
                DoesADefenseExist = true;
            }
        }

        if (DoesADefenseExist)
        {
            print("did Defense");
            ApplyDefense();
            UpdateStatus();
            yield return new WaitForSeconds(2);
        }

        string[] PlayerElements = Player.ElementInSlot;
        string[] EnemyElements = Enemy.elementInSlot;
        for (int i = 0; i < PlayerElements.Length; i++)
        {
            if (Player.AttackDefendSlotMode[i] == 0 && Player.ElementInSlot[i] != "Empty")
            {
                AttackTarget(Ids.Enemy, PlayerElements[i]);
                UpdateStatus();
                Player.ElementInSlot[i] = "Empty";
                Player.UpdatePlayerSlot();
                yield return new WaitForSeconds(1);
            }
        }
        for (int i = 0; i < EnemyElements.Length; i++)
        {
            if (Enemy.AttackDefendSlotMode[i] == 0 && Enemy.elementInSlot[i] != "Empty")
            {
                AttackTarget(Ids.Player, EnemyElements[i]);
                UpdateStatus();
                Enemy.elementInSlot[i] = "Empty";
                Enemy.UpdateEnemySlots();
                yield return new WaitForSeconds(1);
            }
        }
        ResolveDamage();
        UpdateHealth();
        yield return new WaitForSeconds(1);
        Player.IsFrozen = false;
        Enemy.IsFrozen = false;

        if (Status[Ids.Enemy] != "Frozen")
        {
            Enemy.NextRound();
        }
        else
        {
            Enemy.IsFrozen = true;
        }
        if (Status[Ids.Player] != "Frozen")
        {
            Player.CanPlay = true;
        }
        else
        {
            Player.IsFrozen = true;
        }

        Player.UpdatePlayerSlot();
        Enemy.UpdateEnemySlots();


        Status = new string[2];
        UpdateStatus();

        if (Player.ElementInHand.Count == 0)
        {
            Player.RefreshPlayer();
        }
    }
}

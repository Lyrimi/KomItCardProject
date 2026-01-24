using UnityEngine;
using System.Collections.Generic;
using System;

public class CardSlot : MonoBehaviour
{
    CardOrginiser cardOrginiser;
    Dictionary<string, Sprite> SpriteDictionary;
    SpriteRenderer rend;

    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteDictionary = Globals.SpriteDictionary;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTexutre(String element)
    {
        rend.sprite = SpriteDictionary[element];
        
    }

    public void SetAnimationTexture(string element)
    {
        GameObject childObj = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        SpriteRenderer animrender = childObj.GetComponent<SpriteRenderer>();
        animrender.sprite = SpriteDictionary[element];
        print("tried to set texutre");
    }

    public void SetAsEnemyCard()
    {
        anim.SetBool("IsEnemy", true);
    }

    public AnimatorStateInfo RunAnimation(string Animation)
    {
        anim.SetTrigger(Animation);
        print($"tried to run animation {Animation}");
        return anim.GetCurrentAnimatorStateInfo(0);
    }
}

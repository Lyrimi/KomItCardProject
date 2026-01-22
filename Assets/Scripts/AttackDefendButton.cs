using UnityEngine;

public class AttackDefendButton : MonoBehaviour
{
    [SerializeField] Sprite Attack;
    [SerializeField] Sprite Defend;
    SpriteRenderer rend;

    struct MagicNumbers
    {
        public const int attack = 0;
        public const int defend = 1;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetSprite(int mode)
    {
        if (MagicNumbers.attack == mode)
        {
            rend.sprite = Attack;
        }
        else if (MagicNumbers.defend == mode)
        {
            rend.sprite = Defend;
        }
        else
        {
            Debug.LogWarning("Attack Defend not set properly");
        }
    }
}

using UnityEngine;
using Element = Globals.ElementLookup;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerHandler Player;
    [SerializeField] EnemyHandler Enemy;

    Element[] Elements;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Elements = Globals.ElementLookups;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class EndTurn : MonoBehaviour
{
    InputAction pointAction;
    CombatManager combatManager;

    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        combatManager = FindFirstObjectByType<CombatManager>();
    }
    void Update()
    {
        if (Pointer.current.press.wasPressedThisFrame)
        {
            CheckClick();
        }
    }

    public void CheckClick()
    {
        Vector3 screenPos = Pointer.current.position.ReadValue();
        screenPos.z = -Camera.main.transform.position.z;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        Debug.DrawRay(worldPos, Vector2.zero, Color.blue);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            combatManager.OnClick(gameObject);
        }
    }
}

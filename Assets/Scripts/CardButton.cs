using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CardButton : MonoBehaviour
{
    InputAction pointAction;
    PlayerHandler player;

    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        player = FindFirstObjectByType<PlayerHandler>();
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
            player.OnClick(gameObject); 
        }
    }
}

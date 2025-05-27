using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class InventoryRoot : MonoBehaviour
{
    void Start()
    {
        Managers.Input.InputSystemActions.Player.Inventory.started += ShowInventory;
        Managers.Input.InputSystemActions.Inventory.CloseInventory.started += CloseInventory;
    }

    void ShowInventory(InputAction.CallbackContext ctx)
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = true;

        Managers.Input.InputSystemActions.Player.Disable();
        Managers.Input.InputSystemActions.Inventory.Enable();
    }

    private void CloseInventory(InputAction.CallbackContext context)
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        Managers.Input.InputSystemActions.Player.Enable();
        Managers.Input.InputSystemActions.Inventory.Disable();
    }
}

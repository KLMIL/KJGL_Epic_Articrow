using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryRoot : MonoBehaviour
{
    Canvas _canvas;
    void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        Managers.Input.InputSystemActions.Player.Inventory.started += ShowInventory;
        Managers.Input.InputSystemActions.Inventory.CloseInventory.started += CloseInventory;
    }

    void ShowInventory(InputAction.CallbackContext ctx)
    {
        _canvas.enabled = true;
        Managers.Input.InputSystemActions.Player.Disable();
        Managers.Input.InputSystemActions.Inventory.Enable();
    }

    void CloseInventory(InputAction.CallbackContext context)
    {
        _canvas.enabled = false;
        Managers.Input.InputSystemActions.Player.Enable();
        Managers.Input.InputSystemActions.Inventory.Disable();
    }
}
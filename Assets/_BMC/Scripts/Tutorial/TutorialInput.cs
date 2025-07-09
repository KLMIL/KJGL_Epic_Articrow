using UnityEngine;
using UnityEngine.InputSystem;
using YSJ;

public class TutorialInput : MonoBehaviour
{
    InputManager _input;
    InputSystemActions _inputSystemActions;

    void Start()
    {
        _input = Managers.Input;
        _inputSystemActions = _input.InputSystemActions;
        SubscribeInputSystem();
    }

    // 튜토리얼 액션 맵 구독
    public void SubscribeInputSystem()
    {
        _inputSystemActions.Tutorial.Move.performed += _input.OnMove;
        _inputSystemActions.Tutorial.Move.canceled += _input.OnMove;
        _inputSystemActions.Tutorial.Dash.performed += _input.OnDash;
        _inputSystemActions.Tutorial.Dash.canceled += _input.OnDash;

        _inputSystemActions.Tutorial.Interact.performed += _input.OnInteract;
        _inputSystemActions.Tutorial.Interact.canceled += _input.OnInteract;

        _inputSystemActions.Tutorial.Inventory.performed += _input.OnInventory;
        _inputSystemActions.Tutorial.Inventory.canceled += _input.OnInventory;

        _inputSystemActions.Tutorial.MousePos.performed += _input.OnMousePos;
        _inputSystemActions.Tutorial.LeftHand.performed += _input.OnLeftHand;
        _inputSystemActions.Tutorial.LeftHand.canceled += _input.OnLeftHand;
        _inputSystemActions.Tutorial.RightHand.performed += _input.OnRightHand;
        _inputSystemActions.Tutorial.RightHand.canceled += _input.OnRightHand;

        _inputSystemActions.Tutorial.Pause.performed += _input.OnPause;
        _inputSystemActions.UI.Cancel.performed += _input.OnPause;
    }

    // 튜토리얼 액션 맵 구독 해제
    public void DescribeInputSystem()
    {
        _inputSystemActions.Tutorial.Move.performed -= _input.OnMove;
        _inputSystemActions.Tutorial.Move.canceled -= _input.OnMove;
        _inputSystemActions.Tutorial.Dash.performed -= _input.OnDash;
        _inputSystemActions.Tutorial.Dash.canceled -= _input.OnDash;

        _inputSystemActions.Tutorial.Interact.performed -= _input.OnInteract;
        _inputSystemActions.Tutorial.Interact.canceled -= _input.OnInteract;

        _inputSystemActions.Tutorial.Inventory.performed -= _input.OnInventory;
        _inputSystemActions.Tutorial.Inventory.canceled -= _input.OnInventory;

        _inputSystemActions.Tutorial.MousePos.performed -= _input.OnMousePos;
        _inputSystemActions.Tutorial.LeftHand.performed -= _input.OnLeftHand;
        _inputSystemActions.Tutorial.LeftHand.canceled -= _input.OnLeftHand;
        _inputSystemActions.Tutorial.RightHand.performed -= _input.OnRightHand;
        _inputSystemActions.Tutorial.RightHand.canceled -= _input.OnRightHand;

        _inputSystemActions.Tutorial.Pause.performed -= _input.OnPause;
        _inputSystemActions.UI.Cancel.performed -= _input.OnPause;
    }

    // 허용된 액션만 활성화
    public void EnableOnlyAction(params InputAction[] allowedActions)
    {
        var map = _inputSystemActions.Tutorial;

        // 맵 전체 비활성화
        map.Disable();

        // 허용된 액션만 활성화
        foreach(var action in allowedActions)
        {
            action.Enable();
        }
    }

    // 튜토리얼 액션 맵 전체 활성화
    public void EnableActionMap()
    {
        var map = _inputSystemActions.Tutorial;

        // 맵 전체 활성화
        map.Enable();
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public InputSystemActions InputSystemActions => _inputSystemActions;
    InputSystemActions _inputSystemActions;

    #region 입력 변수
    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseWorldPos { get; private set; }  // 마우스 입력(월드 좌표)
    public bool IsPressLeftHandAttack { get; private set; } // 왼손 공격 입력 여부
    public bool IsPressRightHandAttack { get; private set; } // 오른손 공격 입력 여부
    public bool IsPressDash { get; private set; }       // 대시 입력 여부
    #endregion

    #region 액션
    public Action attackAction;                 // 공격
    public Action parryAction;                  // 패링
    public Action<Vector2> dashAction;          // 대시
    public Action interactAction;               // 상호작용(줍기) 
    #endregion

    public void Init()
    {
        _inputSystemActions = new InputSystemActions();
        SetInGame();
    }

    public void SetInGame()
    {
        _inputSystemActions.Player.Enable();
        _inputSystemActions.Inventory.Disable();

        _inputSystemActions.Player.Move.performed += OnMove;
        _inputSystemActions.Player.Move.canceled += OnMove;

        _inputSystemActions.Player.Interact.performed += OnInteract;
        _inputSystemActions.Player.Interact.canceled += OnInteract;

        _inputSystemActions.Player.LeftHand.performed += OnLeftHand;
        _inputSystemActions.Player.LeftHand.canceled += OnLeftHand;
        _inputSystemActions.Player.RightHand.performed += OnRightHand;
        _inputSystemActions.Player.RightHand.canceled += OnRightHand;

        //_inputSystemActions.Player.MousePos.performed += OnMousePos;

        //_inputSystemActions.Player.Attack.performed += OnAttack;
        //_inputSystemActions.Player.Attack.canceled += OnAttack;
        //_inputSystemActions.Player.Dash.performed += OnDash;
        //_inputSystemActions.Player.Dash.canceled += OnDash;
        //_inputSystemActions.Player.Parry.performed += OnParry;
        //_inputSystemActions.Player.Parry.canceled += OnParry;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>().normalized;
        //Debug.Log(MoveInput);
    }

    void OnMousePos(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();
        MouseWorldPos = Camera.main.ScreenToWorldPoint(mouseInput);
        //Debug.Log(MouseWorldPos);
    }

    void OnDash(InputAction.CallbackContext context)
    {
        IsPressDash = context.ReadValueAsButton();
        if (context.performed)
        {
            dashAction?.Invoke(MoveInput);
        }
        //Debug.Log("IsPressDash: " + IsPressDash);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("Interact");
        if (context.performed)
        {
            interactAction?.Invoke();
            Debug.Log("먹는다.");
            // Interact action can be defined here if needed
        }
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("Attack");
        if (context.performed)
            attackAction?.Invoke();
        IsPressLeftHandAttack = context.ReadValueAsButton();
    }

    void OnLeftHand(InputAction.CallbackContext context)
    {
        //Debug.Log("Left Hand");
        if (context.performed)
        {
            // Left hand action can be defined here if needed
            Debug.Log("좌수");
        }
        IsPressLeftHandAttack = context.ReadValueAsButton();
    }

    void OnRightHand(InputAction.CallbackContext context)
    {
        //Debug.Log("Right Hand");
        if (context.performed)
        {
            Debug.Log("우수");
            // Right hand action can be defined here if needed
        }
        IsPressRightHandAttack = context.ReadValueAsButton();
    }


    void OnParry(InputAction.CallbackContext context)
    {
        IsPressRightHandAttack = context.ReadValueAsButton();
        if (context.performed)
        {
            parryAction?.Invoke();
        }
        //Debug.Log("IsPressParry: " + IsPressParry);
    }

    public void CancelAction()
    {
        attackAction = null;
        parryAction = null;
        interactAction = null;
    }

    public void Clear()
    {
        if (_inputSystemActions != null)
        {
            CancelAction();
            _inputSystemActions.Player.Disable();
            _inputSystemActions = null;
        }
    }
}
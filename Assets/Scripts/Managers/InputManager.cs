using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public InputSystemActions InputSystemActions => _inputSystemActions;
    InputSystemActions _inputSystemActions;

    #region 입력 변수
    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseWorldPos { get; private set; }       // 마우스 입력(월드 좌표)
    public bool IsPressLeftHandAttack { get; private set; }  // 왼손 공격 입력 여부
    public bool IsPressRightHandAttack { get; private set; } // 오른손 공격 입력 여부
    public bool IsPressRoll { get; private set; }            // 구르기 입력 여부
    #endregion

    #region 액션
    public Action<Vector2> OnRollAction;          // 구르기
    public Action OnInteractAction;               // 상호작용(줍기) 
    public Action OnLeftHandAction;               // 좌수
    public Action OnRightHandAction;              // 우수
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
        _inputSystemActions.Player.Roll.performed += OnRoll;
        _inputSystemActions.Player.Roll.canceled += OnRoll;

        _inputSystemActions.Player.Interact.performed += OnInteract;
        _inputSystemActions.Player.Interact.canceled += OnInteract;

        //_inputSystemActions.Player.MousePos.performed += OnMousePos;
        _inputSystemActions.Player.LeftHand.performed += OnLeftHand;
        _inputSystemActions.Player.LeftHand.canceled += OnLeftHand;
        _inputSystemActions.Player.RightHand.performed += OnRightHand;
        _inputSystemActions.Player.RightHand.canceled += OnRightHand;
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

    void OnRoll(InputAction.CallbackContext context)
    {
        IsPressRoll = context.ReadValueAsButton();
        if (context.performed)
        {
            OnRollAction?.Invoke(MoveInput);
        }
        //Debug.Log("IsPressDash: " + IsPressDash);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteractAction?.Invoke();
            Debug.Log("상호작용");
        }
    }

    void OnLeftHand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("좌수");
        }
        IsPressLeftHandAttack = context.ReadValueAsButton();
    }

    void OnRightHand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("우수");
        }
        IsPressRightHandAttack = context.ReadValueAsButton();
    }

    public void ClearAction()
    {
        OnLeftHandAction = null;
        OnRightHandAction = null;
        OnRollAction = null;
        OnInteractAction = null;
    }

    public void Clear()
    {
        if (_inputSystemActions != null)
        {
            ClearAction();
            _inputSystemActions.Player.Disable();
            _inputSystemActions = null;
        }
    }
}
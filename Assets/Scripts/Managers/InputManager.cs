using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;
using YSJ;

public class InputManager
{
    public InputSystemActions InputSystemActions => _inputSystemActions;
    InputSystemActions _inputSystemActions;

    #region 입력 변수
    public Vector2 MoveInput { get; private set; }
    public Vector2 MouseWorldPos { get; private set; }       // 마우스 입력(월드 좌표)
    public bool IsPressInteract { get; private set; }        // 상호작용 입력 여부
    public bool IsPressLeftHandAttack { get; private set; }  // 왼손 공격 입력 여부
    public bool IsPressRightHandAttack { get; private set; } // 오른손 공격 입력 여부
    public bool IsPressDash { get; private set; }            // 대시 입력 여부
    public bool IsPressInventory { get; private set; }       // 인벤토리 입력 여부
    #endregion

    #region 액션
    public Action<Vector2> OnDashAction;          // 구르기
    public Action OnInteractAction;               // 상호작용(줍기)
    public Action OnInventoryAction;              // 인벤토리
    public Action OnLeftHandAction;               // 좌수
    public Action OnRightHandAction;              // 우수
    public Action OnPauseAction;                  // 일시정지
    #endregion

    //[Header("Rebind")]
    //InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    //Dictionary<KeyAction, InputAction> _keyActionDict;

    public void Init()
    {
        _inputSystemActions = new InputSystemActions();
        _inputSystemActions.Enable();

        //_keyActionDict = new Dictionary<KeyAction, InputAction>
        //{
        //    { KeyAction.Move, _inputSystemActions.Player.Move},
        //    { KeyAction.Interact, _inputSystemActions.Player.Interact },
        //    { KeyAction.Inventroy, _inputSystemActions.Player.Inventory},
        //    { KeyAction.LeftHand, _inputSystemActions.Player.LeftHand },
        //    { KeyAction.RightHand, _inputSystemActions.Player.RightHand },
        //    { KeyAction.Dash, _inputSystemActions.Player.Dash },
        //};

        LoadKeyBind();
        SubscribeAction();

        SetGameMode();
    }

    public void SubscribeAction()
    {
        _inputSystemActions.Player.Move.performed += OnMove;
        _inputSystemActions.Player.Move.canceled += OnMove;
        _inputSystemActions.Player.Dash.performed += OnDash;
        _inputSystemActions.Player.Dash.canceled += OnDash;

        _inputSystemActions.Player.Interact.performed += OnInteract;
        _inputSystemActions.Player.Interact.canceled += OnInteract;

        _inputSystemActions.Player.Inventory.performed += OnInventory;
        _inputSystemActions.Player.Inventory.canceled += OnInventory;

        _inputSystemActions.Player.MousePos.performed += OnMousePos;
        _inputSystemActions.Player.LeftHand.performed += OnLeftHand;
        _inputSystemActions.Player.LeftHand.canceled += OnLeftHand;
        _inputSystemActions.Player.RightHand.performed += OnRightHand;
        _inputSystemActions.Player.RightHand.canceled += OnRightHand;

        _inputSystemActions.Player.Pause.performed += OnPause;
        _inputSystemActions.UI.Cancel.performed += OnPause;
    }

    // 게임 모드로 전환
    public void SetGameMode()
    {
        _inputSystemActions.Player.Enable();
        _inputSystemActions.UI.Disable();
    }

    // UI 모드로 전한
    public void SetUIMode()
    {
        _inputSystemActions.Player.Disable();
        _inputSystemActions.UI.Enable();
    }

    #region 키 리바인드 관련

    // 키바인드 불러오기
    public void LoadKeyBind()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            Debug.Log(rebinds + "\n 불러옴");
            _inputSystemActions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void ApplyKeyBind(string savedBindings)
    {
        _inputSystemActions.LoadBindingOverridesFromJson(savedBindings);
        Debug.Log("적용하기");
    }
    #endregion

    #region GamePlay
    void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>().normalized;
        //Debug.Log("MoveInput: " + MoveInput);
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
            OnDashAction?.Invoke(MoveInput);
            Debug.Log("대시");
        }
        //Debug.Log("IsPressDash: " + IsPressDash);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        IsPressInteract = context.ReadValueAsButton();
        if (context.performed)
        {
            OnInteractAction?.Invoke();
            //Debug.Log("상호작용");
        }
    }

    void OnInventory(InputAction.CallbackContext context)
    {
        IsPressInventory = context.ReadValueAsButton();
        if (context.performed)
        {
            OnInventoryAction?.Invoke();
            //Debug.Log("인벤토리");
        }
    }

    void OnLeftHand(InputAction.CallbackContext context)
    {
        IsPressLeftHandAttack = context.ReadValueAsButton();
        if (context.performed)
        {
            OnLeftHandAction?.Invoke();
            //Debug.Log("좌수");
        }
    }

    void OnRightHand(InputAction.CallbackContext context)
    {
        IsPressRightHandAttack = context.ReadValueAsButton();
        if (context.performed)
        {
            OnRightHandAction?.Invoke();
            //Debug.Log("우수");
        }
        if (context.canceled)
        {
            //TODO : SkillManager 정리할 때 같이 정리하기
            ActionT0Handler onHandCancelActionT0 = GameManager.Instance.RightSkillManager.OnHandCancelActionT0;
            if (onHandCancelActionT0 != null) onHandCancelActionT0.Trigger();
        }
    }
    #endregion

    void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Managers.Scene.CurrentScene.SceneType == SceneType.InGameScene)
            {
                if (_inputSystemActions.Player.enabled)
                {
                    _inputSystemActions.Player.Disable();
                    _inputSystemActions.UI.Enable();
                    Debug.Log("플레이어 -> UI 모드로 전환");
                }
                else if (_inputSystemActions.UI.enabled)
                {
                    _inputSystemActions.UI.Disable();
                    _inputSystemActions.Player.Enable();
                    Debug.Log("UI -> 플레이어 모드로 전환");
                }
                OnPauseAction?.Invoke();
            }
        }
    }

    public void ClearAction()
    {
        //OnDashAction = null;
        OnInteractAction = null;
        OnInventoryAction = null;
        //OnLeftHandAction = null;
        OnRightHandAction = null;
        OnPauseAction = null;
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
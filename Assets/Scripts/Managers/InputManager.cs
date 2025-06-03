using BMC;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
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


    [Header("Rebind")]
    InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    Dictionary<KeyAction, InputAction> _keyActionDict;

    #region 액션
    public Action<Vector2> OnRollAction;          // 구르기
    public Action OnInteractAction;               // 상호작용(줍기) 
    public Action OnLeftHandAction;               // 좌수
    public Action OnRightHandAction;              // 우수
    #endregion

    public void Init()
    {
        _inputSystemActions = new InputSystemActions();
        _inputSystemActions.Enable();

        _keyActionDict = new Dictionary<KeyAction, InputAction>
        {
            { KeyAction.Roll, _inputSystemActions.Player.Roll },
        };
        LoadBind();
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

    #region 리바인딩 관련

    // 리바인딩
    public void Rebind(KeyAction keyAction, TextMeshProUGUI text)
    {
        // 1. 리바인드 전 비활성화 (기존 입력 액션 작동하지 않도록 막음)
        _inputSystemActions.Player.Disable();

        // 2. 리바인딩 작업을 구성하는 객체 구성
        _rebindingOperation = _keyActionDict[keyAction].PerformInteractiveRebinding().OnComplete(operation => RebindCompleted(keyAction, text));

        // 3. 입력 대기 상태로 진입 (입력 들어오면 OnComplete 콜백 실행됨)
        _rebindingOperation.Start();
    }

    // 리바인딩이 완료되었을 때 실행할 메서드
    void RebindCompleted(KeyAction keyAction, TextMeshProUGUI text)
    {
        // 1. 현재 진행중이던 리바인딩 작업 종료하고 리소스 해제
        _rebindingOperation.Dispose();
        string newBinding = _keyActionDict[keyAction].bindings[0].effectivePath;
        Debug.Log("New Binding: " + newBinding);

        // 2. 리바인딩 후 Player 다시 활성화
        _inputSystemActions.Player.Enable();

        // 3. 리바인딩된 키를 저장
        var rebinds = _inputSystemActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        Debug.Log(rebinds + "\n 저장함");

        // 4. 저장한 리바인드 불러와서 입력 시스템에 적용
        LoadBind();
        text.text = newBinding;
    }

    // 바인드 불러오기
    public void LoadBind()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            Debug.Log(rebinds + "\n 불러옴");
            _inputSystemActions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    #endregion

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
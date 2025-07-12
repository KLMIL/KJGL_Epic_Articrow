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
    public Action OnLeftHandActionEnd;            // YSJ 왼클릭 땠을 때

    public Action OnRightHandAction;              // 우수
    public Action OnRightHandActionEnd;           // YSJ 우클릭 댔을 때

    public Action OnPauseAction;                  // 일시정지
    #endregion

    public void Init()
    {
        _inputSystemActions = new InputSystemActions();
        _inputSystemActions.Enable();

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
        _inputSystemActions.Tutorial.Disable();
    }

    public void SetTutorialMode()
    {
        _inputSystemActions.Player.Disable();
        _inputSystemActions.UI.Disable();
        _inputSystemActions.Tutorial.Enable();
    }

    // UI 모드로 전한
    public void SetUIMode()
    {
        _inputSystemActions.Player.Disable();
        _inputSystemActions.UI.Enable();
        _inputSystemActions.Tutorial.Disable();
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
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>().normalized;
        //Debug.Log("MoveInput: " + MoveInput);
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();
        MouseWorldPos = Camera.main.ScreenToWorldPoint(mouseInput);
        //Debug.Log(MouseWorldPos);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        IsPressDash = context.ReadValueAsButton();
        if (context.performed)
        {
            OnDashAction?.Invoke(MoveInput);
            Debug.Log("대시");
            IsPressDash = false;
        }
        //Debug.Log("IsPressDash: " + IsPressDash);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        IsPressInteract = context.ReadValueAsButton();
        if (context.performed)
        {
            OnInteractAction?.Invoke();
            //Debug.Log("상호작용");
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        IsPressInventory = context.ReadValueAsButton();
        if (context.performed)
        {
            OnInventoryAction?.Invoke();
            //Debug.Log("인벤토리");
        }
    }

    public void OnLeftHand(InputAction.CallbackContext context)
    {
        IsPressLeftHandAttack = context.ReadValueAsButton();
        if (context.performed)
        {
            OnLeftHandAction?.Invoke();
            //Debug.Log("좌수");
        }
        // YSJ cancled로 실행된거면 cancleEvent호출
        if (context.canceled) 
        {
            OnLeftHandActionEnd?.Invoke();
        }
    }

    public void OnRightHand(InputAction.CallbackContext context)
    {
        IsPressRightHandAttack = context.ReadValueAsButton();
        if (context.performed)
        {
            OnRightHandAction?.Invoke();
            //Debug.Log("우수");
        }
        if (context.canceled)
        {
            // YSJ
            OnRightHandActionEnd?.Invoke();
        }
    }
    #endregion

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (Managers.Scene.CurrentScene.SceneType != SceneType.TitleScene
                && Managers.Scene.CurrentScene.SceneType != SceneType.TutorialScene)
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
        else if (Managers.Scene.CurrentScene.SceneType == SceneType.TutorialScene)
        {
            if (_inputSystemActions.Tutorial.enabled)
            {
                _inputSystemActions.Tutorial.Disable();
                _inputSystemActions.UI.Enable();
                Debug.Log("튜토리얼 -> UI 모드로 전환");
            }
            else if (_inputSystemActions.UI.enabled)
            {
                _inputSystemActions.UI.Disable();
                _inputSystemActions.Tutorial.Enable();
                Debug.Log("UI -> 튜토리얼 모드로 전환");
            }
            OnPauseAction?.Invoke();
        }
    }

    public void ClearAction()
    {
        //OnDashAction = null;
        //OnInteractAction = null;
        //OnInventoryAction = null;
        //OnLeftHandAction = null;
        //OnRightHandAction = null;
        OnPauseAction = null;       // 일시 정지 메뉴는 인게임 씬마다 존재하므로 null로 초기화 필요
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

    #region 입력 On/Off

    public void EnablePlayer()
    {
        _inputSystemActions.Player.Enable();
    }

    public void DisablePlayer()
    {
        _inputSystemActions.Player.Disable();
    }

    public void EnableAttack(bool isActive)
    {
        if(isActive)
        {
            _inputSystemActions.Player.LeftHand.Enable();
            _inputSystemActions.Player.RightHand.Enable();
        }
        else
        {
            _inputSystemActions.Player.LeftHand.Disable();
            _inputSystemActions.Player.RightHand.Disable();
        }
    }
    #endregion
}
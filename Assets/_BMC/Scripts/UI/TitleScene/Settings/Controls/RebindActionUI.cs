using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using YSJ;

namespace BMC
{
    // ControlCanvas 자손들 중 'Rebind'가 붙은 오브젝트들이 갖고 있어야 하는 클래스
    public class RebindActionUI : MonoBehaviour
    {
        [Tooltip("리바인딩할 원본 InputActionReference, 어떤 액션을 리바인딩할 지 결정")]
        [SerializeField] InputActionReference _inputSystemAction;

        [Tooltip("유니티 Input System의 InputBinding.id는 GUID 형태로 생성되는데, 이 값을 문자열로 받아서 내부적으로 Guid.Parse(m_BindingId) 형태로 사용")]
        [SerializeField] string _bindingId;

        [Tooltip("리바인딩 대상이 되는 Binding ID, Guid.Parse(m_BindingId) 형태로 사용")]
        [SerializeField] InputBinding.DisplayStringOptions m_DisplayStringOptions;  // 바인딩 문자열 표시 옵션, ex) W, Spacebar, Gamepad Button

        [SerializeField] TextMeshProUGUI _actionLabel; // 무슨 액션인지 표시하는 라벨 ex) Move 등

        [Tooltip("바인딩 문자열 표시할 텍스트(버튼의 텍스트), ex) W, Spacebar, Gamepad Button")]
        [SerializeField] TextMeshProUGUI _bindingText;

        [Tooltip("기본 동작은 m_BindingText.text = displayString이지만, " +
            "더 복잡한 UI(예: 키 아이콘을 바꾸거나 스프라이트를 보여주고 싶을 때)라면 여기에 리스너를 연결하여 원하는 방식으로 UI를 갱신할 수 있다.")]
        [SerializeField] UpdateBindingUIEvent m_UpdateBindingUIEvent;

        [Tooltip("재바인딩이 시작될 때(interactive rebind operation이 Start() 호출되고, 실제 입력 대기를 시작하기 직전) 호출되는 커스텀 이벤트"
            + "Overlay를 꺼내기 전”의 준비 로직(예: 사운드 재생, 애니메이션 시작 등)을 연결할 때 사용 가능하다")]
        [SerializeField] InteractiveRebindEvent m_RebindStartEvent;

        [Tooltip("재바인딩이 완료되거나 취소되었을 때 호출되는 커스텀 이벤트" +
            "실제 새 입력이 확정된 직후 혹은 중간에 “취소”되었을 때, “UI를 원래 상태로 되돌리기” 등 후처리 로직을 연결하는 용도")]
        [SerializeField] InteractiveRebindEvent m_RebindStopEvent;

        [Tooltip("PerformInteractiveRebinding(...)를 통해 생성되며, 내부에서 “대기→입력 감지→바인딩 오버라이드→종료” 시퀀스를 관리" +
            "이 객체를 취소(Cancel()), 중지된 후 해제(Dispose()), 또는 .Start()를 통해 제어")]
        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;

        [Tooltip("Input System 쪽에서 “키보드 레이아웃 변경” 혹은 “바인딩이 외부에서 바뀌었음”을 알려주는 InputSystem.onActionChange 콜백을 받을 때, " +
            "모든 RebindActionUI를 순회하며 UI 갱신을 수행하기 위해 사용")]
        static List<RebindActionUI> s_RebindActionUIs;

        [SerializeField] Button _btn;

        #region 프로퍼티
        public InputActionReference actionReference
        {
            get => _inputSystemAction;
            set
            {
                _inputSystemAction = value;
                // UpdateActionLabel();
                UpdateBindingDisplay();
            }
        }

        /// <summary>
        /// ID (in string form) of the binding that is to be rebound on the action.
        /// </summary>
        /// <seealso cref="InputBinding.id"/>
        public string bindingId
        {
            get => _bindingId;
            set
            {
                _bindingId = value;
                UpdateBindingDisplay();
            }
        }

        public InputBinding.DisplayStringOptions displayStringOptions
        {
            get => m_DisplayStringOptions;
            set
            {
                m_DisplayStringOptions = value;
                UpdateBindingDisplay();
            }
        }

        public TextMeshProUGUI bindingText
        {
            get => _bindingText;
            set
            {
                _bindingText = value;
                UpdateBindingDisplay();
            }
        }

        #endregion

        void Awake()
        {
            _actionLabel = GetComponentInChildren<TextMeshProUGUI>();
            _btn = GetComponentInChildren<Button>();
            _bindingText = _btn.GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            LoadActionBinding();
            _btn.onClick.AddListener(StartInteractiveRebind);
            UI_TitleEventBus.OnResetKeyBind += ResetToDefault; // 키 리셋 이벤트 등록
        }

        #region 메서드
        // 바인딩을 실제로 변경하거나(=override), 디폴트로 초기화(=RemoveBindingOverride)하기 전에,
        // 이 액션에 지정된 이 바인딩(ID)이 유효한가를 검증하는 절차
        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
        {
            bindingIndex = -1;

            action = _inputSystemAction?.action;
            if (action == null)
                return false;

            if (string.IsNullOrEmpty(_bindingId))
                return false;

            // Look up binding index.
            var bindingId = new Guid(_bindingId);
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
            if (bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
                return false;
            }

            return true;
        }

        // 항상 화면에 보이는 키 이름이 최신 상태로 갱신
        public void UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            var deviceLayoutName = default(string);
            var controlPath = default(string);

            // Get display string from action.
            var action = _inputSystemAction?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == _bindingId);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);
            }

            // Set on label (if any).
            if (_bindingText != null)
                _bindingText.text = displayString;

            // Give listeners a chance to configure UI in response.
            m_UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
        }

        // 현재 액션의 해당 바인딩(bindingIndex)에 적용된 “바인딩 오버라이드(override)” 를 삭제하여,
        // 원래(InputActionAsset에 정의된 Default 바인딩) 으로 복원(rest)하는 기능
        public void ResetToDefault()
        {
            _inputSystemAction.action.Disable();

            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            if (action.bindings[bindingIndex].isComposite)
            {
                // It's a composite. Remove overrides from part bindings.
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                    action.RemoveBindingOverride(i);
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }
            UpdateBindingDisplay();
            SaveActionBinding();    // 현재 바인딩 상태를 저장
        }

        // 실제로 키를 눌러서 재바인딩을 시작하도록 트리거(trigger)하는 진입점
        public void StartInteractiveRebind()
        {
            // 액션이나 바인딩 인덱스가 유효하지 않으면 즉시 리턴
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            if (action.bindings[bindingIndex].isComposite) // Composite 바인딩인 경우
            {
                // 첫 번째 파트가 Composite 파트 바인딩(isPartOfComposite)인 경우
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
            }
            else // Composite가 아닌 경우(단일 바인딩), ex) 버튼
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
        }

        // 실제 재바인딩 프로세스를 구성하고 실행하는 핵심 메서드
        void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            // 이미 진행 중인 재바인딩이 있으면 취소
            m_RebindOperation?.Cancel();

            // 재바인딩이 완료되거나 취소된 후, 메모리 해제를 위한 로컬 함수
            void CleanUp()
            {
                m_RebindOperation?.Dispose();
                m_RebindOperation = null;
                action.Enable();                // (중요) 액션을 다시 활성화해야 함
                SaveActionBinding();            // 현재 바인딩 상태를 저장
            }

            // 액션 활성화 상태에서 리바인딩 불가하므로, 비활성화 시켜줘야 함
            action.Disable();

            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")      // 키보드의 ESC 키로 취소 가능
                .OnCancel( 
                    // 리바인딩 도중에 취소된 경우 실행되는 콜백
                    operation =>
                    {
                        m_RebindStopEvent?.Invoke(this, operation);     
                        UI_TitleEventBus.OnActiveKeyConfirmationCanvas?.Invoke(false); // 키 확인 캔버스 닫기
                        UpdateBindingDisplay();
                        CleanUp();
                    })
                .OnComplete( 
                    // 리바인딩이 완료되어 호출되는 콜백
                    operation =>
                    {
                        m_RebindStopEvent?.Invoke(this, operation);
                        UI_TitleEventBus.OnActiveKeyConfirmationCanvas?.Invoke(false); // 키 확인 캔버스 닫기
                        UpdateBindingDisplay();
                        CleanUp();

                        // Composite 파트 처리
                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                        }
                    });

            // part 바인딩 이름 설정
            // Composite 파트(예: “Up”, “Down”, “Left”, “Right” 등) 중 하나를 지정할 때,
            // m_RebindText.text = $"{partName}Waiting for {m_RebindOperation.expectedControlType} input..."; 처럼
            // “어떤 파트(Up/ Down 등)”를 텍스트로 안내해 줄 수 있다.
            var partName = default(string);
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

            #region 키 리바인딩 오버레이 관련
            // 오버레이 켜기 및 Rebind 텍스트(ex: button, key, axis를 기다리는 중인지) 설정
            UI_TitleEventBus.OnActiveKeyConfirmationCanvas?.Invoke(true); // 키 확인 캔버스 열기
            //if (m_RebindText != null)
            //{
            //    var text = !string.IsNullOrEmpty(m_RebindOperation.expectedControlType)
            //        ? $"{partName}Waiting for {m_RebindOperation.expectedControlType} input..."
            //        : $"{partName}Waiting for input...";
            //    m_RebindText.text = text;
            //}
            #endregion

            if (m_RebindStartEvent == null && _bindingText != null)
                _bindingText.text = "<Waiting...>";

            // 바인딩 시작 이벤트 호출
            // 외부에서 “재바인딩 시작” 시에 수행할 로직(사운드 재생, 애니메이션)이 있다면 이곳에서 콜백으로 실행
            m_RebindStartEvent?.Invoke(this, m_RebindOperation);

            m_RebindOperation.Start();
        }

        void SaveActionBinding()
        {
            var currentBindings = _inputSystemAction.action.actionMap.SaveBindingOverridesAsJson();
            //PlayerPrefs.SetString(_inputSystemAction.action.name + _bindingId, currentBindings);
            PlayerPrefs.SetString("rebinds", currentBindings);
            //Debug.Log($"{transform.name} 저장: \n 키: {_inputSystemAction.action.name + _bindingId} \n 값: {currentBindings}");
            Managers.Input.ApplyKeyBind(currentBindings);
        }

        void LoadActionBinding()
        {
            //var savedBindings = PlayerPrefs.GetString(_inputSystemAction.action.name + _bindingId, string.Empty);
            var savedBindings = PlayerPrefs.GetString("rebinds", string.Empty);
            if (!string.IsNullOrEmpty(savedBindings))
            {
                actionReference.action.actionMap.LoadBindingOverridesFromJson(savedBindings);
                Managers.Input.ApplyKeyBind(savedBindings);
                //Debug.Log($"{transform.name} 로드: \n 키: {_inputSystemAction.action.name + _bindingId} \n 값: {savedBindings}");
            }
        }

        protected void OnEnable()
        {
            if (s_RebindActionUIs == null)
                s_RebindActionUIs = new List<RebindActionUI>();
            s_RebindActionUIs.Add(this);
            if (s_RebindActionUIs.Count == 1)
                InputSystem.onActionChange += OnActionChange;
        }

        protected void OnDisable()
        {
            m_RebindOperation?.Dispose();
            m_RebindOperation = null;

            s_RebindActionUIs.Remove(this);
            if (s_RebindActionUIs.Count == 0)
            {
                s_RebindActionUIs = null;
                InputSystem.onActionChange -= OnActionChange;
            }
        }

        // Action 또는 ActionMap, ActionAsset에 변화가 생겼을 때 호출되는 콜백 메서드
        // 이 클래스의 모든 활성화된 인스턴스(s_RebindActionUIs)를 순회하며, “내가 참조하고 있는 Action/ActionMap/ActionAsset이 바뀌었는가를 판단하고,
        // 해당 인스턴스에 대해 UpdateBindingDisplay()를 호출하여 UI를 갱신
        private static void OnActionChange(object obj, InputActionChange change)
        {
            // 바인딩 대상이나 컨트롤 레이아웃이 바뀐 경우가 아닌 경우
            if (change != InputActionChange.BoundControlsChanged)
                return;

            var action = obj as InputAction;
            var actionMap = action?.actionMap ?? obj as InputActionMap;
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

            for (var i = 0; i < s_RebindActionUIs.Count; ++i)
            {
                var component = s_RebindActionUIs[i];
                var referencedAction = component.actionReference?.action;
                if (referencedAction == null)
                    continue;

                if (referencedAction == action ||
                    referencedAction.actionMap == actionMap ||
                    referencedAction.actionMap?.asset == actionAsset)
                    component.UpdateBindingDisplay();
            }
        }

        // We want the label for the action name to update in edit mode, too, so
        // we kick that off from here.
#if UNITY_EDITOR
        protected void OnValidate()
        {
            //UpdateActionLabel();
            UpdateBindingDisplay();
        }
#endif

        private void UpdateActionLabel()
        {
            //if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            //    return;

            //if (_actionLabel != null)
            //{
            //    var binding = action.bindings[bindingIndex];
            //    if (binding.isPartOfComposite)
            //    {
            //        _actionLabel.text = binding.name;
            //    }
            //    else
            //    {
            //        _actionLabel.text = action != null ? action.name : string.Empty;
            //    }
            //}

            // 원본
            if (_actionLabel != null)
            {
                var action = _inputSystemAction?.action;
                _actionLabel.text = action != null ? action.name : string.Empty;
            }
        }

        // 바인딩 문자열이 갱신될 때, 외부 코드(또는 인스펙터 연결)로 알림을 주기 위한 이벤트
        [Serializable]
        public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string>
        {
        }

        // 지금부터 재바인딩을 시작한다거나 지금 재바인딩이 끝났다(또는 취소되었다)는 정보를 외부 코드(또는 인스펙터)로 전달
        [Serializable]
        public class InteractiveRebindEvent : UnityEvent<RebindActionUI, InputActionRebindingExtensions.RebindingOperation>
        {
        }
        #endregion

        #region 주석
        //public TextMeshProUGUI actionLabel
        //{
        //    get => _actionLabel;
        //    set
        //    {
        //        _actionLabel = value;
        //        // UpdateActionLabel();
        //    }
        //}

        //public UpdateBindingUIEvent updateBindingUIEvent
        //{
        //    get
        //    {
        //        if (m_UpdateBindingUIEvent == null)
        //            m_UpdateBindingUIEvent = new UpdateBindingUIEvent();
        //        return m_UpdateBindingUIEvent;
        //    }
        //}

        //public InteractiveRebindEvent startRebindEvent
        //{
        //    get
        //    {
        //        if (m_RebindStartEvent == null)
        //            m_RebindStartEvent = new InteractiveRebindEvent();
        //        return m_RebindStartEvent;
        //    }
        //}

        //public InteractiveRebindEvent stopRebindEvent
        //{
        //    get
        //    {
        //        if (m_RebindStopEvent == null)
        //            m_RebindStopEvent = new InteractiveRebindEvent();
        //        return m_RebindStopEvent;
        //    }
        //}

        //public InputActionRebindingExtensions.RebindingOperation ongoingRebind => m_RebindOperation;
        #endregion
    }
}
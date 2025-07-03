using CKT;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using YSJ;

namespace BMC
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance => _instance;
        static TutorialManager _instance;

        [field: SerializeField] public bool IsEquipParts { get; set; } //아티팩트에 파츠를 장착했는지
        [SerializeField] bool _isUsedRightHand;  //우클릭으로 스킬을 사용했는지
        public Action<bool> OnEquipPartsAction;

        [SerializeField] Artifact_YSJ _artifact;
        [SerializeField] CanDragItem_YSJ _part;
        [SerializeField] ArtifactWindow_YSJ _artifactWindow;

        [Header("입력 관련")]
        InputManager _input;
        InputSystemActions _inputSystemActions;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            _input = Managers.Input;
            _inputSystemActions = _input.InputSystemActions;
            SubscribeInputSystem();

            YSJ.Managers.Input.OnInteractAction += CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd += CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction += CheckRightHand;
        }

        void OnDisable()
        {
            Destroy(_artifact.gameObject);
            _artifactWindow.ResetWindow();

            OnEquipPartsAction = null;

            YSJ.Managers.Input.OnInteractAction -= CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction -= CheckRightHand;

            _instance = null;
        }

        #region [CheckInteraction]
        void CheckInteraction()
        {
            StartCoroutine(CheckInteractionCoroutine());
        }

        IEnumerator CheckInteractionCoroutine()
        {
            yield return null;
            _artifact = BMC.PlayerManager.Instance.transform.Find("Hand").GetComponentInChildren<Artifact_YSJ>();
            _part = BMC.PlayerManager.Instance.GetComponentInChildren<Inventory_YSJ>().GetComponentInChildren<CanDragItem_YSJ>();
            _artifactWindow = Managers.UI.InventoryCanvas.ArtifactWindow;

            // 아티팩트 + 파츠 장착한 경우
            if (_artifact != null && _part != null)
            {
                OnEquipPartsAction.Invoke(true);
                EnableOnlyAction(_inputSystemActions.Tutorial.LeftHand);
                YSJ.Managers.Input.OnInteractAction -= CheckInteraction;
                Debug.LogError("아티팩트와 파츠를 먹어서 상호작용 확인 제거");
            }
        }
        #endregion

        #region [CheckLeftHand]
        void CheckLeftHandEnd()
        {
            StartCoroutine(CheckLeftHandEndCoroutine());
        }

        IEnumerator CheckLeftHandEndCoroutine()
        {
            yield return null;
            if (_part != null && IsEquipParts)
            {
                OnEquipPartsAction.Invoke(false);
                YSJ.Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
                EnableActionMap();
                Debug.LogError("아티팩트와 파츠를 먹어서 평타 공격 확인 제거");
            }
        }
        #endregion

        #region [CheckRightHand]
        void CheckRightHand()
        {
            StartCoroutine(CheckRightHandCoroutine());
        }

        IEnumerator CheckRightHandCoroutine()
        {
            yield return null;            
            //아티팩트를 장착해야 한다 + 파츠를 장착해야 한다
            if ((_artifact != null) && (_part != null) && _isUsedRightHand == false)
            {
                _isUsedRightHand = true;
                YSJ.Managers.Input.OnRightHandAction -= CheckRightHand;
                Debug.LogError("아티팩트와 파츠를 먹고 난 후, 스킬 공격 했으니 스킬 공격 확인 제거");
            }
        }
        #endregion

        #region [TutorialClear]
        public void TutorialClear()
        {
            if (_isUsedRightHand)
            {
                //문 열림
                StageManager.Instance.CurrentRoom.OpenAllValidDoor();
            }
        }
        #endregion

        #region 튜토리얼 Input
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
        public void EnableOnlyAction(InputAction allowedAction)
        {
            var map = _inputSystemActions.Tutorial;

            // 맵 전체 비활성화
            map.Disable();

            // 허용된 액션만 활성화
            allowedAction.Enable();
        }

        // 튜토리얼 액션 맵 전체 활성화
        public void EnableActionMap()
        {
            var map = _inputSystemActions.Tutorial;

            // 맵 전체 활성화
            map.Enable();
        }
        #endregion
    }
}
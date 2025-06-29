using System;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance => _instance;
        static TutorialManager _instance;

        BMC.Door _upDoor;
        UI_EquipcParts _ui_equipParts; //파츠를 획득하면 장착하라는 UI 활성화
        bool _isRightHand; //우클릭으로 스킬을 사용했는지

        InventorySlot _inventorySlot;
        RightSlot _rightSlot;

        private void Awake()
        {
            _instance = _instance ?? this;
        }

        private void Start()
        {
            _upDoor = transform.parent.GetComponentInChildren<BMC.Door>();
            _ui_equipParts = transform.parent.GetComponentInChildren<UI_EquipcParts>();

            _inventorySlot = FindAnyObjectByType<InventorySlot>();
            _rightSlot = FindAnyObjectByType<RightSlot>();

            YSJ.Managers.Input.OnInteractAction += CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd += CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction += CheckRightHand;
        }

        private void OnDisable()
        {
            YSJ.Managers.Input.OnInteractAction -= CheckInteraction;
            YSJ.Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
            YSJ.Managers.Input.OnRightHandAction -= CheckRightHand;
        }

        #region [CheckInteraction]
        void CheckInteraction()
        {
            StartCoroutine(CheckInteractionCoroutine());
        }

        IEnumerator CheckInteractionCoroutine()
        {
            yield return null;
            ImageParts imageParts = _inventorySlot.GetComponentInChildren<ImageParts>();
            if (imageParts != null)
            {
                _ui_equipParts.CanvasEnable(true);
                YSJ.Managers.Input.OnInteractAction -= CheckInteraction;
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
            ImageParts imageParts = _rightSlot.GetComponentInChildren<ImageParts>();
            if (imageParts != null)
            {
                _ui_equipParts.CanvasEnable(false);
                YSJ.Managers.Input.OnLeftHandActionEnd -= CheckLeftHandEnd;
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
            EquipedArtifact equipedArtifact = BMC.PlayerManager.Instance.GetComponentInChildren<EquipedArtifact>();
            ImageParts imageParts = _rightSlot.GetComponentInChildren<ImageParts>();
            //아티팩트를 장착해야 한다 + 파츠를 장착해야 한다
            if ((equipedArtifact != null) && (imageParts != null))
            {
                _isRightHand = true;
                YSJ.Managers.Input.OnRightHandAction -= CheckRightHand;
            }
        }
        #endregion

        #region [TutorialClear]
        public void TutorialClear()
        {
            if (_isRightHand)
            {
                //문 열림
                _upDoor.Open();
            }
        }
        #endregion
    }
}
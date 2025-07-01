using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CKT
{
    public class PlayerController : MonoBehaviour
    {
        #region [컴포넌트]
        Rigidbody2D _rigid;
        #endregion

        #region [자식 오브젝트]
        Transform _leftHand;
        Transform _rightHand;
        #endregion

        #region [입력값]
        Vector2 _moveValue;
        bool _interactValue;
        bool _leftHandValue;
        bool _rightHandValue;
        #endregion

        #region [값]
        float _moveSpeed = 10f;
        float _scanRange = 2f;
        #endregion

        private void Awake()
        {
            _rigid = _rigid ?? GetComponent<Rigidbody2D>();

            _leftHand = GetComponentInChildren<LeftHand>().transform;
            _rightHand = GetComponentInChildren<RightHand>().transform;
        }

        private void Start()
        {
            _rigid.gravityScale = 0;
            _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void FixedUpdate()
        {
            if (_moveValue != Vector2.zero)
            {
                Move(_moveValue, _moveSpeed);
            }

            if (_interactValue)
            {
                _interactValue = false;
                InteractItem();
            }

            if (_leftHandValue)
            {
                _leftHandValue = false;
                //GameManager.Instance.LeftSkillManager.TriggerHand();
            }

            if (_rightHandValue)
            {
                _rightHandValue = false;
                //GameManager.Instance.RightSkillManager.TriggerHand();
            }
        }

        #region [PlayerInput]
        void OnMove(InputValue value)
        {
            _moveValue = value.Get<Vector2>();
        }

        void OnInteract(InputValue value)
        {
            _interactValue = value.isPressed;
        }

        void OnLeftHand(InputValue value)
        {
            _leftHandValue = value.isPressed;
        }

        void OnRightHand(InputValue value)
        {
            _rightHandValue = value.isPressed;
        }
        #endregion

        #region [Method]
        void Move(Vector2 moveValue, float moveSpeed)
        {
            Vector3 moveDir = moveValue * _moveSpeed * Time.deltaTime;
            _rigid.MovePosition(this.transform.position + moveDir);
        }

        void InteractItem()
        {
            Transform target = ScanTarget(_scanRange);
            IInteractable iInteractable = null;

            if (target != null)
            {
                iInteractable = target.GetComponent<IInteractable>();
            }

            if (iInteractable != null)
            {
                if (iInteractable.ItemType == ItemType.Parts)
                {
                    //if (!BMC.PlayerManager.Instance.Inventory.CheckInventorySlotFull())
                    //{
                    //    iInteractable.Interact(null);
                    //}
                }
                else if (iInteractable.ItemType == ItemType.Artifact)
                {
                    if (_leftHand.childCount == 0)
                    {
                        iInteractable.Interact(_leftHand);
                    }
                    else
                    {
                        if (_rightHand.childCount == 0)
                        {
                            iInteractable.Interact(_rightHand);
                        }
                        else
                        {
                            Debug.Log("아티펙트 교체 UI 띄우기");
                        }
                    }
                }
            }
        }

        Transform ScanTarget(float scanRange)
        {
            float sqrRange = scanRange * scanRange;
            Transform target = null;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, scanRange, Vector2.up, 0, ~(1 << this.gameObject.layer));
            for (int i = 0; i < hits.Length; i++)
            {
                float sqrDistance = (hits[i].transform.position - this.transform.position).sqrMagnitude;
                if (sqrDistance < sqrRange)
                {
                    sqrRange = sqrDistance;
                    target = hits[i].transform;
                }
            }

            return target;
        }

        #endregion
    }
}
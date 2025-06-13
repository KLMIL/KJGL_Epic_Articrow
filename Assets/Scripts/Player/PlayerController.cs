using UnityEngine;
using CKT;
using YSJ;
using UnityEditor.SceneManagement;

namespace BMC
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D _rigid;
        PlayerAnimator _playerAnimator;

        PlayerDash _playerDash;

        #region [자식 오브젝트]
        Transform _leftHand;
        Transform _rightHand;

        IAttackable _leftArtifact;
        IAttackable _rightArtifact;
        #endregion

        #region [값]
        float _moveSpeed = 5f;
        float _dampScale = 0.5f;

        float _scanRange = 1.5f;
        #endregion

        LayerMask _interactLayerMask;

        void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerDash = GetComponent<PlayerDash>();

            _leftHand = GetComponentInChildren<YSJ.LeftHand>().transform;
            _rightHand = GetComponentInChildren<YSJ.RightHand>().transform;

            YSJ.Managers.Input.OnInteractAction += InteractItem;

            _interactLayerMask = LayerMask.GetMask("Interact");

            _playerDash = gameObject.AddComponent<PlayerDash>();
        }

        void Start()
        {
            transform.SetParent(null);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(2)) // 마우스 휠 클릭
            {
                MapManager.Instance.CurrentRoom.Complete();
            }
        }

        void FixedUpdate()
        {
            Vector2 moveInput = YSJ.Managers.Input.MoveInput;
            if (!_playerDash.Silhouette.IsActive)
            {
                if (moveInput != Vector2.zero)
                {
                    Move(moveInput, _moveSpeed);
                }
                else
                {
                    Damp(_dampScale);
                }
            }

            if (YSJ.Managers.Input.IsPressLeftHandAttack)
            {
                //_leftHandValue = false;
                //Debug.Log("좌수1");
                GameManager.Instance.LeftSkillManager.TriggerHand();
            }
            else
            {
                GameManager.Instance.LeftSkillManager.TriggerHandCancel();
            }

            if (YSJ.Managers.Input.IsPressRightHandAttack)
            {
                //_rightHandValue = false;
                //Debug.Log("우수");
                GameManager.Instance.RightSkillManager.TriggerHand();
            }
            else
            {
                GameManager.Instance.RightSkillManager.TriggerHandCancel();
            }
        }

        #region [Move]
        void Move(Vector2 inputValue, float moveSpeed)
        {
            if ((_rigid == null) || (_playerAnimator == null))
            {
                Debug.LogError("RigidBody2D or PlayerAnimator is null");
                return;
            }

            // 최종 이동속도
            Vector2 moveDir = inputValue * _moveSpeed;

            // 이동하기 전에 더 작은 속도를 뺄셈 (외력으로 인한 속도가 높아질 것을 고려)
            bool lowSpeed = (_rigid.linearVelocity.sqrMagnitude <= moveDir.sqrMagnitude);
            Vector2 reverseDir = lowSpeed ? _rigid.linearVelocity : moveDir;

            _rigid.linearVelocity -= reverseDir;
            _rigid.linearVelocity += moveDir;
            _playerAnimator.CurrentState |= PlayerAnimator.State.Walk;
        }
        #endregion

        #region [Damp]
        void Damp(float dampScale)
        {
            if ((_rigid == null) || (_playerAnimator == null))
            {
                Debug.LogError("RigidBody2D or PlayerAnimator is null");
                return;
            }

            _rigid.linearVelocity *= dampScale;
            _playerAnimator.CurrentState &= ~PlayerAnimator.State.Walk;
        }
        #endregion

        #region [Interact]
        void InteractItem()
        {
            Transform target = ScanTarget(_scanRange);
            IInteractable iInteractable = null;

            //Debug.Log("1");
            if (target != null)
            {
                //Debug.Log("2");
                iInteractable = target.GetComponent<IInteractable>();
            }

            if (iInteractable != null)
            {
                //Debug.Log("3");
                if (iInteractable.ItemType == ItemType.Parts)
                {
                    //Debug.Log("4");
                    if (!GameManager.Instance.Inventory.CheckInventorySlotFull())
                    {
                        //Debug.Log("5");
                        iInteractable.Interact(null);
                    }
                }
                else if (iInteractable.ItemType == ItemType.Artifact)
                {
                    //Debug.Log("6");
                    if (_leftHand.childCount == 0)
                    {
                        //Debug.Log("7");
                        iInteractable.Interact(_leftHand);
                    }
                    else
                    {
                        //Debug.Log("8");
                        if (_rightHand.childCount == 0)
                        {
                            //Debug.Log("9");
                            iInteractable.Interact(_rightHand);
                        }
                        else
                        {
                            //Debug.Log("10");
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

            RaycastHit2D[] hits = Physics2D.CircleCastAll(this.transform.position, scanRange, Vector2.up, 0, _interactLayerMask);
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
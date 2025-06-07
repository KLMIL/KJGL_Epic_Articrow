using UnityEngine;
using CKT;
using YSJ;

namespace BMC
{
    public class DummyPlayerController : MonoBehaviour
    {
        PlayerMove _playerMove = new PlayerMove();
        PlayerAnimator _playerAnimator;
        Rigidbody2D _rigid;

        #region [자식 오브젝트]
        Transform _leftHand;
        Transform _rightHand;

        IAttackable _leftArtifact;
        IAttackable _rightArtifact;
        #endregion

        #region [입력값]
        bool _leftHandValue;
        bool _rightHandValue;
        #endregion

        #region [값]
        float _scanRange = 2f;
        #endregion

        LayerMask _interactLayerMask;

        void Awake()
        {
            _playerAnimator = GetComponent<PlayerAnimator>();
            _rigid = GetComponent<Rigidbody2D>();

            _leftHand = GetComponentInChildren<LeftHand_YSJ>().transform;
            _rightHand = GetComponentInChildren<RightHand_YSJ>().transform;

            YSJ.Managers.Input.OnInteractAction = InteractItem;

            _interactLayerMask = LayerMask.GetMask("Interact");
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
            _playerMove.Move(Managers.Input.MoveInput, _rigid, _playerAnimator);

            if (YSJ.Managers.Input.IsPressLeftHandAttack)
            {
                //_leftHandValue = false;
                //Debug.Log("좌수1");
                GameManager.Instance.Inventory.InvokeLeftHand();
            }

            if (YSJ.Managers.Input.IsPressRightHandAttack)
            {
                //_rightHandValue = false;
                //Debug.Log("우수");
                GameManager.Instance.Inventory.InvokeRightHand();
            }
        }

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
    }
}
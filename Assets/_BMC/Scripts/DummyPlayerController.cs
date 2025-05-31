using CKT;
using UnityEngine;
using YSJ;

namespace BMC
{
    public class DummyPlayerController : MonoBehaviour
    {
        PlayerMove _playerMove;

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
            _leftHand = GetComponentInChildren<LeftHand_YSJ>().transform;
            _rightHand = GetComponentInChildren<RightHand_YSJ>().transform;

            _playerMove = GetComponent<PlayerMove>();

            YSJ.Managers.Input.interactAction = InteractItem;

            _interactLayerMask = LayerMask.GetMask("Interact");
        }

        void Start()
        {
            transform.SetParent(null);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MapManager.Instance.CurrentRoom.Complete();
            }
        }

        void FixedUpdate()
        {
            _playerMove.Move();

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
                iInteractable = target.GetComponent<IInteractable>();
                //Debug.Log("2");
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
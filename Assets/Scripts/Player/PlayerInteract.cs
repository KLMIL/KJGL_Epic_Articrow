using UnityEngine;

namespace CKT
{
    public class PlayerInteract : MonoBehaviour
    {
        Transform _rightHand;

        float _scanRange;
        LayerMask _interactLayerMask;

        private void Awake()
        {
            _rightHand = GetComponentInChildren<YSJ.RightHand>().transform;
        }

        void Start()
        {
            _scanRange = 1.2f;
            _interactLayerMask = LayerMask.GetMask("Interact");

            YSJ.Managers.Input.OnInteractAction += InteractItem;
        }

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
                    //Debug.Log("8");
                    if (_rightHand.childCount != 0)
                    {
                        //Debug.Log("9");
                        //GameManager.Instance.RightSkillManager.OnThrowAwayActionT0.Trigger();
                        _rightHand.GetComponentInChildren<EquipedArtifact>()?.ThrowAway();
                    }

                    iInteractable.Interact(_rightHand);
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
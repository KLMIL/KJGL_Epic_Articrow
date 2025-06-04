using UnityEngine;

namespace YSJ
{
    public abstract class CanEnterSlot : MonoBehaviour
    {
        GameObject isSelect;
        public virtual void Init()
        {
            isSelect = Instantiate(Resources.Load<GameObject>("Canvas/SelectBox"));
            isSelect.transform.SetParent(transform, false);
            isSelect.transform.SetSiblingIndex(0);
            isSelect.SetActive(false);
        }

        public void EnterSlot()
        {
            isSelect.SetActive(true);
        }

        public void ExitSlot()
        {
            isSelect.SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

namespace CKT
{
    public class UI_DropArea : MonoBehaviour, IDropHandler
    {
        #region [컴포넌트]
        RectTransform _rect;
        #endregion

        void Start()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject pointerDrag = eventData.pointerDrag;

            //DropArea 위에서 Drop했을 때만 호출
            pointerDrag.transform.SetParent(this.transform);
            pointerDrag.GetComponent<RectTransform>().position = _rect.position;

            GameManager.Instance.Inventory.InvokeUpdateList();
        }
    }
}
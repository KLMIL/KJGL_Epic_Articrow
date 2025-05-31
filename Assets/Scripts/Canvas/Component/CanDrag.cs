using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace YSJ
{
    public class CanDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private Canvas canvas;

        private Vector2 pointerOffset;

        Vector2 beforePos;
        Transform beforeParent;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = transform.root.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas not found in parent hierarchy.");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);



            beforeParent = transform.parent;
            transform.parent = transform.root;
            pointerOffset = rectTransform.localPosition - (Vector3)localPointerPosition;
            beforePos = rectTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);

            rectTransform.localPosition = localPointerPosition + pointerOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 드래그한 오브젝트가 겹치는 UI Slot이 있는지 검사
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = eventData.position;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            foreach (var result in results)
            {
                Slot slot = result.gameObject.GetComponent<Slot>();
                if (slot != null && slot.transform.childCount == 0)
                {
                    rectTransform.position = slot.transform.position;
                    transform.SetParent(slot.transform);
                    return;
                }
            }

            // 아무 Slot과도 겹치지 않으면 원래 위치로
            rectTransform.position = beforePos;
            transform.SetParent(beforeParent);
        }
    }
}
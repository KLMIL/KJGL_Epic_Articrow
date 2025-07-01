using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanDragItem_YSJ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform currentParent;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        currentParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas) 
        {
            transform.SetParent(parentCanvas.transform);
        }
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(currentParent);
    }
}

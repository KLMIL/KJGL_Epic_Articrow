using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanDragItem_YSJ))]
public abstract class ImagePartsRoot_YSJ : MonoBehaviour
{
    public abstract string partsName { get; }
}

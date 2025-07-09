using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSJ;

public class CheckNearInteractableObj_YSJ : MonoBehaviour
{
    List<Collider2D> nearInteractableColliders = new();
    LayerMask _layerMask;

    void Awake()
    {
        _layerMask = LayerMask.GetMask("Interact");
    }

    void Update()
    {
        float interactDistance = BMC.PlayerManager.Instance.PlayerStatus.InteractDistance;
        List<Collider2D> cols = Physics2D.OverlapCircleAll(transform.position, interactDistance, _layerMask).ToList();
        foreach (Collider2D col in cols)
        {
            if (col.TryGetComponent<ShowInteractableWindow_YSJ>(out ShowInteractableWindow_YSJ obj) && !nearInteractableColliders.Contains(col))
            {
                nearInteractableColliders.Add(col);
                obj.ShowWindow();
            }
        }

        List<Collider2D> toRemove = new();
        foreach (Collider2D obj in nearInteractableColliders)
        {
            if (!cols.Contains(obj) )
            {
                if (obj)
                {
                    obj.GetComponent<ShowInteractableWindow_YSJ>().HideWindow();
                }
                toRemove.Add(obj);
            }
        }

        foreach (Collider2D obj in toRemove)
        {
            nearInteractableColliders.Remove(obj);
        }
    }
}

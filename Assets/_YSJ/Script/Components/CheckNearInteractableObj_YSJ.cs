using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSJ;

public class CheckNearInteractableObj_YSJ : MonoBehaviour
{
    public float distance = 0.6f;
    List<Collider2D> nearInteractableColliders = new();

    void Update()
    {
        List<Collider2D> cols = Physics2D.OverlapCircleAll(transform.position, distance).ToList();
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
            if (!cols.Contains(obj))
            {
                obj.GetComponent<ShowInteractableWindow_YSJ>().HideWindow();
                toRemove.Add(obj);
            }
        }

        foreach (Collider2D obj in toRemove)
        {
            nearInteractableColliders.Remove(obj);
        }
    }
}

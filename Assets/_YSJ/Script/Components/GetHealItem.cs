using System;
using UnityEngine;
using YSJ;

public class GetHealItem : MonoBehaviour
{
    void Start()
    {
        Managers.Input.OnInteractAction += TryGetHeal;
    }

    private void TryGetHeal()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);

        foreach (Collider2D collider in colliders) 
        {
            if (collider.TryGetComponent<HealItem>(out HealItem healItem)) 
            {
                healItem.Heal(GetComponent<PlayerStatus>());
            }
        }
    }
}

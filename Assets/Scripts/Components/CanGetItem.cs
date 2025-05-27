using System.Collections.Generic;
using UnityEngine;

public class CanGetItem : MonoBehaviour
{
    Collider2D col;
    // 한번 겹쳤던 아이템 중복체크 안하게하기위해서
    List<Collider2D> contactItems = new();

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Item>(out Item contactItem) && !contactItems.Contains(collision))
        {
            if (UIManager.Instance.inventory.TryGetitem(contactItem))
            {
                Destroy(contactItem.gameObject);
            }
            else 
            {
                contactItems.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (contactItems.Contains(collision))
        {
            contactItems.Remove(collision);
        }
    }
}

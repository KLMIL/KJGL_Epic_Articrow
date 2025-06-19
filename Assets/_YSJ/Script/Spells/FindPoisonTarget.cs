using Unity.VisualScripting;
using UnityEngine;

public class FindPoisonTarget : MonoBehaviour
{
    public void RayCastShooot(Vector2 direction, float time, float damage, float interval)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, .1f, LayerMask.GetMask("Monster"));
        if (hit.collider != null)
        {
            DoingPoison poison = hit.collider.AddComponent<DoingPoison>();
            poison.Initialize(time, damage, interval);
        }
        else 
        {
            print("타겟없음");
        }
    }

    private void Start()
    {
        Destroy(gameObject);
    }
}

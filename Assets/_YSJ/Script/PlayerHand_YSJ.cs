using UnityEngine;

public class PlayerHand_YSJ : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        RotationHand();
    }

    void RotationHand() 
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector2 Direction = (MousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}

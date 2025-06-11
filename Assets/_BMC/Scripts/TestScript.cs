using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("콜리전");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거");
    }
}

using TMPro;
using UnityEngine;

public class TA_MoveUpRandom : MonoBehaviour
{
    public float moveSpeed;
    public float moveTime;

    Vector2 targetDirection;
    Color color;
    TextMeshPro tmp;
    float elapsedTime;

    private void OnEnable()
    {
        targetDirection = Vector2.up; //new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f));
        tmp = GetComponent<TextMeshPro>();
        color = tmp.color;
    }
    void FixedUpdate()
    {
        Vector2 movePostion = (Vector2)transform.position + targetDirection * moveSpeed * Time.fixedDeltaTime;
        transform.position = movePostion;

        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > moveTime)
        {
            Destroy(gameObject);
        }

        color.a = Mathf.Lerp(1, 0, elapsedTime / moveTime);
        tmp.color = color;
    }
}

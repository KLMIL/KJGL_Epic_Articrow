using TMPro;
using UnityEngine;
using YSJ;

public class TA_MoveUpRandom : MonoBehaviour
{
    public float moveSpeed;
    public float moveTime;

    Vector2 targetDirection;
    Color color;
    TextMeshPro tmp;
    float elapsedTime;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        elapsedTime = 0f;
        targetDirection = Vector2.up;
        color = tmp.color;
        color.a = 1f;
        tmp.color = color;
    }
    void FixedUpdate()
    {
        Vector2 movePostion = (Vector2)transform.position + targetDirection * moveSpeed * Time.fixedDeltaTime;
        transform.position = movePostion;

        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime > moveTime)
        {
            //Destroy(gameObject);
            Managers.TestPool.Return(Define.PoolID.DamageText, gameObject);
        }

        color.a = Mathf.Lerp(1, 0, elapsedTime / moveTime);
        tmp.color = color;
    }
}

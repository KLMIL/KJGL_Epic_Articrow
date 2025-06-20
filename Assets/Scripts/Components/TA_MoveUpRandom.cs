using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using YSJ;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TA_MoveUpRandom : MonoBehaviour
{
    public float _moveSpeed;
    public float _existTime;

    Color _color;
    TextMeshPro tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        //alpha 값 초기화
        _color = tmp.color;
        _color.a = 1f;
        tmp.color = _color;
    }
    void FixedUpdate()
    {
        Vector2 movePostion = (Vector2)transform.position + Vector2.up * _moveSpeed * Time.fixedDeltaTime;
        transform.position = movePostion;

        _color = tmp.color;
        float fixedDeltaAlpha = (1 / ((_existTime <= 0) ? 0.01f : _existTime)) * Time.fixedDeltaTime;
        _color.a -= fixedDeltaAlpha;
        tmp.color = _color;

        if (tmp.color.a <= 0)
        {
            tmp.text = "0";
            Managers.TestPool.Return(Define.PoolID.DamageText, gameObject);
        }
    }
}

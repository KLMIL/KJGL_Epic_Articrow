using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "Enemy/Status")]
public class EnemyStatusSO : ScriptableObject
{
    public float moveSpeed = 2f;
    public float health = 100f;
}

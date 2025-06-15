using UnityEngine;

/*
 * 적이 저음 소환됐을 때 상태
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "SpawnedConditionSo",
    menuName = "Enemy/Condition/Spawned"
)]
    public class SpawnedConditionSO : EnemyConditionSO
    {
        public override bool IsMet(EnemyController controller)
        {
            return false;
        }
    }
}

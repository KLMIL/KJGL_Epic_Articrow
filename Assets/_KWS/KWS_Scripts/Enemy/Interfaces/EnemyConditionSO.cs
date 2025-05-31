using UnityEngine;

public abstract class EnemyConditionSO : ScriptableObject, IEnemyCondition
{
    public abstract bool IsMet(EnemyController controller);
}

using UnityEngine;

public abstract class EnemyConditionSO : ScriptableObject
{
    public abstract bool IsMet(EnemyController controller);
}

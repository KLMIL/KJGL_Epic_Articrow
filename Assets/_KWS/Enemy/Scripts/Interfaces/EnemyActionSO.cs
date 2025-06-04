using UnityEngine;

public abstract class EnemyActionSO : ScriptableObject, IEnemyAction
{
    public abstract void Act(EnemyController controller);
}

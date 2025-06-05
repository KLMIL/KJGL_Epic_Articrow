using UnityEngine;

public abstract class EnemyActionSO : ScriptableObject
{
    public abstract void Act(EnemyController controller);
}

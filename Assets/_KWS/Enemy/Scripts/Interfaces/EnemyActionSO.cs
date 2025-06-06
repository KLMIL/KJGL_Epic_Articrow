using UnityEngine;

public abstract class EnemyActionSO : ScriptableObject
{
    public abstract void Act(EnemyController controller);

    // 필요하면 오버라이드, 필요없으면 안 써도 됨
    public virtual void OnEnter(EnemyController controller) { }
    public virtual void OnExit(EnemyController controller) { }
}

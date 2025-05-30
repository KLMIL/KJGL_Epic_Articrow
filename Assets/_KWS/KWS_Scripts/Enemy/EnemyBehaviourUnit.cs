[System.Serializable]
public class EnemyBehaviourUnit
{
    public string name;
    public IEnemyCondition condition;
    public IEnemyAction action;
    public int nextStateIndex;

    public float duration;
    public float elapsedTime;

    public EnemyBehaviourUnit(
        IEnemyCondition condition, 
        IEnemyAction action, 
        int nextStateIndex = -1, 
        string name = "",
        float duration = 1f
        )
    {
        this.condition = condition;
        this.action = action;
        this.nextStateIndex = nextStateIndex;
        this.name = name;
        this.duration = duration;
        this.elapsedTime = 0f;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}

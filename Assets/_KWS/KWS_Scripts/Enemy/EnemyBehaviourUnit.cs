[System.Serializable]
public class EnemyBehaviourUnit
{
    public string name;
    public IEnemyCondition condition;
    public IEnemyAction action;
    public int nextStateIndex;

    public EnemyBehaviourUnit(IEnemyCondition condition, IEnemyAction action, int nextStateIndex = -1, string name = "")
    {
        this.condition = condition;
        this.action = action;
        this.nextStateIndex = nextStateIndex;
        this.name = name;
    }
}

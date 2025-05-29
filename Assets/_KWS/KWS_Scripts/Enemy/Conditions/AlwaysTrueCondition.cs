using UnityEngine;

public class AlwaysTrueCondition : IEnemyCondition
{
    public bool IsMet(EnemyController controller) => true;
}

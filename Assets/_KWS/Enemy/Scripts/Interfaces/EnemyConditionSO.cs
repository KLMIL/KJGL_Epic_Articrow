using UnityEngine;

namespace Game.Enemy
{
    public abstract class EnemyConditionSO : ScriptableObject
    {
        public abstract bool IsMet(EnemyController controller);
    }
}

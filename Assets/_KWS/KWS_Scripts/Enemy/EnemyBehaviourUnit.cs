using UnityEngine;

[System.Serializable]
public class EnemyBehaviourUnit
{
    [Header("Behaviour Name")]
    public string name;
    [Header("Condition: ScriptableObject")]
    public EnemyConditionSO condition;
    [Header("Action: ScriptableObject")]
    public EnemyActionSO action;
    [Header("Next State Index")]
    public int nextStateIndex;
    [Header("Current State Duration")]
    public float duration;
    [Header("Action animation")]
    public string animationName;
    [Header("Interrupt other states")]
    public InterruptType interruptType;

    [HideInInspector]
    public float elapsedTime;


    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}

public enum InterruptType { None, Soft, Hard }
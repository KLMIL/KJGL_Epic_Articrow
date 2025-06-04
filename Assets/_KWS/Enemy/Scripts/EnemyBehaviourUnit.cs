using UnityEngine;

[System.Serializable]
public class EnemyBehaviourUnit
{
    [Header("Behaviour Name")]
    public string stateName;

    [Header("Interrupt other states")]
    public InterruptType interruptType;

    [Header("Condition: ScriptableObject")]
    public EnemyConditionSO condition;

    [Header("Action: ScriptableObject")]
    public EnemyActionSO action;

    [Header("Action animation")]
    public string animationName;

    [Header("Current State Duration")]
    public float duration;

    [Header("Next State Name")]
    public string nextStateName;


    [HideInInspector]
    public float elapsedTime;


    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}

public enum InterruptType { None, Soft, Hard }
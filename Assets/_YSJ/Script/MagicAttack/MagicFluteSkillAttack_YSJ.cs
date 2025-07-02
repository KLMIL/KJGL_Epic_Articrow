using System.Collections.Generic;
using UnityEngine;

public class MagicFluteSkillAttack_YSJ : MagicRoot_YSJ
{
    List<Transform> attacks = new();
    float attackDeltaTime;
    int attackIndex;

    private void Start()
    {

        foreach (Transform child in transform) 
        {
            attacks.Add(child);
        }
        attackDeltaTime = LifeTime / attacks.Count;
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        if (elapsedTime > attackDeltaTime) 
        {
            elapsedTime = 0;

        }
    }
}

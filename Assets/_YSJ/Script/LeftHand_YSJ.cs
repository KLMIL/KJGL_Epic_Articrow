using UnityEngine;

public class LeftHand_YSJ : Hand_YSJ
{
    void Awake()
    {
        checkDirection = GetComponent<CheckDirection_YSJ>();
    }

    void Update()
    {
        SpriteSort();
        SpriteRotation();
    }
}

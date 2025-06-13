using UnityEngine;

namespace YSJ
{
    public class LeftHand : Hand
    {
        void Awake()
        {
            checkDirection = GetComponent<CheckPlayerDirection>();
        }

        void Update()
        {
            SpriteSort();
            SpriteRotation();
        }
    }
}
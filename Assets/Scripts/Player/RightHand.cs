using UnityEngine;

namespace YSJ
{
    public class RightHand : Hand
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
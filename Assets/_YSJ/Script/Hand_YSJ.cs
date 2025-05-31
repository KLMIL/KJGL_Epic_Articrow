using UnityEngine;
using UnityEngine.Rendering;

public class Hand_YSJ : MonoBehaviour
{
    protected CheckDirection_YSJ checkDirection;

    protected void SpriteSort() 
    {
        Transform parent = transform.root;

        if (!parent) 
        {
            Debug.LogError("손의 부모 없음!");
        }

        if (TryGetComponent<SortingGroup>(out SortingGroup sortingGroup) && parent.TryGetComponent<SpriteRenderer>(out SpriteRenderer parentSprite))
        {
            if (transform.position.y > parent.position.y)
            {
                sortingGroup.sortingOrder = parentSprite.sortingOrder - 1;
            }
            else 
            {
                sortingGroup.sortingOrder = parentSprite.sortingOrder + 1;
            }
        }
        else 
        {
            Debug.LogError("손 또는 부모의 스프라이트랜더러 없음!");
        }
    }

    protected void SpriteRotation() 
    {
        if (checkDirection) 
        {
            transform.rotation = Quaternion.Euler(0, 0, checkDirection.Angle - 90);
        }
    }
}

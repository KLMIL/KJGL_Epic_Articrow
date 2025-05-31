using UnityEngine;

public class TestChangeColor_YSJ : MonoBehaviour
{
    public Color up;
    public Color down;
    public Color right;
    public Color left;
    CheckDirection_YSJ checkDirection;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        checkDirection = GetComponent<CheckDirection_YSJ>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (checkDirection.CurrentDirection) 
        {
            case CheckDirection_YSJ.Direction.right:
                spriteRenderer.color = right;
                break;
            case CheckDirection_YSJ.Direction.left:
                spriteRenderer.color = left;
                break;
            case CheckDirection_YSJ.Direction.up:
                spriteRenderer.color = up;
                break;
            case CheckDirection_YSJ.Direction.down:
                spriteRenderer.color = down;
                break;
        }
    }
}

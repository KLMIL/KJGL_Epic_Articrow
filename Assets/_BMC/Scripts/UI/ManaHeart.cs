using UnityEngine;
using UnityEngine.UI;

public enum ManaStatus
{
    Empty,
    Full
}

public class ManaHeart : MonoBehaviour
{
    public Sprite fullMana, emptyMana;
    Image ManaImage;

    private void Awake()
    {
        ManaImage = GetComponent<Image>();
    }

    public void SetHeartImage(ManaStatus status)
    {
        switch (status)
        {
            case ManaStatus.Empty:
                ManaImage.sprite = emptyMana;
                break;
            case ManaStatus.Full:
                ManaImage.sprite = fullMana;
                break;
            default:
                break;
        }
    }
}
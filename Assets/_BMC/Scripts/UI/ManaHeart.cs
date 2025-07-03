using UnityEngine;
using UnityEngine.UI;

public enum ManaStatus
{
    Empty,
    Half,
    Full
}

public class ManaHeart : MonoBehaviour
{
    public Sprite fullMana, halfMana, emptyMana;
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
            case ManaStatus.Half:
                ManaImage.sprite = halfMana;
                break;
            case ManaStatus.Full:
                ManaImage.sprite = fullMana;
                break;
            default:
                break;
        }
    }
}
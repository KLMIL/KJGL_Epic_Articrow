using UnityEngine;
using UnityEngine.UI;

public enum HeartStatus
{
    Empty,
    Half,
    Full
}

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch(status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
            default:
                break;
        }
    }

    public void SetColor(Color color)
    {
        heartImage.color = color;
    }
}

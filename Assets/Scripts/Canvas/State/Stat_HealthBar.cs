using UnityEngine;
using UnityEngine.UI;

public class Stat_HealthBar : MonoBehaviour
{
    Image bar;
    Slider _slider;

    void Start()
    {
        UIManager.Instance.HealthBar = this;
        bar = GetComponent<Image>();
        _slider = GetComponent<Slider>();
    }

    public void SetStateBarLimit(float maxValue = 100f)
    {
        _slider.maxValue = maxValue;
    }

    public void HealthBarUpdate(float value)
    {
        bar.fillAmount = value;
        //_slider.value = value;
    }

    void Update()
    {
        if (GameManager.Instance.player)
        {
            HealthBarUpdate(GameManager.Instance.player.GetComponent<PlayerStatus>().Health / (float)GameManager.Instance.player.GetComponent<PlayerStatus>().MaxHealth);
        }
    }
}

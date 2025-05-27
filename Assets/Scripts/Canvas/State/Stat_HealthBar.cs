using UnityEngine;
using UnityEngine.UI;

public class Stat_HealthBar : MonoBehaviour
{
    Image bar;
    void Start()
    {
        UIManager.Instance.HealthBar = this;
        bar = GetComponent<Image>();
    }

    public void HealthBarUpdate(float value)
    {
        bar.fillAmount = value;
    }

    private void Update()
    {
        if (GameManager.Instance.player)
        {
            HealthBarUpdate(GameManager.Instance.player.GetComponent<Health>().currentHealthPoint / GameManager.Instance.player.GetComponent<Health>().maxHealth);
        }
    }
}

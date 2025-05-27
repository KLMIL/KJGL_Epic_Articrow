using UnityEngine;
using UnityEngine.UI;

public class Stat_ManaBar : MonoBehaviour
{
    Image bar;

    void Start()
    {
        UIManager.Instance.manaBar = this;
        bar = GetComponent<Image>();
    }

    public void ManabarUpdate(float value)
    {
        bar.fillAmount = value;
    }
}

using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LeftHandCoolTime : MonoBehaviour
{
    TextMeshProUGUI tmp;
    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UIManager.Instance.leftHandCoolTime = this;
    }

    public void CoolTimeUpdate(float time)
    {
        tmp.text = (Mathf.Floor(time * 10f) / 10f).ToString();
    }
}

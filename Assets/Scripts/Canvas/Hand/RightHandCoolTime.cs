using TMPro;
using UnityEngine;

public class RightHandCoolTime : MonoBehaviour
{
    TextMeshProUGUI tmp;
    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UIManager.Instance.rightHandCoolTime = this;
    }

    public void CoolTimeUpdate(float time)
    {
        tmp.text = (Mathf.Floor(time * 10f) / 10f).ToString();
    }
}

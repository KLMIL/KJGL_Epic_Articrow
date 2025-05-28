using TMPro;
using UnityEngine;

public class StatText : MonoBehaviour
{
    protected TextMeshProUGUI _statText;

    void Awake()
    {
        _statText = GetComponent<TextMeshProUGUI>();
    }

    public void TextUpdate(float value)
    {
        _statText.text = value.ToString();
    }
}
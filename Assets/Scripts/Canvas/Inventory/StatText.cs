using TMPro;
using UnityEngine;

public class StatText : MonoBehaviour
{
    public void TextUpdate(float value)
    {
        GetComponent<TextMeshProUGUI>().text = value.ToString();
    }
}

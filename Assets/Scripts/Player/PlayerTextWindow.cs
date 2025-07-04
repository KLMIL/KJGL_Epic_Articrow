using TMPro;
using UnityEngine;

namespace BMC
{
    public class PlayerTextWindow : MonoBehaviour
    {
        TextMeshPro _text;

        public void Init()
        {
            _text = GetComponentInChildren<TextMeshPro>(true);
            SetText();
        }

        public void SetText(string text = null)
        {
            _text.text = text;
            gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }
}
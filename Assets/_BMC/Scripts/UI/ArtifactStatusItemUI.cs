using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace BMC
{
    /// <summary>
    /// 아티팩트 스테이터스 항목 UI
    /// </summary>
    public class ArtifactStatusItemUI : MonoBehaviour
    {
        TextMeshProUGUI _valueText;
        void Awake()
        {
            _valueText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        }

        public void SetText(int value)
        {
            _valueText.text = value.ToString();
        }

        public void SetText(float value)
        {
            _valueText.text = value.ToString("F1");
        }

        public void Reset()
        {
            _valueText.text = "";
        }
    }
}
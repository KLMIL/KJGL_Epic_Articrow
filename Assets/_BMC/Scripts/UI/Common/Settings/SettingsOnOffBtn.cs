using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public enum OnOffButtonType
    {
        FullScreen,
        VSync,
    }

    /// <summary>
    /// 설정에서 사용할 On / Off 버튼
    /// </summary>
    public class SettingsOnOffBtn : MonoBehaviour
    {
        Button _btn;
        TextMeshProUGUI _text;
        [SerializeField] OnOffButtonType _btnType;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            _btn.onClick.AddListener(OnClickBtn);
            SettingsManager.Instance.OnOffBtnDict.Add(_btnType, this); // 버튼 등록
        }

        public void SetPlayerPrefsValue<T>(string valueName, T valueType)
        {
            Type type = valueType.GetType();
            if (type == typeof(int))
            {
                PlayerPrefs.SetInt(valueName, Convert.ToInt32(valueType));
            }
            else if(type == typeof(float))
            {
                PlayerPrefs.SetFloat(valueName, Convert.ToSingle(valueType));
            }
            //else if(type == typeof(string))
            //{
            //    PlayerPrefs.SetString(valueName, Convert.ToString(valueType));
            //}
        }
        public void UpdateText(string text)
        {
            _text.text = text;
        }

        void OnClickBtn()
        {
            SettingsManager.Instance.OnOffBtnActionDict[_btnType]?.Invoke();
        }
    }
}
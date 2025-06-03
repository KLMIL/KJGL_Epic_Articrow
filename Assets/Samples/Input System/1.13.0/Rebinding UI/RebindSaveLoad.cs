using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Rebind 저장 및 로드
/// </summary>
public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds)) // 무언가 있으면 로드
            actions.LoadBindingOverridesFromJson(rebinds); // json 문자열에서 로드
    }

    public void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson(); // json 문자열로 저장
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
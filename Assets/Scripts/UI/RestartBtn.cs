using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartBtn : MonoBehaviour
{
    Button _btn;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
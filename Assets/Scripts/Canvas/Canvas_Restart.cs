using UnityEngine;

public class Canvas_Restart : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.CanvasRestart = this;
        GetComponent<Canvas>().enabled = false;
    }
}

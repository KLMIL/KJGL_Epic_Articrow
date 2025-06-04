using UnityEngine;

public class TestBeam : MonoBehaviour
{
    bool isClicked;
    LineRenderer lineRenderer;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isClicked)
            {
            }
            else
            {
                isClicked = true;
                lineRenderer.enabled = true;
            }

            DrawLine();
        }
        else 
        {
            isClicked = false;
            lineRenderer.enabled = false;
        }
    }

    void DrawLine() 
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}

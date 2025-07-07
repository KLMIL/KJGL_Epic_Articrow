using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer _lineRenderer;

    [SerializeField]
    Texture[] _textures;

    int animationStep;

    [SerializeField] float fps = 30f;

    float fpsCounter;

    Transform _target;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void AssignTarget(Vector3 startPosition, Transform newTarget)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, startPosition);
        _target = newTarget;
    }

    void Update()
    {
        _lineRenderer.SetPosition(1, _target.position);

        fpsCounter += Time.deltaTime;
        if(fpsCounter >= 1f / fps)
        {
            animationStep++;
            if(animationStep == _textures.Length)
            {
                animationStep = 0;
            }

            _lineRenderer.material.SetTexture("_MainTex", _textures[animationStep]);

            fpsCounter = 0f;
        }
    }
}

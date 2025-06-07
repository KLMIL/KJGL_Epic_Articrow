using System.Collections.Generic;
using UnityEngine;

// 대시 실루엣
public class Silhouette : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;

    public bool IsActive { get => _isActive; set { _isActive = value; } }
    [SerializeField] bool _isActive = false;            // 실루엣 활성화 여부

    [Header("실루엣")]
    [SerializeField] int _silhouetteCount = 10;         // 생성할 실루엣 개수
    [SerializeField] float _silhouetteTime = 0.1f;      // 실루엣 위치 조정 주기 (작게 하면 촘촘하게 나옴)
    [SerializeField] float _delta = 0;                  // 실루엣 위치 조정 시간 카운트

    [SerializeField] int _idx = 0;                      // 실루엣 리스트 인덱스
    [SerializeField] GameObject _silhouetteParent;      // 실루엣 보관할 오브젝트
    [SerializeField] List<SpriteRenderer> _silhouetteList = new List<SpriteRenderer>(); // 실루엣 리스트

    void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        CreateSilhouette();
    }

    // 실루엣 생성
    void CreateSilhouette()
    {
        _delta = 0;
        _idx = 0;

        if (!_silhouetteParent)
        {
            // 하이어라키 정리를 위해 따로 정리용 오브젝트를 두고, 밑에 위치시키기
            _silhouetteParent = new GameObject(gameObject.name + " SilhouetteList");
            for (int i = 0; i < _silhouetteCount; i++)
            {
                // 빈 게임오브젝트 생성
                GameObject spriteCopy = new GameObject(transform.gameObject.name + " Silhouette " + i);
                spriteCopy.transform.parent = _silhouetteParent.transform;

                // SpriteRenderer 컴포넌트 추가
                SpriteRenderer spriteRenderer = spriteCopy.AddComponent<SpriteRenderer>();
                _silhouetteList.Insert(i, spriteRenderer);
            }
        }
    }

    void Update()
    {
        _delta += Time.deltaTime;
        if (_delta > _silhouetteTime)
        {
            _delta = 0;
            if (_isActive)
            {
                UpdateSilhouette(); // 실루엣 업데이트
            }
            FadeOutSilhouette();    // 실루엣 페이드 아웃
        }
    }

    void UpdateSilhouette()
    {
        if (_silhouetteList.Count > 0)
        {
            _silhouetteList[_idx].sprite = _spriteRenderer.sprite;                              // 지금 주인의 스프라이트를 받아서 실루엣에게 적용.
            _silhouetteList[_idx].flipX = _spriteRenderer.flipX;                                // 주인의 스프라이트 플립을 실루엣에게 적용
            _silhouetteList[_idx].transform.position = transform.position + Vector3.forward;    // 실루엣을 실루엣 주인 위치에 깊이 +1로 위치
            _silhouetteList[_idx].transform.rotation = transform.rotation;                      // 회전 적용
            _silhouetteList[_idx].transform.localScale = transform.localScale;                  // 크기 적용
            _silhouetteList[_idx].color = Color.gray;                                           // 회색

            _idx++;
            if (_idx > _silhouetteList.Count - 1)
                _idx = 0;
        }
    }

    void FadeOutSilhouette()
    {
        // 모든 실루엣 투명하게 하기
        for (int i = 0; _silhouetteList.Count > i; i++)
            _silhouetteList[i].color -= new Color(0, 0, 0, 1f / _silhouetteList.Count);
    }

    public void Clear()
    {
        _silhouetteList.Clear();
        Destroy(_silhouetteParent);
    }
}
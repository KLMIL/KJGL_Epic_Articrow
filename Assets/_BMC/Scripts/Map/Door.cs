using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YSJ;

namespace BMC
{
    public class Door : MonoBehaviour
    {
        [Header("기본 컴포넌트")]
        Animator _anim;
        SpriteRenderer _spriteRenderer;
        [SerializeField] BoxCollider2D _boxCollider;

        [Header("상태")]
        [field: SerializeField] public Room CurrentRoom { get; private set; }
        [field: SerializeField] public bool IsOpen { get; private set; }

        DoorDetectionPlayer _doorDetectionPlayer;

        static Coroutine _nextStageCoroutine;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            CurrentRoom = transform.root.GetComponent<Room>();
            _doorDetectionPlayer = GetComponentInChildren<DoorDetectionPlayer>();
            _doorDetectionPlayer.Init(this);
            _nextStageCoroutine = null;
        }

        #region 문 개방/폐쇄
        // 열기
        public void Open()
        {
            IsOpen = true;
            _anim.Play("Open");
        }

        // 닫기
        public void Close()
        {
            IsOpen = false;
            _boxCollider.enabled = true;
        }

        // 폐기(아예 벽으로 만들고 문 기능 못하게 하기)
        public void Dispose()
        {
            Close();
            enabled = false;
        }
        #endregion

        // 문 열릴 때 문 파괴되는 애니메이션 이벤트
        public void OnOpenAnimationEvent()
        {
            StartCoroutine(DestroyAfterOpenCoroutine());
        }

        IEnumerator DestroyAfterOpenCoroutine()
        {
            float alphaValue = 1;

            Color color = _spriteRenderer.color;
            while (alphaValue > 0)
            {
                alphaValue -= Time.deltaTime;
                color.a = alphaValue;
                _spriteRenderer.color = color;
                yield return null;
            }

            alphaValue = 0;
            color.a = alphaValue;
            _spriteRenderer.color = color;
        }

        // 다음 스테이지로 넘어가기
        public void NextStage()
        {
            int sceneIdx = SceneManager.GetActiveScene().buildIndex;
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            //sceneIdx = (sceneIdx + 1) % sceneCount;
            
            // TODO: 테스트를 위해 무조건 1번 씬(시작 씬)으로 이동
            //sceneIdx = 1;
            //Managers.Scene.LoadScene(sceneIdx);
            if(_nextStageCoroutine == null)
            {
                //Invoke("EnableReviewUI", 1f);
                //_nextStageCoroutine = StartCoroutine(Managers.Scene.LoadSceneCoroutine(sceneIdx));
                GameFlowManager.Instance.RequestNextRoom();
            }
        }

        private void EnableReviewUI()
        {
            Game.Test.MapTestStart.Instance.isReview = true;
        }
    }
}
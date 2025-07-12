using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Dungeon 
{
    public class UI_StageProgressCanvas : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;

        [Header("UI 연결")]
        [SerializeField] RectTransform _viewportRect;
        [SerializeField] RectTransform _stagePathImage;
        [SerializeField] RectTransform _playerIconImage;
        [SerializeField] TMP_Text _progressText;

        [Header("세팅값")]
        [SerializeField] float _roomSpacing = 256f;
        [SerializeField] float _stageMoveDuration = 1.5f;
        [SerializeField] float _iconMoveDuration = 0.5f;

        [Header("내부 상태")]
        [SerializeField] int _nowRoomIndex = 0;
        [SerializeField] int _nextRoomIndex = 1;



        /// <summary>
        /// 진행상황 UI 표시
        /// </summary>
        public void Show(int nowIdx, int nextIdx)
        {
            // 오브젝트 활성화 및 내부 상태값 갱신
            if (_canvas != null)
            {
                _canvas.enabled = true;
            }

            _nowRoomIndex = nowIdx;
            _nextRoomIndex = nextIdx;

            // 텍스트 초기화
            if (_progressText != null)
            {
                // TODO: 로컬라이징 하던가 글씨 없애야함
                _progressText.text = "다음 방으로 이동 중 ...";
            }

            // 시작 상태: 현재 방이 중앙, 플레이어 아이콘도 현재 방 위치에 존재
            CenterRoom(_nowRoomIndex);
            SetPlayerIconPosition(_nowRoomIndex);
        }

        /// <summary>
        /// 진행상황 UI 비활성화
        /// </summary>
        public void Hide()
        {
            //gameObject.SetActive(false);
            if (_canvas != null)
            {
                _canvas.enabled = false;
            }
        }


        /// <summary>
        /// Viewport 중앙에 특정 방이 오도록 던전 스프라이트 이동
        /// </summary>
        private void CenterRoom(int roomIdx)
        {
            float offsetX = -(roomIdx * _roomSpacing) - (_roomSpacing / 2f);
            _stagePathImage.anchoredPosition = new Vector2(offsetX, _stagePathImage.anchoredPosition.y);
        }

        /// <summary>
        /// 플레이어 아이콘을 해당 방 위치에 맞춰 이동
        /// </summary>
        private void SetPlayerIconPosition(int roomIdx)
        {
            //float x = roomIdx * _roomSpacing;
            //_playerIconImage.anchoredPosition = new Vector2(x, _playerIconImage.anchoredPosition.y);
            _playerIconImage.anchoredPosition = Vector2.zero;
        }

        
        /// <summary>
        /// 진행상황 연출
        /// </summary>
        public IEnumerator PlayerProgressAnimation()
        {
            // 1. 던전 스프라이트 이동
            float startX = -(_nowRoomIndex * _roomSpacing) - (_roomSpacing / 2f);
            float endX = -(_nextRoomIndex * _roomSpacing) - (_roomSpacing / 2f);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / _stageMoveDuration;
                float x = Mathf.Lerp(startX, endX, t);
                _stagePathImage.anchoredPosition = new Vector2(x, _stagePathImage.anchoredPosition.y);
                yield return null;
            }
            _stagePathImage.anchoredPosition = new Vector2(endX, _stagePathImage.anchoredPosition.y);


            // 2. 플레이어 아이콘 이동
            //float iconStartX = _nowRoomIndex * _roomSpacing;
            //float iconEndX = _nextRoomIndex * _roomSpacing;
            //t = 0f;

            //while (t < 1f)
            //{
            //    t += Time.unscaledDeltaTime / _iconMoveDuration;
            //    float x = Mathf.Lerp(iconStartX, iconEndX, t);
            //    _playerIconImage.anchoredPosition = new Vector2(x, _playerIconImage.anchoredPosition.y);
            //    yield return null;
            //}
            //_playerIconImage.anchoredPosition = new Vector2(iconEndX, _playerIconImage.anchoredPosition.y);
            yield return null;
        }
    }
}


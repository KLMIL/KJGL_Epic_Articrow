using TMPro;
using UnityEngine;
using YSJ;

namespace Game.Test
{
    public class MapTestStart : MonoBehaviour
    {
        public static MapTestStart Instance { get; private set; }


        [Header("SO")]
        public MapTestDataSO dataSO;

        [Header("Review Canvas")]
        public GameObject reviewUI;
        public TextMeshProUGUI infoText;
        public TextMeshProUGUI diffText;
        public TextMeshProUGUI propText;
        public TextMeshProUGUI commText;

        [Header("Select Canvas")]
        public GameObject selectMapUI;
        public TextMeshProUGUI currDiffText;




        #region LifeCycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        bool isFirstMap = true;
        bool isReview = false;
        private void Update()
        {
            if (!isReview)
            {
                if (!isFirstMap)
                {
                    infoText.text = dataSO.lastSceneName;
                    diffText.text = "";
                    propText.text = "";
                    commText.text = "";

                    reviewUI.SetActive(true);
                    isReview = true;
                }
                else
                {
                    isFirstMap = false;
                }
                currDiffText.text = dataSO.lastSelectedDifficulty;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                selectMapUI.SetActive(!selectMapUI.activeSelf);
            }
        }
        #endregion



        #region Review
        int diff = -1;
        int prop = -1;
        string comm = "";

        public void OnDiffButtonClicked(int index)
        {
            diff = index;
            switch (diff)
            {
                case 0: diffText.text = "매우 쉬움"; break;
                case 1: diffText.text = "쉬움"; break;
                case 2: diffText.text = "보통"; break;
                case 3: diffText.text = "어려움"; break;
                case 4: diffText.text = "매우 어려움"; break;
            }
        }

        public void OnPropButtonClicked(int index)
        {
            prop = index;
            switch (prop)
            {
                case 0: propText.text = "매우 부적절"; break;
                case 1: propText.text = "부적절"; break;
                case 2: propText.text = "애매함"; break;
                case 3: propText.text = "적당함"; break;
                case 4: propText.text = "매우 적당함"; break;
            }
        }

        public void OnSubmitButtonClicked()
        {
            if (diff == -1 || prop == -1) return;

            comm = commText.text;
            dataSO.AddData(infoText.text, diff, prop, comm);
            reviewUI.SetActive(false);

            diff = -1;
            prop = -1;
            comm = "";
        }
        #endregion



        #region MapSelect
        string currDiff = "Easy";
        public void OnDifficultyButtonClicked(int index)
        {
            switch (index)
            {
                case 0: currDiff = "Easy";  break;
                case 1: currDiff = "Normal"; break;
                case 2: currDiff = "Hard"; break;
            }
            currDiffText.text = currDiff;
        }

        public void OnRandomRoomButtonClicked()
        {
            OnRoomButtonClicked(Random.Range(1, 71));
        }

        public void OnRoomButtonClicked(int index)
        {
            // 맵 이름 만들기
            string sceneName = currDiff;
            sceneName += "-01"; // Add Stage Info

            int room = ((index - 1) / 5) + 1;
            int idx = ((index - 1) % 5) + 1;

            sceneName += "-";
            sceneName += room.ToString("D2");
            sceneName += "-";
            sceneName += idx.ToString("D2");

            // SO에 정보 기록
            dataSO.lastSceneName = sceneName;
            dataSO.lastSelectedDifficulty = currDiff;

            selectMapUI.SetActive(false);

            // TODO: 선택된 맵에 맞는 씬 로드
            StartCoroutine(Managers.Scene.LoadSceneCoroutine(sceneName));
            isReview = false;
        }
        #endregion
    }
}

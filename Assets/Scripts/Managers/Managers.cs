using UnityEngine;

namespace YSJ
{
    public class Managers : MonoBehaviour
    {
        static Managers s_instance;
        public static Managers Instance { get { Init(); return s_instance; } }

        SoundManager _sound = new SoundManager();
        ResourceManager _resource = new ResourceManager();
        SceneManagerEx _scene = new SceneManagerEx();
        InputManager _input = new InputManager();
        UIManager _ui = new UIManager();
        DataManager _data = new DataManager();
        PoolManager _pool = new PoolManager();

        public static SoundManager Sound => Instance._sound;
        public static ResourceManager Resource => Instance._resource;
        public static SceneManagerEx Scene => Instance._scene;
        public static InputManager Input => Instance._input;
        public static UIManager UI => Instance._ui;
        public static DataManager Data => Instance._data;
        public static PoolManager Pool => Instance._pool;

        void Start()
        {
            Init();

            Cursor.lockState = CursorLockMode.Confined;
        }

        // 매니저 인스턴스 없어도 생성
        static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();


                // 필요한 매니저 초기화
                Data.Init();
                Resource.Init();
                Pool.Init();
                Sound.Init();
                Input.Init();
                UI.InstantiateSettingsUI();
            }
        }

        // 게임 종료 시 매니저 정리
        public void Clear()
        {
            Debug.LogWarning("Managers 파괴");

            UI_CommonEventBus.Clear();
            UI_TitleEventBus.Clear();
            UI_InGameEventBus.Clear();

            _pool.Clear();
            _scene.Clear();
            _sound.Clear();
            _input.Clear();

            s_instance = null;
        }

        void OnDisable()
        {
            Clear();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    [CreateAssetMenu(menuName = "TEST/MapTestDataSO")]
    public class MapTestDataSO : ScriptableObject
    {
        public List<MapTestResult> results = new();
        public string lastSelectedDifficulty = "Easy";
        public string lastSceneName = "Easy-01-01-01";

        public void AddData(string sceneName, int diffScore, int properScore, string comment)
        {
            var result = results.Find(r => r.sceneName == sceneName);
            if (result == null)
            {
                result = new MapTestResult { sceneName = sceneName };
                results.Add(result);
            }

            if (0 <= diffScore && diffScore <= 4)
            {
                result.difficultyScores[diffScore]++;
            }
            if (0 <= properScore &&  properScore <= 4)
            {
                result.properScores[properScore]++;
            }
            if (!string.IsNullOrWhiteSpace(comment))
            {
                result.subjectiveNotes.Add(comment);
            }
        }


        // 데이터 정의를 위한 이너클래스
        [System.Serializable]
        public class MapTestResult
        {
            public string sceneName;                        // Ease-01-01-01
            public int[] difficultyScores = new int[5];     // 0(bad) ~ 4(good)
            public int[] properScores = new int[5];         // 0(bad) ~ 4(good)
            public List<string> subjectiveNotes = new();
        }
    }
}

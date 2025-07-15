using System;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsClass : MonoBehaviour
{
    /* 방에서 플레이어에게 피해를 준 공격자의 정보 저장 (None이 공격자면 이탈) */
    // class<List<string>> 방이름< 때린애들 >

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            List<Part> equipedParts = new List<Part>
            {
                new Part { name = "Part1", count = 1 },
                new Part { name = "Part2", count = 2 }
            };

            SaveEquipParts(equipedParts);
        }
        else if(Input.GetKeyDown(KeyCode.U))
        {
            SendEquipPartsData();
        }
    }


    /* 방 전환 시, 19개 파츠 중 장착한 파츠 정보 저장 */
    AllEquipParts _allEquipParts = new AllEquipParts(); // 방 이름과 장착한 파츠 정보 리스트

    [Serializable]
    public class AllEquipParts
    {
        public List<EquipParts> equipPartsList = new List<EquipParts>(); // 방 이름과 장착한 파츠 정보 리스트
    }

    [Serializable]
    public class EquipParts
    {
        public string sceneName; // 방 이름
        public List<Part> equipedParts = new List<Part>(); // 각 씬에서 장착한 파츠 정보
    }

    [Serializable]
    public class Part
    {
        public string name; // 파츠 이름
        public int count; // 파츠 번호
    }

    // 각 방을 나갈 때마다 장착한 파츠 정보 저장
    // Artifact에서 호출해줘야 함
    public void SaveEquipParts(List<Part> equipedParts)
    {
        EquipParts equipParts = new EquipParts
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            equipedParts = equipedParts
        };
        _allEquipParts.equipPartsList.Add(equipParts);
    }

    // 데이터 전송
    public void SendEquipPartsData()
    {
        string json = JsonUtility.ToJson(_allEquipParts);
        Debug.Log(json);

        // Analytics에 보내는 부분 작성할 예정
    }
}
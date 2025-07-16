using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsClass
{
    /* 방에서 플레이어에게 피해를 준 공격자의 정보 저장 (None이 공격자면 이탈) */
    // class<List<string>> 방이름< 때린애들 >

    #region 방 전환 시, 19개 파츠 중 장착한 파츠 정보 저장
    // 방 이름과 장착한 파츠 정보 리스트 클래스
    [Serializable]
    public class AllEquipParts
    {
        public List<EquipParts> equipPartsList = new List<EquipParts>(); // 방 이름과 장착한 파츠 정보 리스트

        public AllEquipParts()
        {
            equipPartsList = new List<EquipParts>();
        }
    }

    // 해당 방에서의 파츠 정보 클래스
    [Serializable]
    public class EquipParts
    {
        public string sceneName;    // 방 이름
        public int roomIdx;         // 방 인덱스 (0부터 시작)
        public List<Part> equipedParts = new List<Part>(); // 각 씬에서 장착한 파츠 정보

        public EquipParts()
        {
            sceneName = string.Empty;
            roomIdx = -1;
            equipedParts = new List<Part>();
        }
    }

    // 파츠 정보 클래스
    [Serializable]
    public class Part
    {
        public string name; // 파츠 이름
        public int count;   // 파츠 개수
    }
    #endregion
}
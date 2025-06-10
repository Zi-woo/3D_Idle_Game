using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 데이터
[CreateAssetMenu(menuName = "Data/PlayerData")]
public class PlayerData : CharacterData
{
    public int experience;
    public int gold;
    public int currentStage;
    //public List<SkillData> skills;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : CharacterData
{
    public int rewardExperience;
    public int rewardGold;

    public ItemData dropItem;
    //public List<SkillData> skills;
}

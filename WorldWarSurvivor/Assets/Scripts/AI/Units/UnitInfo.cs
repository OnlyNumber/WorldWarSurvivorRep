using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitInfo", menuName = "EnemyUnits/UnitInfo")]
public class UnitInfo : ScriptableObject
{
    //public 
    public Vector2Int HumanHealthRange;
    public Vector2Int MeleeSkillRange;
    public Vector2Int RangeSkillRange;
    [SerializeField] private List<GameObject> modelsForCreation;


    public InventoryItemInfo Weapon;
    public InventoryItemInfo Helmet;
    public InventoryItemInfo BodyArmour;

    public HumanStats GenerateHuman()
    {

        HumanStats humanStats = new();
        humanStats.CurrentLevel = 0;
        humanStats.CurrentAmountOfExperience = 0;

        humanStats.MaxHealth = Random.Range(HumanHealthRange.x, HumanHealthRange.y);
        humanStats.CurrentHealth = humanStats.MaxHealth;

        humanStats.MeleeSkill = Random.Range(MeleeSkillRange.x, MeleeSkillRange.y);
        humanStats.RangeSkill = Random.Range(RangeSkillRange.x, RangeSkillRange.y);

        humanStats.ModelPrefab = null;//modelsForCreation[Random.Range(0, modelsForCreation.Count)];

        var temp = new InventoryItemInfo();
        temp.CopyInfo(Weapon);
        humanStats.HumanInventoryInfo.EquipmentInfo.MainHandItem = temp;

        temp = new InventoryItemInfo();
        temp.CopyInfo(Helmet);
        humanStats.HumanInventoryInfo.EquipmentInfo.HeadItem = temp;

        temp = new InventoryItemInfo();
        temp.CopyInfo(BodyArmour);
        humanStats.HumanInventoryInfo.EquipmentInfo.BodyItem = temp;

        return humanStats;
    }
}

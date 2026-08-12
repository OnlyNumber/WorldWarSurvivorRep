using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GunShootAction : AbilityAction
{
    private const float Cover_Angle = 175f;

    private int Damage;

    private int AttackRange;

    private int AttackEnergyCost;

    private Human CurrentHuman;

    public GunShootAction(ActionSO actionSO) : base(actionSO)
    {
        var gunShootSO = actionSO as ShootActionSO;

        Damage = gunShootSO.Damage;
        AttackRange = gunShootSO.AttackRange;
        AttackEnergyCost = gunShootSO.AttackEnergyCost;
    }

    public override void Initialize(ActingObject actingObject, BoardGrid myGrid)
    {
        CurrentActingObject = actingObject;
        this.myGrid = myGrid;

        CurrentHuman = CurrentActingObject as Human;
        CurrentHuman.AddAnimationAction(Animations.Attack, 0.9f, EndAttack);
    }

    public override void Dispose()
    {
        CurrentHuman.RemoveAnimationAction(Animations.Attack, 0.9f, EndAttack);
    }

    public override HashSet<BoardCell> GetAccessibleCells(Vector2Int CellPosition)
    {
        HashSet<BoardCell> targets = new();
        FogOfWar.FindVisibleCellsFromPosition(myGrid, CellPosition, out var cells, AttackRange);

        foreach (var cell in cells)
        {
            if (cell.gridObject != null && cell.gridObject is Human)
            {
                targets.Add(cell);
            }
        }

        targets.Remove(CurrentHuman.MyCurrentCell);

        return targets;
    }

    public override HashSet<BoardCell> GetAccessibleCells()
    {
        return GetAccessibleCells(CurrentHuman.MyCurrentCell.Coordinate);
    }

    public override void TargetCell(BoardCell attackingCell)
    {
        CurrentHuman.ChangeEnergy(-AttackEnergyCost);

        TurnController.AddMovingObject(CurrentHuman);
        CurrentHuman.SetCurrentAnimation(Animations.Attack);

        CurrentHuman.transform.LookAt(attackingCell.transform);


        if (attackingCell.gridObject != null)
        {
            if (CalculateChanceOfHit((Human)CurrentActingObject, (Human)attackingCell.gridObject))
            {
                attackingCell.gridObject.HealthSystem.ChangeHealth(-Damage);
            }
        }

        //EndAttack();
    }

    private bool CalculateChanceOfHit(Human attacker, Human defender)
    {
        int randomValue = Random.Range(0, 101);

        int AttackerSkillValue = (int)(((float)attacker.MyHumanStats.RangeSkill + (float)attacker.MyHumanStats.GetRangeAttackEffects()) * attacker.MyHumanStats.GetRangeAttackPercentEffects());
        int DefenderSkillValue = (int)(((float)attacker.MyHumanStats.GetRangeDefendEffects()) * attacker.MyHumanStats.GetRangeDefendPercentEffects());
        DefenderSkillValue += CoverBonus(attacker, defender);

        Debug.Log("Value " + randomValue + " Skill value " + (AttackerSkillValue - DefenderSkillValue));

        return randomValue < (AttackerSkillValue - DefenderSkillValue);
    }

    private int CoverBonus(Human attacker, Human defender)
    {
        int currentBonus = 0;

        List<BoardCell> myCover = new();

        for (int y = -1; y < 1; y += 2)
        {
            for (int x = -1; x < 1; x += 2)
            {
                var cell = myGrid.GetCell(defender.MyCurrentCell.Coordinate + new Vector2Int(x, y));

                if (cell != null && cell.gridObject != null && (cell.gridObject.coverType == CoverType.HalfCover || cell.gridObject.coverType == CoverType.FullCover))
                    myCover.Add(cell);
            }
        }

        foreach (var item in myCover)
        {
            if (CanProtectFrom(attacker.CenterPosition, defender.CenterPosition, item.transform.position))
            {
                if (item.gridObject.coverType == CoverType.FullCover)
                {
                    currentBonus = 40;
                    break;
                }
                else
                    currentBonus = 20;
            }
        }


        return currentBonus;
    }

    public bool CanProtectFrom(Vector3 attackerPosition, Vector3 protectedPosition, Vector3 coverDirection)
    {
        Vector3 directionToAttacker = attackerPosition - protectedPosition;
        directionToAttacker.Normalize();

        float dot = Vector3.Dot(coverDirection, directionToAttacker);
        float requiredDot = Mathf.Cos(Cover_Angle * 0.5f * Mathf.Deg2Rad);

        return dot >= requiredDot;
    }


    public override bool IsActionAccessible()
    {
        return CurrentHuman.CurrentEnergy > AttackEnergyCost;
    }

    private void EndAttack()
    {
        Debug.Log("End attack");

        TurnController.RemoveMovingObject(CurrentHuman);
        CurrentHuman.SetCurrentAnimation(Animations.Idle);
        CurrentHuman.StartCoroutine(Utilities.WaitAndRun(() => CurrentHuman.SetCurrentAnimation(Animations.Idle), 0.2f));

        CurrentHuman.SetCurrentAnimation(Animations.Idle);
    }

    public float GetAttackRange()
    {
        return AttackRange;
    }
}
using SrpgFramework.Units.Skills;
using SrpgFramework.Ai.Evaluators;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.Units.Units
{
    public class AiUnit : MonoBehaviour
    {
        private Unit unit;
        public Dictionary<Unit, float> UnitScoreDict { get; private set; } = new();
        public Dictionary<Cell, float> CellScoreDict { get; private set; } = new();

        public HashSet<Skill> MoveBrains { get; private set; } = new();
        public HashSet<Skill> ActionBrains { get; private set; } = new();

        public List<Evaluator<Unit>> UnitEvaluators = new();
        public List<Evaluator<Cell>> CellEvaluators = new();

        private void Awake()
        {
            unit = GetComponent<Unit>();
            MoveBrains.Add(new MoveSkill());
            ActionBrains.Add(new AttackSkill());
        }

        private void Start()
        {
            UnitEvaluators.Add(Resources.Load<Evaluator<Unit>>(Evaluator<Unit>.ResourcesPath + "Hate"));
            UnitEvaluators.Add(Resources.Load<Evaluator<Unit>>(Evaluator<Unit>.ResourcesPath + "LowHp"));

            CellEvaluators.Add(Resources.Load<Evaluator<Cell>>(Evaluator<Cell>.ResourcesPath + "Action"));
            CellEvaluators.Add(Resources.Load<Evaluator<Cell>>(Evaluator<Cell>.ResourcesPath + "Distance"));
            CellEvaluators.Add(Resources.Load<Evaluator<Cell>>(Evaluator<Cell>.ResourcesPath + "Escape"));
        }

        //ÆÀ¹À
        public void EvaluateUnits()
        {
            UnitScoreDict.Clear();

            foreach (var u in BattleManager.UnitMgr.Units)
            {
                EvaluateUnit(u);
            }
        }

        public void EvaluateCells()
        {
            CellScoreDict.Clear();
            foreach(var c in BattleManager.CellGridMgr.Cells.Values)
            {
                EvaluateCell(c);
            }
        }

        public void EvaluateUnit(Unit toEvaluate)
        {
            if (UnitEvaluators.Any())
            {
                UnitScoreDict.Add(toEvaluate, UnitEvaluators.Sum(evaluator =>
                {
                    evaluator.PreCalculate(unit);
                    return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
                }));
            }
            else
            {
                UnitScoreDict.Add(toEvaluate, 0);
            }
        }

        public void EvaluateCell(Cell toEvaluate)
        {
            if (CellEvaluators.Any())
            {
                CellScoreDict.Add(toEvaluate, CellEvaluators.Sum(evaluator =>
                {
                    evaluator.PreCalculate(unit);
                    return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
                }));
            }
            else
            {
                CellScoreDict.Add(toEvaluate, 0);
            }
        }

        //ÊµÐÐ
        public IEnumerator Execute()
        {
            EvaluateUnits();
            EvaluateCells();
            var brainList = MoveBrains.Concat(ActionBrains).ToHashSet();
            var brains = brainList.Where(brain => brain.ShouldExecute(unit, unit.Cell));

            while (brains.Any())
            {
                var topBrain = brains.OrderByDescending(brain => brain.Evaluate(unit)).First();
                yield return topBrain.AIExecute(unit);
                brains = brainList.Where(brain => brain.ShouldExecute(unit, unit.Cell));
            }
            yield break;
        }
    }
}
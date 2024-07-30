using SrpgFramework.Units.Abilities;
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

        public HashSet<Ability> MoveBrains { get; private set; } = new();
        public HashSet<Ability> ActionBrains { get; private set; } = new();

        public List<Evaluator<Unit>> UnitEvaluators = new();
        public List<Evaluator<Cell>> CellEvaluators = new();

        private void Awake()
        {
            unit = GetComponent<Unit>();
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
                if (UnitEvaluators.Any())
                {
                    UnitScoreDict.Add(u, UnitEvaluators.Sum(evaluator =>
                    {
                        evaluator.PreCalculate(unit);
                        return evaluator.Evaluate(u, unit) * evaluator.Weight;
                    }));
                }
                else
                {
                    UnitScoreDict.Add(u, 0);
                }
            }
        }

        public void EvaluateCells()
        {
            CellEvaluators.Clear();
            foreach(var c in BattleManager.CellGridMgr.Cells)
            {
                if (CellEvaluators.Any())
                {
                    CellScoreDict.Add(c.Value, CellEvaluators.Sum(evaluator =>
                    {
                        evaluator.PreCalculate(unit);
                        return evaluator.Evaluate(c.Value, unit) * evaluator.Weight;
                    }));
                }
                else
                {
                    CellScoreDict.Add(c.Value, 0);
                }
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
                var topBrain = brains.OrderByDescending(brain => brain.Evaluate(unit, unit.Cell)).First();
                yield return topBrain.AIExecute(unit);
                brains = brainList.Where(brain => brain.ShouldExecute(unit, unit.Cell));
            }
            yield break;
        }
    }
}
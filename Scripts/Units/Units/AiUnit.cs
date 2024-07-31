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

        //评估
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
            var score = !UnitEvaluators.Any() ? 0 : UnitEvaluators.Sum(evaluator =>
            {
                evaluator.PreCalculate(unit);
                return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
            });

            if (UnitScoreDict.ContainsKey(toEvaluate))
            {
                UnitScoreDict[toEvaluate] = score;
            }
            else
            {
                UnitScoreDict.Add(toEvaluate, score);
            }
        }

        public void EvaluateCell(Cell toEvaluate)
        {
            var score = !CellEvaluators.Any() ? 0 : CellEvaluators.Sum(evaluator =>
            {
                evaluator.PreCalculate(unit);
                return evaluator.Evaluate(toEvaluate, unit) * evaluator.Weight;
            });
            if(CellScoreDict.ContainsKey(toEvaluate))
            {
                CellScoreDict[toEvaluate] = score;
            }
            else
            {
                CellScoreDict.Add(toEvaluate, score);
            }
        }

        /// <summary>
        /// 评估周围一圈Cell 默认为单位可移动范围
        /// </summary>
        public void EvaluateNeighborCells()
        {
            var cells = unit.Cell.GetNeighborCells(unit.Mov);
            foreach (var cell in cells)
            {
                EvaluateCell(cell);
            }
        }

        //实行
        public IEnumerator Execute()
        {
            EvaluateUnits();
            EvaluateCells();
            var brainList = MoveBrains.Concat(ActionBrains);
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
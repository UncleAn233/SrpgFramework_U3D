using SrpgFramework.Ai.Evaluators;
using SrpgFramework.CellGrid.AStar;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units.Units;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.Players.AI.Evaluators
{
    //抵达格子花费回合数越高评分越低
    [CreateAssetMenu(fileName = "Distance", menuName = "AI/Evaluator/Cell/Distamce")]
    public class DistanceCellEvaluator : Evaluator<Cell>
    {
        public override float Evaluate(Cell cellToEvaluate, Unit evaluatingUnit)
        {
            if (cellToEvaluate.Equals(evaluatingUnit.Cell))
            {
                return 0;
            }

            var path = AStar.FindPath(evaluatingUnit.Cell, cellToEvaluate, evaluatingUnit.Move);
            if(path is not null)
            {
                var pathCost = path.Sum(c => c.MoveCost);
                var turnsToGetThere = Mathf.Ceil(pathCost / evaluatingUnit.Mov);
                return 1 - turnsToGetThere;
            }

            return -999;
        }
    }
}
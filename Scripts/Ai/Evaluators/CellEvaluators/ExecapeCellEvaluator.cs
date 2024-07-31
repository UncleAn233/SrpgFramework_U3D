using SrpgFramework.Ai.Evaluators;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.Players.AI.Evaluators
{
    //��Ѫ�����ж�������빥����Χ
    [CreateAssetMenu(fileName = "Escape", menuName = "AI/Evaluator/Cell/Escape")]
    public class EscapeCellEvaluator : Evaluator<Cell>
    {
        [Range(0, 1)]
        public float DangerLine = 0.2f;    //���ڸ�Ѫ����ΪΣ��ֵ

        public override float Evaluate(Cell toEvaluate, Unit unit)
        {
            if (unit.HpPercent < DangerLine || unit.ActionPoints == 0)  //ֻ��Ѫ������Σ��ֵ���Ѿ�û���ж�����ʱ�ӷ�
            {
                if (toEvaluate.GetDistance(unit.Cell) > unit.Mov)
                {
                    return 0;   //������Ŀǰ�޷��ִ�ĸ���ΪĿ��
                }

                var score = BattleManager.UnitMgr.GetEnemyUnits(unit.Player).Min(u => u.Cell.GetDistance(toEvaluate) + notOnLine(u.Cell, toEvaluate));
                return score;
            }

            return 0;
        }

        private float notOnLine(Cell c1, Cell c2)
        {
            var onLine = c1.Coord.x == c2.Coord.x || c1.Coord.y == c2.Coord.y;
            return onLine ? 0f : 0.1f;
        }
    }
}
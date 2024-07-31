using System;

namespace SrpgFramework.Units.Units
{
    public partial class Unit   //单位事件
    {
        public Action<int> OnTurnStart;
        public Action<int> OnTurnEnd;
        public void TurnStart(int turn)
        {
            OnTurnStart?.Invoke(turn);
        }
        public void TurnEnd(int turn)
        {
            OnTurnEnd?.Invoke(turn);
        }

        public Action OnDie;

        public Action BeforeDamaged;
        public Action AfterDamaged;
    }
}
using System;

namespace SrpgFramework.Units.Units
{
    public partial class Unit   //单位事件
    {
        public Action<int> OnTurnStart;
        public Action<int> OnTurnEnd;

        public Action OnDie;

        public Action BeforeDamaged;
        public Action AfterDamaged;
    }
}
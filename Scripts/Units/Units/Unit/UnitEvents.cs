using System;

namespace SrpgFramework.Units.Units
{
    public partial class Unit   //��λ�¼�
    {
        public Action<int> OnTurnStart;
        public Action<int> OnTurnEnd;

        public Action OnDie;

        public Action BeforeDamaged;
        public Action AfterDamaged;
    }
}
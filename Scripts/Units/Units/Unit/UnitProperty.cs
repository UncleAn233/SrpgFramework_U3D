
namespace SrpgFramework.Units.Units
{
    public partial class Unit
    {
        public int Lv { get; set; }    //�ȼ�
        public int Exp { get; set; }     //����

        public int MaxHp { get; set; }
        private int hp = 10;    //Ѫ��
        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                if (hp <= 0)
                    Die();
            }
        }

        public int Atk { get; set; } = 1;   //����
        public int Def { get; set; } = 0;    //����
        public int Mdef { get; set; } = 0;   //ħ��
        public int Dodge { get; set; } = 0;   //����
        public int Mov { get; set; } = 3;   //�ƶ�
        public int Hate { get; set; } = 1; //���
        
        public float HpPercent { get => (float)hp / MaxHp; }

        private void initAttribute(UnitData data)
        {
            MaxHp = data.Attribute.Hp;
            hp = data.Attribute.Hp;
            Atk = data.Attribute.Atk;
            Def = data.Attribute.Def;
            Mdef = data.Attribute.Mdef;
            Dodge = data.Attribute.Dodge;
            Mov = data.Attribute.Mov;
            Hate = data.Attribute.Hate;
        }
    }
}
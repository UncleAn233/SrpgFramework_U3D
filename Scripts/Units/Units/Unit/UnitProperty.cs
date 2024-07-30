
namespace SrpgFramework.Units.Units
{
    public partial class Unit
    {
        public int Lv { get; set; }    //µÈ¼¶
        public int Exp { get; set; }     //¾­Ñé

        public int MaxHp { get; set; }
        private int hp = 10;    //ÑªÌõ
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

        public int Atk { get; set; } = 1;   //¹¥»÷
        public int Def { get; set; } = 0;    //·ÀÓù
        public int Mdef { get; set; } = 0;   //Ä§¿¹
        public int Dodge { get; set; } = 0;   //ÉÁ±Ü
        public int Mov { get; set; } = 3;   //ÒÆ¶¯
        public int Hate { get; set; } = 1; //³ðºÞ
        
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
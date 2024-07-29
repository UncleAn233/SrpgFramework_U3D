using SrpgFramework.Global;
using SrpgFramework.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units
{
    public partial class Unit
    {
        public HashSet<string> Tags;
        public UnitType UnitType { get; private set; }
        public int PlayerNumber;
        public Player Player => GameManager.PlayerMgr.GetPlayer(PlayerNumber);

        public int Lv { get; set; }    //�ȼ�
        public int Exp { get; set; }     //����

        public int MaxHp { get; set; }
        private int hp;    //Ѫ��
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                this.ApplyEvent(nameof(Hp));
                if (hp < 1)
                    this.ApplyEvent("Die");
            }
        }

        public int Atk { get; set; }   //����
        public int Def { get; set; }    //����
        public int Mdef { get; set; }   //ħ��
        public int Dodge { get; set; }   //����
        public int Mov { get; set; } = 3;   //�ƶ�
        public int Hate { get; set; } //���
        
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
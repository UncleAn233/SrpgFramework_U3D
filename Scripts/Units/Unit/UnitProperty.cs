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

        public int Lv { get; set; }    //µÈ¼¶
        public int Exp { get; set; }     //¾­Ñé

        public int MaxHp { get; set; }
        private int hp;    //ÑªÌõ
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

        public int Atk { get; set; }   //¹¥»÷
        public int Def { get; set; }    //·ÀÓù
        public int Mdef { get; set; }   //Ä§¿¹
        public int Dodge { get; set; }   //ÉÁ±Ü
        public int Mov { get; set; } = 3;   //ÒÆ¶¯
        public int Hate { get; set; } //³ðºÞ
        
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
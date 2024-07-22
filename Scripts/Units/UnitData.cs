using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SrpgFramework.Abilities;
using System;
using System.Collections.Generic;

namespace SrpgFramework.Units
{
    [JsonConverter(typeof(UnitDataConverter))]
    public class UnitData
    {
        //基础信息
        public string Name;
        public string Spine;

        public UnitDataAttribute Attribute; //属性数值
        public List<Ability> Ability;   //能力相关
    }

    public class UnitDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(UnitData).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            UnitData result = new();
            var jo = JObject.Load(reader);

            result.Name = jo["Name"].ToString();
            result.Spine = jo["Spine"].ToString();

            result.Attribute = JsonConvert.DeserializeObject<UnitDataAttribute>(jo["Attribute"].ToString());

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitDataAttribute
    {
        public int Exp;     //经验
        public int Lv;    //等级

        public int Hp;    //血条
        public int Atk;   //攻击
        public int Def;    //防御
        public int Mdef;   //魔抗
        public int Dodge;   //闪避
        public int Mov;    //移动
        public int Hate; //仇恨
    }

    public class UnitDataAnimation
    {
        public float AttackInterval;
        public string MoveAnimation;
    }
}

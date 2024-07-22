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
        //������Ϣ
        public string Name;
        public string Spine;

        public UnitDataAttribute Attribute; //������ֵ
        public List<Ability> Ability;   //�������
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
        public int Exp;     //����
        public int Lv;    //�ȼ�

        public int Hp;    //Ѫ��
        public int Atk;   //����
        public int Def;    //����
        public int Mdef;   //ħ��
        public int Dodge;   //����
        public int Mov;    //�ƶ�
        public int Hate; //���
    }

    public class UnitDataAnimation
    {
        public float AttackInterval;
        public string MoveAnimation;
    }
}

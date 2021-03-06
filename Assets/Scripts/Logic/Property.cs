using UnityEngine;

namespace DefaultNamespace
{
    public struct PropertyCollection
    {
        public float Money;
        public float Food;
        public float San;
        public float Day;
        public float ChanceOfQiangCai;
        public float ChanceOfTuanGou;
        public float ChanceOfGongsiKongTou;
        public float ChanceOfSick;

        public override string ToString()
        {
            return $"Money:{Money} Food:{Food} San:{San} Day:{Day}";
        }

        public void Add(in PropertyCollection other)
        {
            Money += other.Money;
            Food += other.Food;
            San += other.San;
            Day += other.Day;
            ChanceOfQiangCai += other.ChanceOfQiangCai;
            ChanceOfTuanGou += other.ChanceOfTuanGou;
            ChanceOfGongsiKongTou += other.ChanceOfGongsiKongTou;
            ChanceOfSick += other.ChanceOfSick;
        }

        public static PropertyCollection Parse(string columnString)
        {
            var inst = new PropertyCollection();
            if (string.IsNullOrEmpty(columnString)) return inst;

            var list = columnString.Split("；");

            foreach (var property in list)
            {
                if (string.IsNullOrEmpty(property)) continue;

                var idx = property.IndexOf('】');
                if (idx == -1)
                {
                    Debug.LogError("PropertyCollection Parse Error: " + columnString);
                    continue;
                }

                var propertyName = property[..(idx + 1)];
                var propertyValue = float.Parse(property[(idx + 1)..]);

                switch (propertyName)
                {
                    case ConstStr.家境:
                        inst.Money = propertyValue;
                        break;
                    case ConstStr.心情:
                        inst.San = propertyValue;
                        break;
                    case ConstStr.食物:
                        inst.Food = propertyValue;
                        break;
                    case ConstStr.公司空投:
                        inst.ChanceOfGongsiKongTou = propertyValue;
                        break;
                    case ConstStr.染病概率:
                        inst.ChanceOfSick = propertyValue;
                        break;
                    case ConstStr.团购概率:
                        inst.ChanceOfTuanGou = propertyValue;
                        break;
                    case ConstStr.抢菜概率:
                        inst.ChanceOfQiangCai = propertyValue;
                        break;
                    default:
                        Debug.LogError("奇怪的属性：" + columnString);
                        break;
                }
            }

            return inst;
        }
    }
}
using UnityEngine;

namespace DefaultNamespace
{
    public struct PropertyCollection
    {
        public Property Money;
        public Property Food;
        public Property San;
        public Property Day;
        public Property ChangeOfQiangCai;
        public Property ChangeOfTuanGou;
        public Property ChangeOfGongsiKongTou;
        public Property ChangeOfSick;

        public static PropertyCollection Parse(string columnString)
        {
            // TODO:
            return default;
        }
    }

    public struct Property
    {
        public readonly string Name;
        public int Value;

        public Property(int value, string name = null)
        {
            Value = value;
            Name = name;
        }

        public static Property operator +(Property a, int b)
        {
            a.Value += b;
            return a;
        }

        public static Property operator ++(Property a)
        {
            a.Value += 1;
            return a;
        }

        public override string ToString() => Value.ToString();
    }
}
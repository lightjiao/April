using UnityEngine;

namespace DefaultNamespace
{
    public class PropertyCollection
    {
        public Property LevelOfCommunity;
        public Property Food;
        public Property San;
        public Property Day;
        public Property ChangeOfQiangCai;
        public Property ChangeOfTuanGou;
        public Property ChangeOfGongsiKongTou;
        public Property ChangeOfSick;
    }

    public class Property
    {
        public string Name { get; private set; }
        public int Value { get; private set; }

        public Property(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public bool CalChangeOnce()
        {
            return Random.Range(0, 100) < Value;
        }
    }
}
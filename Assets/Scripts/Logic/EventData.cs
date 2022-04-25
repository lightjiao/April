using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public struct EventData : ICsvData
    {
        public int Id;
        public string Type;
        public int Weight;
        public Condition Condition;
        public string Content;
        public int LifeTime;
        public PropertyCollection AffectsProperties;
        public List<Branch> Branches;

        // 事件分支
        public bool ParseOneRaw(string dataRowString)
        {
            try
            {
                var columnStrings = dataRowString.Split(CsvParser.DataSplitSeparators);
                for (var i = 0; i < columnStrings.Length; i++)
                {
                    columnStrings[i] = columnStrings[i].Trim(CsvParser.DataTrimSeparators);
                }

                Id = int.Parse(columnStrings[0]);
                Type = columnStrings[2];

                if (columnStrings[3] == "")
                {
                    Debug.LogWarning("EventData: " + Id + " has no weight, default as 1");
                    Weight = 1;
                }
                else
                {
                    Weight = int.Parse(columnStrings[3]);
                }

                Condition = Condition.Parse(columnStrings[4]);
                Content = columnStrings[5];
                LifeTime = columnStrings[6] == "" ? 999 : int.Parse(columnStrings[6]);
                AffectsProperties = PropertyCollection.Parse(columnStrings[7]);
                Branches = new List<Branch>
                {
                    Branch.Parse(columnStrings[8], columnStrings[9]),
                    Branch.Parse(columnStrings[10], columnStrings[11])
                };

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e + dataRowString);
                return false;
            }
        }
    }
}
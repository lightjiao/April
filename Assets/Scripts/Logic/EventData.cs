﻿namespace DefaultNamespace
{
    public class EventData : ICsvData
    {
        public int Id;
        public string Content;

        // 影响数据
        public PropertyCollection AffectsProperties;

        // 事件分支
        public bool ParseOneRaw(string dataRowString)
        {
            var columnStrings = dataRowString.Split(CsvParser.DataSplitSeparators);
            for (var i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(CsvParser.DataTrimSeparators);
            }

            var index = 0;
            index++;
            Id = int.Parse(columnStrings[index++]);
            index++;
            Content = columnStrings[index++];

            return true;
        }
    }
}
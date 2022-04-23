namespace DefaultNamespace
{
    public class EventData : ICsvData
    {
        public int Id;
        public string Content;

        // 影响数据
        public int Satiety; // 饱腹感
        public int San;
        public int Money; // 家境
        public int Food;

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
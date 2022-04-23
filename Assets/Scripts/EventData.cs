namespace DefaultNamespace
{
    public class EventData
    {
        public int Id;
        public string Content;
        
        public BasicProperty AddProperty;
        
        // 条件
        //  属性达到XX -> 
        //  存在某个事件 ->

        public int[] ConditionEventIds; // 有条件才会被随机到，包括事件、属性
        public int[] ExcludeEvents;
    }
}
namespace DefaultNamespace
{
    public struct BasicProperty
    {
        // 属性
        public int Satiety; // 饱腹感
        public int San;
        public int Money;
        public int Food;
    }
    
    public class Player
    {
        public BasicProperty Property;
        
        // 食物有可能会坏掉一部分（事件）
        
        // 食物为0则会减少饱腹感，饱腹感不够则会减少健康度，健康度为0则死亡
        
        // 声明不够
    }
}
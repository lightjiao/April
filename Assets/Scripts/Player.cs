namespace DefaultNamespace
{
    public struct BasicProperty
    {
        // 属性
        public bool IsCovId19;
        public int Health;
        public int MaxHealth; // 长时间健康度不够会减少最大健康度
        public int Satiety; // 饱腹感
        public int Happy; // 快乐度
        public int Money;

        // 物资
        public int Food;
        public int Medicine;

        // 宠物
        public bool HasPet;
        public int PetFood;
        public int PetHealth;
        public int PetSatiety;
        public int PetMedicine;
    }
    
    public class Player
    {
        public BasicProperty Property;
        
        // 食物有可能会坏掉一部分（事件）
        
        // 食物为0则会减少饱腹感，饱腹感不够则会减少健康度，健康度为0则死亡
        
        // 声明不够
    }
}
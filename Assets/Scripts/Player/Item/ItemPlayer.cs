namespace Player.Item
{
    public class ItemPlayer
    {
        public float Health { get; set; }
        public int Amount { get; set; }

        public ItemPlayer()
        {
            Health = 0;
            Amount = 0;
        }
        
        public ItemPlayer(float health, int amount)
        {
            Health = health;
            Amount = amount;
        }
        
        
        
    }
}
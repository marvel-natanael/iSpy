namespace Player
{
    public struct ItemPlayer
    {
        public float Health { get; set; }
        public int Amount { get; set; }

        public ItemPlayer(float health, int amount)
        {
            Health = health;
            Amount = amount;
        }
        
    }
}
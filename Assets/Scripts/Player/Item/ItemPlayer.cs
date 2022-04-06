using Mirror;

namespace Player.Item
{
    public class ItemPlayer : NetworkBehaviour
    {
        [SyncVar]
        public float health;
        [SyncVar]
        public int amount;

        public ItemPlayer()
        {
            health = 0;
            amount = 0;
        }
        
        public ItemPlayer(float _health, int _amount)
        {
            health = _health;
            amount = _amount;
        }
    }
}
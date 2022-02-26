using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        public ItemPlayer ItemPlayer { get; set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        
    }
}
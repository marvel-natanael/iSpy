using System;
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
            
            ItemPlayer = new ItemPlayer
            {
                Health = 100,
                Amount = 5
            };
        }
    }
}
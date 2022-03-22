using System.Collections.Generic;
using UnityEngine;

namespace Player.Bullets
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance;

        private readonly List<GameObject> _listBulletPool = new List<GameObject>();

        private const int AmountPool = 20;

        [Header("Game Object")] [SerializeField]
        private GameObject bullet;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            for (var i = 0; i < AmountPool; i++)
            {
                var objBullet = Instantiate(bullet);
                objBullet.SetActive(false);
                _listBulletPool.Add(objBullet);
            }
        }
        
        /// <summary>
        ///   <para>Get Game Object Bullet Pool</para>
        /// </summary>
        public GameObject GetBullet()
        {
            foreach (var bulletPool in _listBulletPool)
            {
                if (!bulletPool.activeInHierarchy)
                {
                    return bulletPool;
                }
            }

            return null;
        }
    }
}
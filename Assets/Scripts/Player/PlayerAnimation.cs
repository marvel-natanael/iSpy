using System;
using System.Collections;
using Mirror;
using Player.Item;
using Player.Weapons;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : NetworkBehaviour
    {
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private PlayerShoot _playerShoot;

        private WeaponSwap weapon;

        private PlayerManager pm;

        private void Start()
        {
            pm = GetComponent<PlayerManager>();
            weapon = GetComponent<WeaponSwap>();
            _animator = GetComponent<Animator>();

            _playerMovement = GetComponent<PlayerMovement>();
            _playerShoot = GetComponent<PlayerShoot>();
        }

        private void OnEnable()
        {
            PlayerManager.onDeath += Death;
        }

        private void OnDisable()
        {
            PlayerManager.onDeath -= Death;
        }

        private void Update()
        {
            if (!hasAuthority) return;
            Animation();
        }

        [ClientCallback]
        private void Animation()
        {
            if (_playerMovement.GetInputMovement() != Vector2.zero && weapon.GetWeapon() != null)
            {
                WalkWithWeapon();
            }
            else if (_playerMovement.GetInputMovement() != Vector2.zero)
            {
                Walk();
            }
            else if (Input.GetKey(KeyCode.Q) || InGameUIManager.instance.PlayerUI.curHealth <= 0)
            {
                Death();
            }
            else if (weapon.GetWeapon() != null)
            {
                IdleWeapon();
            }
            else
            {
                Idle();
            }

            //Debug.Log("Health " + InGameUIManager.instance.PlayerUI.curHealth);

            /*if (pm.ItemPlayer.health <= 0)
            {
                Death();
            }*/
        }

        public void Death()
        {
            Debug.Log("Death");

            SetAnimation("Idle", false);
            SetAnimation("Idle Weapon", false);
            SetAnimation("Walk", false);
            SetAnimation("Walk Weapon", false);
            SetAnimation("Reload", false);
            SetAnimation("Death", true);
        }


        private void WalkWithWeapon()
        {
            SetAnimation("Idle", false);
            SetAnimation("Idle Weapon", false);
            SetAnimation("Walk", false);
            SetAnimation("Walk Weapon", true);
            SetAnimation("Reload", false);
            SetAnimation("Death", false);
        }

        private void Idle()
        {
            SetAnimation("Idle", true);
            SetAnimation("Idle Weapon", false);
            SetAnimation("Walk", false);
            SetAnimation("Walk Weapon", false);
            SetAnimation("Reload", false);
            SetAnimation("Death", false);
        }

        private void IdleWeapon()
        {
            SetAnimation("Idle", false);
            SetAnimation("Idle Weapon", true);
            SetAnimation("Walk", false);
            SetAnimation("Walk Weapon", false);
            SetAnimation("Reload", false);
            SetAnimation("Death", false);
        }

        private void Reload()
        {
            SetAnimation("Idle", false);
            SetAnimation("Idle Weapon", false);
            SetAnimation("Walk", false);
            SetAnimation("Walk Weapon", false);
            SetAnimation("Reload", true);
            SetAnimation("Death", false);
        }

        private void Walk()
        {
            SetAnimation("Idle", false);
            SetAnimation("Idle Weapon", false);
            SetAnimation("Walk", true);
            SetAnimation("Walk Weapon", false);
            SetAnimation("Reload", false);
            SetAnimation("Death", false);
        }

        private void SetAnimation(string animationName, bool isActive)
        {
            _animator.SetBool(animationName, isActive);
        }
    }
}
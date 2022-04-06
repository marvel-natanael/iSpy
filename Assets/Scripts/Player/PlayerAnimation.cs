using System.Collections;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : NetworkBehaviour
    {
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private PlayerShoot _playerShoot;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _playerMovement = GetComponent<PlayerMovement>();
            _playerShoot = GetComponent<PlayerShoot>();
        }

        private void Update()
        {
            Animation();
        }

        private void Animation()
        {
            if (_playerShoot.GetShoot() && _playerMovement.GetInputMovement() != Vector2.zero)
            {
                WalkWithWeapon();
            }
            else if (_playerMovement.GetInputMovement() != Vector2.zero)
            {
                Walk();
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Reload();
            }
            else if (_playerShoot.GetShoot())
            {
                IdleWeapon();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Death();
            }
            else
            {
                Idle();
            }
        }

        private void Death()
        {
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
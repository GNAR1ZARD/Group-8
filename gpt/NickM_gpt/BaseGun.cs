﻿using System.Collections.Generic;
using Assets.Scripts.People;
using Assets.Scripts.Player;
using Assets.Scripts.Utils;
using UnityEngine;


//Orginal Code (ChatGPT Code below)
/* namespace Assets.Scripts.Weapons
{
    public class BaseGun : MonoBehaviour
    {
        public AmmoType AmmunitionType;
        public GameObject Bullet;
        public bool IsMelee;
        public Transform Tip;
        public List<AudioClip> AttackSound;
        public float ShotDelay = .1f;

        private float _lastShot;
        private PlayerAmmo _ammo;
        private UnityEngine.Camera _viewCamera;

        void Start()
        {
            _ammo = FindObjectOfType<PlayerAmmo>();
            _viewCamera = GameObject.Find("HeadCamera").GetComponent<UnityEngine.Camera>();
        }

        public void Fire()
        {
            if (CanFire())
            {
                GetComponent<Animator>().SetTrigger("Fire");

                if (IsMelee)
                {
                    MeleeAttack();
                }
                else
                {
                    RangedAttack();
                }
            }
        }

        public void MeleeAttack()
        {
            _lastShot = Time.fixedTime;

            var ray = _viewCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && Vector3.Distance(transform.position, hit.point) < .5f)
            {
                if (hit.collider.tag == "Enemy")
                {
                    var hitContext = new HitContext
                    {
                        Damage = Random.Range(0, 2),
                        Direction = transform.forward,
                        Force = 1,
                        IsMelee = true                    
                    };
                    hit.collider.GetComponent<HealthBehavior>().TakeDamage(hitContext);
                }
            }
        }

        public void RangedAttack()
        {
            if (AttackSound.Count > 0)
            {
                AudioSource.PlayClipAtPoint(AttackSound.Random(), transform.position);
            }

            var bullet = Instantiate(Bullet);

            if (Tip != null)
            {
                var ray = _viewCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Vector3 dir;

                if (Physics.Raycast(ray, out hit))
                {
                    dir = (hit.point - Tip.transform.position).normalized;
                }
                else
                {
                    dir = _viewCamera.transform.forward;
                }

                bullet.transform.position = Tip.transform.position;
                bullet.transform.rotation = Quaternion.LookRotation(dir);
            }
            else
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
            }

            bullet.GetComponent<Bullet>().Damage = Random.Range(1, 5);

            _lastShot = Time.fixedTime;
            _ammo.RemoveAmmo(AmmoType.Solution, 1);
        }

        private bool CanFire()
        {
            var ammunitionIsNotEmpty = (AmmunitionType == AmmoType.Infinite || _ammo.HasAmmo(AmmunitionType, 1));
            var itHasBeenLongEnoughSinceTheLastShot = Time.fixedTime - _lastShot > ShotDelay;

            return ammunitionIsNotEmpty && itHasBeenLongEnoughSinceTheLastShot;
        }
    }
}

*/

namespace Assets.Scripts.Weapons
{
    public class BaseGun : MonoBehaviour
    {
        public AmmoType AmmunitionType;
        public GameObject Bullet;
        public bool IsMelee;
        public Transform Tip;
        public List<AudioClip> AttackSound;
        public float ShotDelay = 0.1f;

        private float _lastShot;
        private PlayerAmmo _ammo;
        private UnityEngine.Camera _viewCamera;

        private void Start()
        {
            _ammo = FindObjectOfType<PlayerAmmo>();
            _viewCamera = GameObject.Find("HeadCamera").GetComponent<UnityEngine.Camera>();
        }

        public void Fire()
        {
            if (CanFire())
            {
                GetComponent<Animator>().SetTrigger("Fire");

                if (IsMelee)
                    MeleeAttack();
                else
                    RangedAttack();
            }
        }

        private void MeleeAttack()
        {
            _lastShot = Time.fixedTime;

            var ray = _viewCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hit) && Vector3.Distance(transform.position, hit.point) < 0.5f)
            {
                if (hit.collider.CompareTag("Enemy"))
                    ApplyMeleeDamage(hit.collider);
            }
        }

        private void ApplyMeleeDamage(Collider enemyCollider)
        {
            var hitContext = new HitContext
            {
                Damage = Random.Range(0, 2),
                Direction = transform.forward,
                Force = 1,
                IsMelee = true
            };
            enemyCollider.GetComponent<HealthBehavior>()?.TakeDamage(hitContext);
        }

        private void RangedAttack()
        {
            PlayRandomAttackSound();

            var bullet = Instantiate(Bullet);
            SetBulletPositionAndRotation(bullet);

            bullet.GetComponent<Bullet>().Damage = Random.Range(1, 5);

            _lastShot = Time.fixedTime;
            _ammo.RemoveAmmo(AmmoType.Solution, 1);
        }

        private void PlayRandomAttackSound()
        {
            if (AttackSound.Count > 0)
                AudioSource.PlayClipAtPoint(AttackSound.Random(), transform.position);
        }

        private void SetBulletPositionAndRotation(GameObject bullet)
        {
            var bulletPosition = Tip != null ? Tip.position : transform.position;
            var bulletRotation = Tip != null ? Quaternion.LookRotation(GetBulletDirection()) : transform.rotation;

            bullet.transform.position = bulletPosition;
            bullet.transform.rotation = bulletRotation;
        }

        private Vector3 GetBulletDirection()
        {
            var ray = _viewCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            return Physics.Raycast(ray, out var hit) ? (hit.point - Tip.transform.position).normalized : _viewCamera.transform.forward;
        }

        private bool CanFire()
        {
            var ammunitionIsNotEmpty = AmmunitionType == AmmoType.Infinite || _ammo.HasAmmo(AmmunitionType, 1);
            var enoughTimeHasPassed = Time.fixedTime - _lastShot > ShotDelay;

            return ammunitionIsNotEmpty && enoughTimeHasPassed;
        }
    }
}

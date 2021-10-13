using System;
using System.Collections;
using Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units
{
    public class Utility : MonoBehaviour
    {
        protected UnitStat unit;

        protected GameObject target;
        
        public ParticleSystem muzzleFire;

        public string gunSound;
        
        private IEnumerator _attack;

        private AudioManager _manager;
        
        public Animator anim;
        
        private static readonly int Firing = Animator.StringToHash("firing");

        private void Start()
        {
            unit = gameObject.GetComponent<UnitStat>();
            _manager = FindObjectOfType<AudioManager>();
        }
        
        private IEnumerator AttackTimer(int hitPercentage, UnitStat targetStat)
        {
            while(true)
            {
                yield return new WaitForSeconds(1.25f);
                if (anim != null)
                {
                    _manager.Play(gunSound);
                }
                yield return new WaitForSeconds(1f); // wait 1.25 second
                unit.actions -= 2;
                unit.Exit();
                target = null;
                unit.acting = false;
                if (anim != null)
                {
                    anim.SetBool(Firing,false);
                }
                if (Random.Range(0, 100) <= hitPercentage)
                {
                    targetStat.healthPoints -= unit.damage;
                    targetStat.HealthUpdate();
                }
                StopCoroutine(_attack);
            }
        }
        
        private IEnumerator RotateTimer()
        {
            while(true)
            {
                yield return new WaitForSeconds(1f); // wait 1 second
                if (anim == null)
                {
                    muzzleFire.Play();
                    _manager.Play(gunSound);
                }
                else
                {
                    anim.SetBool(Firing, true);
                }
                unit.rotation = false;
                StartCoroutine(_attack);
                StopCoroutine("RotateTimer");
            }
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        public void Attack()
        {
            int hitPercentage = 0;
            UnitStat targetStat = target.GetComponent<UnitStat>();
            GridStat targetGridStat = targetStat.currentGrid.GetComponent<GridStat>();
            
            int coverHorizontal = targetGridStat.x - (int) unit.currentPosition.x;
            int coverVertical = targetGridStat.y - (int) unit.currentPosition.y;
            
            bool coverRight = (coverHorizontal > 0) && targetGridStat.coverRight;
            bool coverLeft = (coverHorizontal < 0) && targetGridStat.coverLeft;
            bool coverForward = (coverVertical > 0) && targetGridStat.coverForward;
            bool coverBack = (coverVertical < 0) && targetGridStat.coverBack;

            if (coverRight || coverLeft || coverForward || coverBack)
            {
                hitPercentage -= unit.halfCover;
            }

            int distance = Math.Abs(coverHorizontal) + Math.Abs(coverVertical);
    
            if (distance <= unit.optimalRange)
            {
                hitPercentage += 90;
            }
            else if (distance <= unit.maxAttackRange)
            {
                hitPercentage += 70;
            }
            else
            {
                unit.Exit();
                target = null;
                return;
            }

            Quaternion currentRotation = transform.rotation;
            transform.LookAt(target.transform);
            unit.targetRotation = transform.rotation;
            transform.rotation = currentRotation;
            unit.rotation = true;
            unit.playerController.cameraBehaviour.CameraSwitch(gameObject);
            _attack = AttackTimer(hitPercentage,targetStat);
            unit.acting = true;
            unit.Exit();
            //print("CurrentRotation: " + transform.rotation + " ,TargetRotation: " + Unit.targetRotation + " from " + gameObject.name);
            StartCoroutine("RotateTimer");
        }
    } 
}

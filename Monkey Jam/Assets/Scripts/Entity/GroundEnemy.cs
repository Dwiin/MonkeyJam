using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace MonkeyJam.Entities
{
    public class GroundEnemy : EnemyBase
    {
        int wayPointIndex;
        bool isChasing = false;
        float direction;
        Vector2 facingDirection;

        [SerializeField] LayerMask mask;
        private List<AttackData> _onCooldown;

        private void Start()
        {
            SetupStats(Data.Stats);
            _onCooldown = new List<AttackData>();

            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            wayPointIndex = 0;

        }

        private void Update()
        {
            if(waypoints.Length == 0)
            {
                return;
            }
            if (isPatroling) { MoveToWayPoint(waypoints[wayPointIndex]); }
            else { _animator.SetBool("isRunning", false); }

            RaycastCheck();
        }

        private void Movement(Transform currentPoint)
        {
            _animator.SetBool("isRunning", true);
            
            if (currentPoint.position.x - transform.position.x < 0)
            {
                direction = -1;
                facingDirection = Vector2.left;
                Quaternion newRot = Quaternion.identity;
                newRot.y = 180;
                transform.rotation = newRot;
            }
            else
            {
                direction = 1;
                facingDirection = Vector2.right;
                Quaternion newRot = Quaternion.identity;
                newRot.y = 0;
                transform.rotation = newRot;

            }

            Vector3 newPos = transform.position;
            newPos.x += direction * Time.deltaTime * _stats.MoveSpeed;
            transform.position = newPos;
        }

        private void MoveToWayPoint(Transform currentPoint)
        {
            Movement(currentPoint);
            if(direction == -1 && transform.position.x <= currentPoint.position.x)
            {
                wayPointIndex += 1;
                if(wayPointIndex >= waypoints.Length)
                {
                    wayPointIndex = 0;
                }
            }
            else if(direction == 1 && transform.position.x >= currentPoint.position.x)
            {
                wayPointIndex += 1;
                if (wayPointIndex >= waypoints.Length)
                {
                    wayPointIndex = 0;
                }
            }
        }

        private void RaycastCheck()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, detectRange, mask);
            if (hit)
            {
                isPatroling = false;
                isChasing = true;
                Debug.Log("Chase");
                Chasing(hit.transform);
            }
            else
            {
                isChasing = false;
                isPatroling = true;
            }
        }

        private void Chasing(Transform player)
        {
            RaycastHit2D attack = Physics2D.Raycast(transform.position, facingDirection, attackRange);
            if (attack)
            {
                Debug.Log("Attack");
                bool usedAttack = false;
                AttackData attDat;
                foreach(AttackData dat in Data.Attacks)
                {
                    if (_onCooldown.Contains(dat) || usedAttack) continue;
                    attDat = dat;
                    break;
                }
                if (!usedAttack) return;
                
            }
            else
            {
                Movement(player);
            }
        }
    }
}
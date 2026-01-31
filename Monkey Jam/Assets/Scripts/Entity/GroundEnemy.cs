using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace MonkeyJam.Entities
{
    public class GroundEnemy : EnemyBase
    {
        int wayPointIndex;
        bool isChasing = false;
        Vector2 facingDirection;

        [SerializeField] LayerMask mask;

        private void Start()
        {
            SetupStats(Data.Stats);

            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            wayPointIndex = 0;

        }

        private void Update()
        {
            if (isPatroling) { MoveToWayPoint(waypoints[wayPointIndex]); }
            else { _animator.SetBool("isRunning", false); }

            RaycastCheck();
        }

        private void MoveToWayPoint(Transform currentPoint)
        {
            _animator.SetBool("isRunning", true);
            float direction;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, 5.0f, mask);
            if (hit)
            {
                isPatroling = false;
                isChasing = true;
            }
            else
            {
                isChasing = false;
                isPatroling = true;
            }
        }

        private void Chasing()
        {

        }
    }
}
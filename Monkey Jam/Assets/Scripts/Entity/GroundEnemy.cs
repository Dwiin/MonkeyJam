using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace MonkeyJam.Entities
{
    public class GroundEnemy : EnemyBase
    {
        int wayPointIndex;

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
        }

        private void MoveToWayPoint(Transform currentPoint)
        {
            _animator.SetBool("isRunning", true);
            float direction;
            if (currentPoint.position.x - transform.position.x < 0) 
            {
                direction = -1;
                Quaternion newRot = Quaternion.identity;
                newRot.y = 180;
                transform.rotation = newRot;
            }
            else
            {
                direction = 1;
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
    }
}
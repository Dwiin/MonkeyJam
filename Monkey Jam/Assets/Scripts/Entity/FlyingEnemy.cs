using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace MonkeyJam.Entities
{
    public class FlyingEnemy : EnemyBase
    {
        int wayPointIndex;
        bool isChasing = false;
        bool isDecending = false;
        bool hasAttacked = false;
        float direction;
        Vector2 facingDirection;
        Transform playerTransform;

        float startingYPos;
        float lastXPos;

        [SerializeField] LayerMask mask;
        [SerializeField] float attackTimer = 2.0f;
        float currentTimer;

        private void Start()
        {
            SetupStats(Data.Stats);

            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            wayPointIndex = 0;
            
            startingYPos = transform.position.y;
        }

        private void Update()
        {
            if (waypoints.Length == 0)
            {
                return;
            }
            if (!hasAttacked)
            {
                Debug.Log("Stupid fuck");
                if (isPatroling) { MoveToWayPoint(waypoints[wayPointIndex]); }
                else { _animator.SetBool("isRunning", false); }

                if(currentTimer >= attackTimer)
                {
                    RaycastCheck();
                }
                else
                {
                    currentTimer += Time.deltaTime;
                }
            }
            else if (isDecending)
            {
                lastXPos += _stats.MoveSpeed * direction * Time.deltaTime;
            }
            else
            {
                ReturnToWayPoint();
            }

            
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

            if (!isChasing)
            {
                Vector3 newPos = transform.position;
                newPos.x += direction * Time.deltaTime * _stats.MoveSpeed;
                transform.position = newPos;
            }            
        }

        private void MovementDown(Transform player)
        {
            Vector3 newRot = transform.rotation.eulerAngles;
            newRot.z = -35;

            transform.rotation = Quaternion.Euler(newRot);

            //Vector3 newPos = transform.position;
            //newPos.y -= _stats.MoveSpeed * Time.deltaTime;
            //transform.position = newPos;

            transform.position = Vector2.Lerp(transform.position, player.position, _stats.MoveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                isDecending = false;
                hasAttacked = true;
            }
        }

        private void MoveToWayPoint(Transform currentPoint)
        {

            Movement(currentPoint);
            
            if (direction == -1 && transform.position.x <= currentPoint.position.x)
            {
                wayPointIndex += 1;
                if (wayPointIndex >= waypoints.Length)
                {
                    wayPointIndex = 0;
                }
            }
            else if (direction == 1 && transform.position.x >= currentPoint.position.x)
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
            if (!isDecending)
            {
                Vector2 pos = transform.position;
                pos.x += 3 * direction;
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, detectRange, mask);
                if (hit)
                {
                    lastXPos = transform.position.x;
                    isPatroling = false;
                    isDecending = true;
                    playerTransform = hit.transform;
                    Chasing(hit.transform);
                }
                else
                {
                    isChasing = false;
                    isPatroling = true;
                }
            }
            else
            {
                Chasing(playerTransform);
            }
        }

        private void Chasing(Transform player)
        {
            RaycastHit2D attack = Physics2D.Raycast(transform.position, Vector2.down, attackRange);
            if (attack)
            {

            }
            else
            {
                Movement(player);
                MovementDown(player);
            }
        }

        private void ReturnToWayPoint()
        {
            Vector3 newRot = transform.rotation.eulerAngles;
            newRot.z = 0;

            transform.rotation = Quaternion.Euler(newRot);

            Vector2 returnPosition;
            returnPosition.x = lastXPos;
            returnPosition.y = startingYPos;

            transform.position = Vector2.Lerp(transform.position,  returnPosition, _stats.MoveSpeed * Time.deltaTime);

            if (transform.position.y >= startingYPos - 0.1)
            {
                hasAttacked = false;
                isDecending = false;
                isPatroling = true;
                currentTimer = 0;
            }            
        }
    }
}
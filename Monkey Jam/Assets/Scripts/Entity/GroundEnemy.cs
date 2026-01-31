using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System.Collections;

namespace MonkeyJam.Entities
{
    public class GroundEnemy : EnemyBase
    {
        int wayPointIndex;
        bool isChasing = false;
        float direction;
        Vector2 facingDirection;

        [SerializeField] private float _attackDelay = 5;
        [SerializeField] LayerMask mask;
        private List<AttackData> _onCooldown;
        private bool _dontAttack = false;

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
            bool usedAttack = false;
            AttackData attDat = new AttackData { };
            Player hitPlayer = null;
            float closestDist = float.MaxValue;
            List<AttackData> valid = new List<AttackData>();
            foreach (AttackData dat in Data.Attacks)
            {
                if (dat.AttackRange < closestDist) closestDist = dat.AttackRange;
                if (_onCooldown.Contains(dat) || usedAttack) continue;
                Collider2D caught = Physics2D.OverlapCircle(transform.position, dat.AttackRange, mask);
                if (caught == null) continue;
                if (!caught.gameObject.TryGetComponent(out hitPlayer)) continue;
                //attDat = dat;
                //usedAttack = true;
                valid.Add(dat);
                //break;
            }
            if (valid.Count > 0)
            {
                attDat = valid[UnityEngine.Random.Range(0, valid.Count)];
                usedAttack = true;
            }
            if (!usedAttack && Vector2.Distance(player.position, transform.position) > closestDist)
            {
                Movement(player);
                return;
            }
            if (!usedAttack || _dontAttack) return;
            Debug.Log($"IsRanged: {attDat.IsRanged}");

            Debug.Log("Attacking player in range!");
            _animator.SetInteger("attackIndex", attDat.AttackRangeIndex);
            _animator.SetTrigger("Attack");
            StartCoroutine(HandleAttackDelay());
            StartCoroutine(HandleCooldown(attDat));

            //RaycastHit2D attack = Physics2D.Raycast(transform.position, facingDirection, attDat.AttackRange, mask);
            //if (attack)
            //{
            //    Debug.LogWarning($"Attack: {attDat.AttackRange}");
            //    _animator.SetInteger("attackIndex", attDat.AttackRangeIndex);
            //    _animator.SetTrigger("Attack");
            //    StartCoroutine(HandleCooldown(attDat));
            //}
            //else
            //{
            //    Movement(player);
            //}
        }

        private IEnumerator HandleCooldown(AttackData data)
        {
            _onCooldown.Add(data);
            yield return new WaitForSeconds(data.Cooldown);
            _onCooldown.Remove(data);
        }

        private IEnumerator HandleAttackDelay()
        {
            _dontAttack = true;
            yield return new WaitForSeconds(_attackDelay);
            _dontAttack = false;
        }
    }
}
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace MonkeyJam.Entities
{
    public class GroundEnemy : EnemyBase
    {   
        private void Start()
        {
            SetupStats(Data.Stats);

            _rb = GetComponent<Rigidbody2D>();
            Debug.Log(wayPoint1.position);
        }

        private void Update()
        {
            MoveToWayPoint(wayPoint1);
        }

        private void MoveToWayPoint(Transform currentPoint)
        {
            float direction;
            if (currentPoint.position.x - transform.position.x < 0) { direction = -1; }
            else { direction = 1; }
            
            Vector3 newPos = transform.position;
            newPos.x += direction * Time.deltaTime * _stats.MoveSpeed;
            transform.position = newPos;

            if(direction == -1 && transform.position.x <= currentPoint.position.x)
            {
                Debug.Log("Reached Point");
            }
            else if(direction == 1 && transform.position.x >= currentPoint.position.x)
            {
                Debug.Log("Reached Point 2");
            }
        }
    }
}
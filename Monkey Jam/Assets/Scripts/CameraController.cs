using UnityEngine;

namespace MonkeyJam.Entities {
    public class CameraController : MonoBehaviour {
        [SerializeField] private Transform _target;
        [SerializeField] private float _followSpeed = 5f;
        [SerializeField] private float _distanceTilMove = 1f;
        private Rigidbody2D _targetRB;

        private void Start() {
            transform.position = _target.position;
            _targetRB = _target.GetComponent<Rigidbody2D>();
        }

        private void Update() {
            if (_targetRB.linearVelocity.magnitude > 0) {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(_target.position.x, _target.position.y) + _targetRB.linearVelocity, _followSpeed * Time.deltaTime);
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, _target.position, _followSpeed * Time.deltaTime);
            }
        }
    }
}
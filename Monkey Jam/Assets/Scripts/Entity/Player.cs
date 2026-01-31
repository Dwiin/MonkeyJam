using MonkeyJam.Managers;
using MonkeyJam.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace MonkeyJam.Entities {
    public class Player : EntityBase
    {
        //[SerializeField] private EnemyBase _toPosess; //Debug only, will be removed for actual implementation
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space]
        [Header("Player Movement Shite")]
        [SerializeField] private float _jumpForce = 20f;
        [SerializeField] private float _gravity = -9.6f;
        [SerializeField] private float _ignoreGravTime = 0.1f;
        [SerializeField] private float _jumpFreezeTime = 0.1f;
        [SerializeField] private float _groundCheckLength = 0.5f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _groundCheckPoint;

        private InputSystem_Actions _controls;
        private Vector2 _moveVector;
        private bool _isJumping = false;
        private float _currentIgnoreGrav = 0f;
        private float _currentJumpTime = 0f;
        private bool _grounded = false;
        private List<AttackData> _onCooldown;
        private AttackData _currentAttack;


        private void Start() {
            if (_spriteRenderer == null) {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            _onCooldown = new List<AttackData>();
            //SetupStats(Data.Stats);
            Posess(Data);
            foreach (Collider2D coll in _attackColliders) {
                coll.enabled = false;
            }
        }

        private void OnEnable() {
            if (_controls == null) {
                _controls = new InputSystem_Actions();
            }
            _controls.Player.Enable();

            _controls.Player.Move.performed += OnMovement;
            _controls.Player.Move.canceled += OnMovement;
            _controls.Player.Jump.performed += OnJump;
            _controls.Player.Attack.performed += OnAttack;
            _controls.Player.Secondary.performed += OnSecondary;
        }

        private void OnMovement(InputAction.CallbackContext context) {
            _moveVector = context.ReadValue<Vector2>();
        }

        private void OnAttack(InputAction.CallbackContext context) {
            if (Data.Attacks == null || Data.Attacks.Length == 0) return;
            if (_onCooldown.Contains(Data.Attacks[0])) return;
            _currentAttack = Data.Attacks[0];
            _animator.SetInteger("attackIndex", 0);
            _animator.SetTrigger("Attack");
            StartCoroutine(HandleCooldown(Data.Attacks[0]));

        }

        private void OnSecondary(InputAction.CallbackContext context) {
            if (Data.Attacks == null || Data.Attacks.Length == 0) return;
            if (_onCooldown.Contains(Data.Attacks[1])) return;
            _currentAttack = Data.Attacks[1];
            _animator.SetInteger("attackIndex", 1);
            _animator.SetTrigger("Attack");
            StartCoroutine(HandleCooldown(Data.Attacks[1]));

        }

        private void OnJump(InputAction.CallbackContext context) {
            if (_isJumping || !_grounded) return;
            _isJumping = true;
            _currentIgnoreGrav = 0;
            _currentJumpTime = 0f;
        }
        
        public void Posess(EnemyData data)
        {
            SetupStats(data.Stats); 
            _animator.runtimeAnimatorController = data.AnimationController;
        }

        private void Update() {
            if (_moveVector == Vector2.zero) return;
            _spriteRenderer.flipX = _moveVector.x < 0;
            foreach(Collider2D collider in _attackColliders) {
                Vector2 offset = Vector2.zero;
                if (collider is BoxCollider2D) {
                    BoxCollider2D box = (BoxCollider2D) collider;
                    offset = _spriteRenderer.flipX ? new Vector2(-box.size.x * 0.5f, 0) : new Vector2(box.size.x * 0.5f, 0);
                }else if (collider is CircleCollider2D) {
                    CircleCollider2D circle = (CircleCollider2D) collider;
                    offset = _spriteRenderer.flipX ? new Vector2(-circle.radius * 0.5f, 0) : new Vector2(circle.radius * 0.5f, 0);
                }else if (collider is CapsuleCollider2D) {
                    CapsuleCollider2D capsule = (CapsuleCollider2D) collider;
                    offset = _spriteRenderer.flipX ? new Vector2(-capsule.size.x * 0.5f, 0) : new Vector2(capsule.size.x * 0.5f, 0);
                }
                collider.offset = offset;
            }
        }

        private void FixedUpdate() {
            _rb.linearVelocity = _moveVector.With(y: 0) * _stats.MoveSpeed;
            _grounded = IsGrounded();

            if (!_isJumping) {
                _rb.linearVelocity = _rb.linearVelocity.Add(y: _gravity);
            }
            else {
                _currentIgnoreGrav += Time.deltaTime;
                if (_currentIgnoreGrav >= _ignoreGravTime) {
                    _currentJumpTime += Time.deltaTime;
                }
                else {
                    _rb.linearVelocity = _rb.linearVelocity.Add(y: _jumpForce);
                }
                if (_currentIgnoreGrav >= _ignoreGravTime && _currentJumpTime >= _jumpFreezeTime) {
                    _isJumping = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject == this.gameObject) return;
            EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
            if (entity == null) return;
            DamageManager.HandleDamage(new DamageInfo() { Damage = _currentAttack.Damage, DamageType = _currentAttack.DamageType, Target = entity, Source = this });
        }

        private IEnumerator HandleCooldown(AttackData data) {
            _onCooldown.Add(data);
            yield return new WaitForSeconds(data.Cooldown);
            _onCooldown.Remove(data);
        }

        private bool IsGrounded() => Physics2D.Raycast(_groundCheckPoint.position, Vector3.down, _groundCheckLength, _groundLayer);

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_groundCheckPoint.position, Vector3.down * _groundCheckLength);
        }
    }
}
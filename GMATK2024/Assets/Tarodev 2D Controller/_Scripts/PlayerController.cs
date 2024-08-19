using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TarodevController
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        // The gun object
        [SerializeField] private Transform _gunObject;
        // The bullet prefab
        [SerializeField] private GameObject _bulletPrefab;
        // Point from bullet will be shot
        [SerializeField] private Transform _shootingPoint;
        // Speed of the bullet
        [SerializeField] private float _bulletSpeed = 10f;
        private Vector2 direction;

        [Space(8)]
        [SerializeField] private Texture2D _cursorSprite;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;
        private bool _inpuEnabled = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

            var hotspot = new Vector2(_cursorSprite.width / 2, _cursorSprite.height / 2);
            Cursor.SetCursor(_cursorSprite, hotspot, CursorMode.Auto);
        }

        private void OnEnable()
        {
            StaticEventHandler.OnGamePaused += StaticEventHandler_OnGamePaused;
        }

        private void OnDisable()
        {
            StaticEventHandler.OnGamePaused -= StaticEventHandler_OnGamePaused;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();

            // Rotate gun in direction of the mouse
            RotateChildToMouse();

            // Shoot if mouse buttons clicked
            Shoot();
        }

        private void StaticEventHandler_OnGamePaused(bool isGamePaused)
        {
            _inpuEnabled = !isGamePaused;
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        // Handle gun rotation in direction of the mouse
        private void RotateChildToMouse()
        {
            if (_gunObject == null) return;

            // Mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Direction to the mouse
            direction = (mousePosition - _gunObject.position).normalized;

            // Angle between the gun and the mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the gun to face the mouse
            _gunObject.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Shoot
        private void Shoot()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Left click shots scale up bullets
            if (Input.GetMouseButtonDown(0))
            {
                ShootBullet(SCALETYPE.ScaleUp);
            }

            // Right click shots scale down bullets
            if (Input.GetMouseButtonDown(1))
            {
                ShootBullet(SCALETYPE.ScaleDown);
            }
        }

        private void ShootBullet(SCALETYPE bulletType)
        {
            if (_bulletPrefab == null || _shootingPoint == null) return;

            // Instantiate the bullet at the shooting point's position and rotation
            GameObject gun = Instantiate(_bulletPrefab, _shootingPoint.position, _gunObject.rotation);

            Bullet bullet = gun.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetScaleType(bulletType);
            }

            // Apply velocity to the bullet object
            Rigidbody2D gunRb = gun.GetComponent<Rigidbody2D>();
            if (gunRb != null)
            {
                gunRb.velocity = direction * _bulletSpeed;
                SoundManager.instance.PlayClip(AUDIOCLIPTYPE.Shoot);
            }
        }

        private void FixedUpdate()
        {
            if (!_inpuEnabled)
            {
                return;
            }

            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();

            ApplyMovement();
        }

        #region Collisions

        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        public void Die()
        {
            StaticEventHandler.CallPlayerDiedEvent();
            Destroy(gameObject);
        }

        private void ApplyMovement() => _rb.velocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}

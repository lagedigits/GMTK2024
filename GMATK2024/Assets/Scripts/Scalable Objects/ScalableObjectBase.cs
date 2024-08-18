using TarodevController;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public abstract class ScalableObjectBase : MonoBehaviour
{
    [Space(8)]
    [SerializeField] protected float _maxScaleSize = 4f;
    [SerializeField] protected float _scaleSpeed = 0.3f;
    [SerializeField] protected float _minScaleSize = 1f;
    [SerializeField] protected float _bounceAmount = 0.35f; // How much it overshoots
    [SerializeField] protected float _bounceDuration = 0.25f; // How long the bounce lasts
    [SerializeField] protected float _killVelocityThreshold = 3f;

    [Space]
    [SerializeField] protected bool _isWeighted;

    protected SpriteRenderer _sr;
    protected Vector2 _originalSize;
    protected Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _originalSize = _sr.size;
    }
    private void Start()
    {
        if (_isWeighted)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.isKinematic = true;
        }
    }

    public abstract void Scale(SCALETYPE scalingType);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isWeighted && collision.collider.TryGetComponent<PlayerController>(out var player))
        {
            ContactPoint2D contactPoint = collision.GetContact(0);

            Vector2 objectPosition = transform.position;
            Vector2 playerPosition = collision.collider.transform.position;

            if (objectPosition.y > playerPosition.y && _rb.velocity.y < -_killVelocityThreshold)
            {
                Vector2 impactDirection = (contactPoint.point - playerPosition).normalized;
                if (impactDirection.y > 0.5f) // This checks if the angle is mostly downward
                {
                    player.Die();
                }
            }
        }
    }

}

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

    protected SpriteRenderer _sr;
    protected Vector2 _originalSize;

    protected virtual void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _originalSize = _sr.size;
    }
    public abstract void Scale(SCALETYPE scalingType);
}

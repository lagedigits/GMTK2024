using System.Collections;
using UnityEngine;

public class ScalableObjectExpand : ScalableObjectBase
{
    public override void Scale(SCALETYPE scalingType)
    {
        switch (scalingType)
        {
            case SCALETYPE.ScaleUp:
                StartCoroutine(nameof(ScaleUpWithBounce));
                break;

            case SCALETYPE.ScaleDown:
                StartCoroutine(nameof(ScaleDownWithBounce));
                break;
        }
    }

    private IEnumerator ScaleUpWithBounce()
    {
        Vector2 targetScale = new Vector2(_maxScaleSize + _bounceAmount, _maxScaleSize + _bounceAmount);
        Vector2 bounceScale = new Vector2(_maxScaleSize, _maxScaleSize);

        float elapsedTime = 0f;

        while (elapsedTime < _bounceDuration)
        {
            _sr.size = Vector2.Lerp(_originalSize, targetScale, elapsedTime / _bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_isWeighted)
        {
            _rb.isKinematic = false;
            _rb.gravityScale = 7f;
            _rb.mass = 60f;
        }

        elapsedTime = 0f;
        while (elapsedTime < _bounceDuration)
        {
            _sr.size = Vector2.Lerp(targetScale, bounceScale, elapsedTime / _bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _sr.size = bounceScale;
    }

    private IEnumerator ScaleDownWithBounce()
    {
        Vector2 currentScale = _sr.size;
        Vector2 bounceOutScale = new Vector2(currentScale.x + _bounceAmount, currentScale.y + _bounceAmount);
        Vector2 targetScale = new Vector2(_minScaleSize, _minScaleSize);  // Minimum scale size as the target

        float elapsedTime = 0f;

        // First, scale up slightly to create the bounce-out effect (only if not already at minimum size)
        if (currentScale.x > _minScaleSize || currentScale.y > _minScaleSize)
        {
            while (elapsedTime < _bounceDuration)
            {
                _sr.size = Vector2.Lerp(currentScale, bounceOutScale, elapsedTime / _bounceDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        elapsedTime = 0f;
        // Now, scale down to the target (minimum) size
        while (elapsedTime < _bounceDuration)
        {
            _sr.size = Vector2.Lerp(bounceOutScale, targetScale, elapsedTime / _bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the scale is set to the target size at the end
        _sr.size = targetScale;
    }
}

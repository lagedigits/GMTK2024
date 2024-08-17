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
        Vector2 currentScale = _sr.size;  // Assume the current size is 4x4
        Vector2 bounceOutScale = new Vector2(currentScale.x + _bounceAmount, currentScale.y + _bounceAmount);  // Bounce out to 4.6x4.6
        Vector2 targetScale = _originalSize;  // Target is the original size, e.g., 4x4

        float elapsedTime = 0f;

        // First, scale up slightly to create the bounce-out effect
        while (elapsedTime < _bounceDuration)
        {
            _sr.size = Vector2.Lerp(currentScale, bounceOutScale, elapsedTime / _bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        // Now, scale down to the target (original) size
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

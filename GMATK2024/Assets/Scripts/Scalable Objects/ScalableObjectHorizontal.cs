using System.Collections;
using UnityEngine;

public class ScalableObjectHorizontal : ScalableObjectBase
{
    public override void Scale(ScalingType scalingType)
    {
        switch (scalingType)
        {
            case ScalingType.ScaleUp:
                StartCoroutine(nameof(ScaleUpWithBounce));
                break;

            case ScalingType.ScaleDown:
                StartCoroutine(nameof(ScaleDownWithBounce));
                break;
        }
    }

    private IEnumerator ScaleUpWithBounce()
    {
        Vector2 targetScale = new Vector2(_maxScaleSize + _bounceAmount, _originalSize.y);
        Vector2 bounceScale = new Vector2(_maxScaleSize, _originalSize.y);

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
        Vector2 currentScale = _sr.size;
        Vector2 bounceOutScale = new Vector2(currentScale.x + _bounceAmount, currentScale.y);
        Vector2 targetScale = new Vector2(_minScaleSize, _originalSize.y);

        float elapsedTime = 0f;

        // Bounce out effect before starting the scale down
        while (elapsedTime < _bounceDuration)
        {
            _sr.size = Vector2.Lerp(currentScale, bounceOutScale, elapsedTime / _bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        // Scale down to the target size
        while (elapsedTime < _bounceDuration)
        {
            _sr.size = Vector2.Lerp(bounceOutScale, targetScale, elapsedTime / _bounceDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _sr.size = targetScale;
    }
}

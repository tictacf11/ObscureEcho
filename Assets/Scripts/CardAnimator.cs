using System;
using System.Collections;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    [SerializeField] float flipAnimationTime = .5f;
    bool isFlipping = false;

    public void Flip(Action onHalfAnimation, Action onAnimationEnded, bool frontToBack = true)
    {
        StartCoroutine(FlipRoutine(onHalfAnimation, onAnimationEnded, frontToBack));
    }

    private IEnumerator FlipRoutine(Action onHalfAnimation, Action onAnimationEnded, bool frontToBack)
    {
        yield return new WaitUntil(() => !isFlipping);
        isFlipping = true;

        float halfDuration = flipAnimationTime / 2;

        for (float i = 0; i <= halfDuration; i += Time.deltaTime)
        {
            float angle = frontToBack ? Mathf.Lerp(0, 90, i / halfDuration) : Mathf.Lerp(180, 90, i / halfDuration);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        onHalfAnimation?.Invoke();

        for (float i = 0; i <= halfDuration; i += Time.deltaTime)
        {
            float angle = frontToBack ? Mathf.Lerp(90, 180, i / halfDuration) : Mathf.Lerp(90, 0, i / halfDuration);
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, 0, 0);
        onAnimationEnded?.Invoke();
        isFlipping = false;
    }
}

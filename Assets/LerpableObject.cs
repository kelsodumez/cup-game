using UnityEngine;

public class LerpableObject : MonoBehaviour
{
    private Vector3 _lerpTarget;
    private Vector3 _startPos;
    private float _startTime;
    private float _lerpDist;
    [SerializeField] private bool _doLerp = false;

    [SerializeField] float _lerpSpeed = 5.0f;

    void Update()
    {
        if (_doLerp)
        {
            DoLerp();
        }
    }

    public void BeginLerpingToPoint(Vector3 target, float lerpSpeed)
    {
        _doLerp = true;
        _startTime = Time.time;
        _startPos = transform.position;
        _lerpTarget = target;
        _lerpSpeed = lerpSpeed;
        _lerpDist = Vector3.Distance(_startPos, target);

    }

    private void DoLerp()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - _startTime) * _lerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / _lerpDist;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(_startPos, _lerpTarget, fractionOfJourney);

        if ((transform.position - _lerpTarget).sqrMagnitude <= 0.0001f)
        {
            ResetLerper();
        }
    }

    public bool IsLerping()
    {
        // returns if currently lerping
        return _doLerp;
    }

    private void ResetLerper()
    {
        _doLerp = false;
    }
}

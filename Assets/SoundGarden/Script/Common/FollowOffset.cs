using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOffset : MonoBehaviour
{
    public Vector3 offset;
    private float maxFollowSpeed;
    [SerializeField] private Transform followTransform;
    private Coroutine followCoroutine;

    public bool isXLocal = false;
    public bool isYLocal = false;
    public bool isZLocal = false;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetFollowStats(Transform _followTransform, Vector3 _offSet, float _maxFollowSpeed, bool _isXLocal = false, bool _isYLocal = false, bool _isZLocal = false)
    {
        followTransform = _followTransform;
        offset = _offSet;
        maxFollowSpeed = _maxFollowSpeed;
        isXLocal = _isXLocal;
        isYLocal = _isYLocal;
        isZLocal = _isZLocal;
    }

    public void SnapToFollowPos()
    {
        Vector3 offsetRelativeToForward = GetAdjustedOffset();
        Vector3 targetPos = followTransform.transform.position + offsetRelativeToForward;
        targetPos = new Vector3(targetPos.x, followTransform.transform.position.y + offset.y, targetPos.z);
        this.transform.position = targetPos;
    }

    public void StartFollow()
    {
        SnapToFollowPos();
        followCoroutine = StartCoroutine(FollowObject());
    }

    public void StopFollow(float stopTime = 0.5f)
    {
        StartCoroutine(StopFollowObject(stopTime));
    }

    private IEnumerator FollowObject()
    {
        while (this.transform.gameObject.activeInHierarchy)
        {
            Vector3 offsetRelativeToForward = GetAdjustedOffset();
            Vector3 targetPos = followTransform.transform.position + offsetRelativeToForward;
            targetPos = new Vector3(targetPos.x, followTransform.transform.position.y + offset.y, targetPos.z);
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, maxFollowSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator StopFollowObject(float stopTime = 0.5f)
    {
        yield return new WaitForSeconds(stopTime);

        StopCoroutine(followCoroutine);
    }

    private Vector3 GetAdjustedOffset()
    {
        Vector3 adjustedOffset = Vector3.zero;

        if (isXLocal)
        {
            adjustedOffset += offset.x * followTransform.transform.right;
        }
        if (isYLocal)
        {
            adjustedOffset += offset.y * followTransform.transform.up;
        }
        if (isZLocal)
        {
            adjustedOffset += offset.z * followTransform.transform.forward;
        }

        return adjustedOffset;
    }
}

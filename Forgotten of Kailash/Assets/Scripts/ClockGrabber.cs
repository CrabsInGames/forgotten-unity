using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClockGrabber : MonoBehaviour
{
    public Glyph hand;
    public Transform pivot;
    public ClockGrabber other;
    public Transform[] LockPoints;
    public List<Transform> correctPoints;
    bool held;

    public UnityEvent onRightPosition;
    public UnityEvent onWrongPosition;

    public void StartGrab()
    {
        held = true;
        hand.transform.position = pivot.position;
        hand.HighlightReset();
        StartCoroutine(GrabCont());
    }
    IEnumerator GrabCont()
    {
        while (held)
        {
            Transform closest = ClosestPoint();
            hand.transform.rotation = closest.rotation;
            yield return null;
        }
    }
    public void EndGrab()
    {
        Transform target = ClosestPoint();
        transform.parent = target;
        transform.localPosition = Vector3.zero;
        
        held = false;

        hand.transform.localPosition = Vector3.zero;

        if (correctPoints.Contains(transform.parent))
        {
            hand.HighlightRight();
            onRightPosition.Invoke();
        }
        else
            onWrongPosition.Invoke();
    }

    Transform ClosestPoint()
    {
        Transform closest = LockPoints[0];
        for (int i = 1; i < LockPoints.Length; i++)
        {
            switch (LockPoints[i] == other.transform.parent)
            {
                case false:
                    Vector3 targetOffset = closest.position - transform.position;
                    Vector3 newOffset = LockPoints[i].position - transform.position;
                    switch (newOffset.sqrMagnitude < targetOffset.sqrMagnitude)
                    {
                        case true:
                            closest = LockPoints[i];
                            break;
                    }
                    break;
            }
        }
        return closest;
    }

    private void OnDrawGizmos()
    {
        switch (held)
        {
            case true:
                Gizmos.color = Color.blue;
                break;
            case false:
                Gizmos.color = Color.red;
                break;
        }
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.1f);
    }
}

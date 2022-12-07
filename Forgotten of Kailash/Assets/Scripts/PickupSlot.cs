using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupSlot : MonoBehaviour
{
    [SerializeField] float snapTime;
    public Rigidbody desiredObject;
    [SerializeField] Rigidbody snapped;
    int holdLayer;
    [SerializeField] Mesh previewMesh;
    //[SerializeField] LayerMask holdLayer;
    //[SerializeField] LayerMask pickupLayer;

    private void Start()
    {
        holdLayer = LayerMask.NameToLayer("Hold");

        if (snapped)
        {
            snapped.isKinematic = true;
            snapped.transform.SetParent(transform);
            OnSnap.Invoke();
            switch (snapped == desiredObject)
            {
                case true:
                    OnDesiredSnap.Invoke();
                    Glyph glyph = snapped.GetComponent<Glyph>();
                    switch ((bool)glyph)
                    {
                        case true:
                            glyph.HighlightRight();
                            break;
                    }
                    break;
            }
        }
    }

    public UnityEvent OnSnap;
    public UnityEvent OnDrop;
    public UnityEvent OnDesiredSnap;
    public UnityEvent OnDesiredDrop;

    private void OnTriggerEnter(Collider other)
    {
        if (snapped)
            return;

        Rigidbody otherRB = other.attachedRigidbody;
        Debug.Log(name + " collided with " + otherRB);
        if (!otherRB)
            return;

        Debug.Log(otherRB + " layer is " + LayerMask.LayerToName(otherRB.gameObject.layer));
        switch (otherRB.gameObject.layer == holdLayer)
        {
            case true:
                Debug.Log("drop " + otherRB);
                PlayerMovement.active.EndHold(true);
                StartCoroutine(SnapPositionCor(otherRB));
                snapped = otherRB;
                OnSnap.Invoke();
                switch (snapped == desiredObject)
                {
                    case true:
                        OnDesiredSnap.Invoke();
                        Glyph glyph = snapped.GetComponent<Glyph>();
                        switch((bool)glyph)
                        {
                            case true:
                                glyph.HighlightRight();
                                break;
                        }
                        break;
                }
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == snapped)
        {
            if (other.attachedRigidbody == desiredObject)
            {
                OnDesiredDrop.Invoke();
                Glyph glyph = other.attachedRigidbody.GetComponent<Glyph>();
                if (glyph)
                    glyph.HighlightReset();
            }

            snapped.transform.SetParent(null);
            snapped = null;
            OnDrop.Invoke();
        }
    }

    IEnumerator SnapPositionCor(Rigidbody rb)
    {
        Vector3 startPos = rb.position;
        Quaternion startRot = rb.rotation;
        rb.isKinematic = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / snapTime;
            rb.transform.position = Vector3.Lerp(startPos, transform.position, t);
            rb.transform.rotation = Quaternion.Lerp(startRot, transform.rotation, t);
            yield return null;
        }
        rb.transform.SetParent(transform);
    }

    private void OnDrawGizmos()
    {
        switch ((bool)snapped)
        {
            case true:
                Gizmos.color = Color.blue;
                break;
            case false:
                Gizmos.color = Color.red;
                break;
        }
        Gizmos.DrawWireMesh(previewMesh, transform.position, transform.rotation);
    }
}

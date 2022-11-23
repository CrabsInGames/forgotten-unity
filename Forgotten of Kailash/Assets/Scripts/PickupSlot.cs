using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSlot : MonoBehaviour
{
    Rigidbody snapped;
    int holdLayer;
    //[SerializeField] LayerMask holdLayer;
    //[SerializeField] LayerMask pickupLayer;
    [SerializeField] float snapTime;

    private void Start()
    {
        holdLayer = LayerMask.NameToLayer("Hold");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (snapped)
            return;

        Rigidbody otherRB = other.attachedRigidbody;
        Debug.Log(name + " collided with " + otherRB);
        if (!otherRB)
            return;

        Debug.Log(otherRB + " layer is " + LayerMask.LayerToName(otherRB.gameObject.layer));
        if (otherRB.gameObject.layer == holdLayer)
        {
            Debug.Log("drop " + otherRB);
            PlayerMovement.active.EndHold();
            StartCoroutine(SnapPositionCor(otherRB));
            snapped = otherRB;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == snapped)
            snapped = null;
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
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}

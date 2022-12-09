using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeTrigger : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onPlayerEnter;
    [SerializeField] bool moveRigidbodies;
    [SerializeField] Vector3 rbVector;
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Cube":
                switch (moveRigidbodies && !other.attachedRigidbody.isKinematic)
                {
                    case true:
                        Rigidbody otherRB = other.attachedRigidbody;
                        Debug.Log(name + " moving " + otherRB + " away");
                        Vector3 startPos = otherRB.position;
                        Vector3 moveWorld = transform.TransformDirection(rbVector);
                        Vector3 endPos = startPos + moveWorld;
                        otherRB.position = endPos;
                        break;
                }
                break;
            case "Player":
                Debug.Log("player entered " + name);
                onPlayerEnter.Invoke();
                break;
        }
    }

    public void MovePlayerBy(float z)
    {
        Vector3 moveLocal = new Vector3(0, 0, z);

        PlayerMovement.active.characterController.enabled = false;
        Vector3 startPos = PlayerMovement.active.transform.position;
        Vector3 moveWorld = transform.TransformDirection(moveLocal);
        Vector3 endPos = startPos + moveWorld;
        PlayerMovement.active.transform.position = endPos;
        PlayerMovement.active.characterController.enabled = true;
    }
}

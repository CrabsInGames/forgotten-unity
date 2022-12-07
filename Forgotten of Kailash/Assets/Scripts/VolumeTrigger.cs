using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeTrigger : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onEnter;
    
    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke();
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

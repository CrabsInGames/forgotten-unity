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
}

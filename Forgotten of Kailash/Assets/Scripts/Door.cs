using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float openTime;
    [SerializeField] Transform[] wings;
    [SerializeField] Transform[] closed;
    [SerializeField] Transform[] open;

    public void Open()
    {
        Debug.Log("opening door " + name);
        for (int i = 0; i < wings.Length; i++)
        {
            StartCoroutine(MoveDoorCor(wings[i].rotation, open[i].rotation, wings[i]));
        }
    }
    public void Close()
    {
        Debug.Log("closing door " + name);
        for (int i = 0; i < wings.Length; i++)
        {
            StartCoroutine(MoveDoorCor(wings[i].rotation, closed[i].rotation, wings[i]));
        }
    }

    IEnumerator MoveDoorCor(Quaternion startRotation, Quaternion endRotation, Transform target)
    {
        float t = 0;
        while (t < 1) 
        {
            t += Time.deltaTime / openTime;
            Quaternion q = Quaternion.Lerp(startRotation, endRotation, t);
            target.rotation = q;
            yield return null;
        }
    }
}

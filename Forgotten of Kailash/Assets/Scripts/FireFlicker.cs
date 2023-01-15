using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float timeStep;
    Vector3 startPos;
    public float t;
    public Vector3 from;
    public Vector3 to;

    private void Start()
    {
        startPos = transform.position;
        from = transform.position;
        to = transform.position;
        t = 1;
    }

    private void Update()
    {
        t += Time.deltaTime / timeStep;
        switch (t >= 1)
        {
            case true:
                from = to;
                to = RandomVector();
                t = 0;
                break;
            case false:
                Vector3 position = Vector3.Lerp(from, to, t);
                transform.position = position;
                break;
        }
    }
    Vector3 RandomVector()
    {
        float x = Random.value - 0.5f;
        float y = Random.value - 0.5f;
        float z = Random.value - 0.5f;
        Vector3 offset = new Vector3(x, y, z);
        Vector3 offsetN = offset.normalized * radius;
        return startPos + offsetN;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

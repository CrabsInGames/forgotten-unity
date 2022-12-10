using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int tentacles;
    [SerializeField] int maxTentacles;
    [Range(0, 1)] public int redness;
    [SerializeField] float maxRednessSize;
    [SerializeField] Animator animator;
    [SerializeField] Transform[] pillars;
    [SerializeField] float pillarRaiseTime = 1;
    int activePillarCount = 0;
    [SerializeField] GameObject redVFX;

    void Start()
    {
        if (!animator)
            animator = GetComponent<Animator>();
        UpdateRedness();
    }

    public void RaisePillar(int index)
    {
        StartCoroutine(PillarCor(pillars[index]));
    }
    IEnumerator PillarCor(Transform pillar)
    {
        float startY = pillar.position.y;
        float endY = 0;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / pillarRaiseTime;
            float y = Mathf.Lerp(startY, endY, t);
            Vector3 position = pillar.transform.position;
            position.y = y;
            pillar.transform.position = position;
            yield return null;
        }
        Transform vfx = pillar.GetChild(0);
        vfx.gameObject.SetActive(true);
        activePillarCount++;
        UpdateRedness();
    }
    void UpdateRedness()
    {
        redness = activePillarCount / pillars.Length;
        redVFX.SetActive(activePillarCount > 0);
        float size = maxRednessSize * redness;
        Debug.Log("red vfx active " + redVFX.active + "; size is " + activePillarCount);
        redVFX.transform.localScale = activePillarCount * Vector3.one;
    }
}

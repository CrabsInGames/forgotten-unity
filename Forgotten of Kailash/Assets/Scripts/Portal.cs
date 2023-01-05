using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [Range(0, 1)] public int redness;
    [SerializeField] float maxRednessSize;
    [SerializeField] Transform[] pillars;
    [SerializeField] float pillarRaiseTime = 1;
    int activePillarCount = 0;
    [SerializeField] GameObject redVFX;

    [SerializeField] float puzzleTimeLimit;
    float tentacleTime;
    [SerializeField] Animator[] tentacles;
    List<Animator> tentaclesIn;
    List<Animator> tentaclesOut;
    int a_out = Animator.StringToHash("Out");
    public UnityEvent AllOut;

    void Start()
    {
        UpdateRedness();
        tentaclesIn = new List<Animator>();
        tentaclesIn.AddRange(tentacles);
        tentaclesOut = new List<Animator>();
        tentacleTime = puzzleTimeLimit / (tentacles.Length + 1);
        Debug.Log("tentacles appear every " + tentacleTime + " seconds");
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

    public void StartCountdown()
    {
        Debug.Log("Countdown started");
        StartCoroutine(TentacleCor());
    }
    public void RandomTentacleOut()
    {
        if (tentaclesIn.Count <= 0)
        {
            Debug.Log("time is over");
            StopCoroutine(TentacleCor());
            AllOut.Invoke();
        }    

        int r = Random.Range(0, tentaclesIn.Count - 1);
        Animator chosen = tentaclesIn[r];
        Debug.Log(chosen + " chosen");
        chosen.SetBool(a_out, true);

        tentaclesIn.Remove(chosen);
        string InMessage = "tentacles in: ";
        foreach (Animator ten in tentaclesIn)
        {
            InMessage += ten.name + ", ";
        }
        Debug.Log(InMessage);

        tentaclesOut.Add(chosen);
        string OutMessage = "tentacles out: ";
        foreach (Animator ten in tentaclesOut)
        {
            OutMessage += ten.name + ", ";
        }
        Debug.Log(OutMessage);
        //Debug.Log(tentaclesOut.Count + " tentacles out");
    }
    public void HideAllTentacles()
    {
        StopCoroutine(TentacleCor());
        tentaclesOut.Clear();
        tentaclesIn.Clear();
        tentaclesIn.AddRange(tentacles);
        foreach (Animator anim in tentacles)
        {
            anim.SetBool(a_out, false);
        }
        StartCoroutine(TentacleCor());
    }
    IEnumerator TentacleCor()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(tentacleTime);
            RandomTentacleOut();
        }
    }
}

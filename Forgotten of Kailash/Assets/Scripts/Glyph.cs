using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glyph : MonoBehaviour
{
    Renderer rend;
    Color baseCol;
    [SerializeField] Color rightCol;
    [SerializeField] Color wrongCol;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        baseCol = rend.material.color;
    }

    public void HighlightWrong()
    {
        rend.material.color = wrongCol;
    }
    public void HighlightRight()
    {
        if (!rend) return;
        rend.material.color = rightCol;
    }
    public void HighlightReset()
    {
        rend.material.color = baseCol;
    }
}

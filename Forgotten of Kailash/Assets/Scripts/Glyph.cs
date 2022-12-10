using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glyph : MonoBehaviour
{
    Renderer rend;
    Color baseCol;
    [SerializeField] Color rightCol;
    [SerializeField] Color wrongCol;

    SpriteRenderer[] spriteRenderers;
    Color baseSpriteCol;
    [SerializeField] Color rightSpriteCol;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        baseCol = rend.material.color;
        spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        switch (spriteRenderers.Length > 0)
        {
            case true:
                baseSpriteCol = spriteRenderers[0].color;
                break;
            case false:
                Debug.Log(name + " has no sprite renderers");
                break;
        }
    }

    public void HighlightWrong()
    {
        rend.material.color = wrongCol;
    }
    public void HighlightRight()
    {
        if (!rend) return;
        rend.material.color = rightCol;
        foreach (SpriteRenderer s in spriteRenderers)
            s.color = rightSpriteCol;
    }
    public void HighlightReset()
    {
        rend.material.color = baseCol;
        foreach (SpriteRenderer s in spriteRenderers)
            s.color = baseSpriteCol;
    }
}

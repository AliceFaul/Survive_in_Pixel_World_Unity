using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneColorLerp : MonoBehaviour
{
    public Transform runeParent;

    public SpriteRenderer[] runes;
    public float lerpTime = 3f;

    private Color currentColor;
    private Color targetColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runes = new SpriteRenderer[runeParent.childCount];
        for(int i = 0; i < runeParent.childCount; i++)
        {
            runes[i] = runeParent.GetChild(i).GetComponent<SpriteRenderer>();
        }

        targetColor = runes[0].color;
        currentColor = targetColor;
    }

    private void Update()
    {
        currentColor = Color.Lerp(currentColor, targetColor, lerpTime * Time.deltaTime);
        foreach (SpriteRenderer r in runes)
        {
            r.color = currentColor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            targetColor.a = 1.0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetColor.a = 0.0f;
        }
    }
}

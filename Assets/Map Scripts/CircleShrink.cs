using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShrink : MonoBehaviour
{
    bool isOutside = true;
    [SerializeField]
    float minSize, shrinkMultiplier;
    bool isShrinked = false;

    void Update()
    {
        if(isOutside)
        {
            Damage();
        }
        else
        {
            Debug.Log("is ");
        }
        if(!isShrinked)
        StartCoroutine(Shrink());
    }

    void Damage()
    {
        Debug.Log("is damaged");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOutside = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOutside = true;
        }
    }

    IEnumerator Shrink()
    {
        Vector3 minScale = new Vector3 (minSize, minSize);
        Vector3 startScale = transform.localScale;
        float timer = 0f;
        while(transform.localScale.x > minSize)
        {
            transform.localScale = Vector3.Lerp(startScale, minScale, timer/shrinkMultiplier);
            timer += Time.deltaTime;
            yield return null;
        }
        isShrinked = true;
    }
}

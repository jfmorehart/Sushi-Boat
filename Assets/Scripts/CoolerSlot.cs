using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerSlot : Station
{
    private Vector3 ogPos;
    private void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(Hovering());
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(Unhovering());
    }

    public override void Start()
    {
        base.Start();
        ogPos = transform.position;
    }

	public override bool OnItemAdd(Item item)
	{
        bool onitem = base.OnItemAdd(item);
     //   if (onitem) { 
	    //    ogPos -
	    //}

        return onitem;
	}

	IEnumerator Hovering()
    {
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        float duration = 0.3f;
        Vector3 hoverPos = ogPos + new Vector3(0, 1f, 0);
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, hoverPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = hoverPos;
    }

    IEnumerator Unhovering()
    {
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        float duration = 0.3f;
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, ogPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = ogPos;
    }
}

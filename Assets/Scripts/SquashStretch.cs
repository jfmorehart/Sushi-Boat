using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SquashStretch : MonoBehaviour
{
    private Vector3 ogScale;
    public float clickScale = 1.2f;
    public Vector3 squashScale = new Vector3(1.1f,0.9f,1f);
    public float squashDuration=0.3f;
    public Vector3 stretchScale = new Vector3(0.9f,1.1f,1f);
    public float stretchDuration=0.3f;
    public float returnDuration=0.3f;
    private void Start()
    {
        ogScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        SS(1);
    }

    private void OnMouseDown()
    {
        SS(clickScale);
    }

    private void OnMouseExit()
    {
        transform.localScale = ogScale;
    }


    public void SS(float extraScale)
    {
        StartCoroutine(SquashAndStretch(clickScale));
    }
    IEnumerator SquashAndStretch(float extraScale)
    {
        transform.localScale = ogScale;
       Tween squash1 = transform.DOScale(new Vector3( ogScale.x*squashScale.x,ogScale.y*squashScale.y,ogScale.z*squashScale.z)*extraScale,squashDuration);
        yield return squash1.WaitForCompletion();
        Tween stretch1 = transform.DOScale(new Vector3(ogScale.x*stretchScale.x,ogScale.y*stretchScale.y,ogScale.z*stretchScale.z)*extraScale,stretchDuration);
        yield return stretch1.WaitForCompletion();
        Tween back = transform.DOScale(ogScale,returnDuration);
        yield return back.WaitForCompletion();
        transform.localScale = ogScale;
    }
}

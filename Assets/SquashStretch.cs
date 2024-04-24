using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SquashStretch : MonoBehaviour
{
    private Vector3 ogScale;
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
        StartCoroutine(SquashAndStretch());
    }

    private void OnMouseExit()
    {
        transform.localScale = ogScale;
    }

    IEnumerator SquashAndStretch()
    {
        transform.localScale = Vector3.one;
       Tween squash1 = transform.DOScale(new Vector3( ogScale.x*squashScale.x,ogScale.y*squashScale.y,ogScale.z*squashScale.z),squashDuration);
        yield return squash1.WaitForCompletion();
        Tween stretch1 = transform.DOScale(new Vector3(ogScale.x*stretchScale.x,ogScale.y*stretchScale.y,ogScale.z*stretchScale.z),stretchDuration);
        yield return stretch1.WaitForCompletion();
        Tween back = transform.DOScale(ogScale,returnDuration);
        yield return back.WaitForCompletion();
        transform.localScale = ogScale;
    }
}

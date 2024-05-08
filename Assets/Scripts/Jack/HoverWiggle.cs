using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverWiggle : MonoBehaviour
{
    public float scale, wforce, decay, springConstant;
    float hoverImpulse = 0.01f;

    // Update is called once per frame
    void Update()
    {
        wforce *= 1 - Time.deltaTime * decay;
        wforce += -springConstant * (scale - 1) * Time.deltaTime;
        scale += wforce * Time.deltaTime;
        transform.localScale = Vector3.one * scale;
    }

	private void OnMouseOver()
	{
        wforce += hoverImpulse;
        Debug.Log("mouseOver");
	}
}

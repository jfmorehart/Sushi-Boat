using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Clickable : MonoBehaviour
{
	public virtual void Awake()
	{
		foreach(Renderer r in transform.GetComponentsInChildren<Renderer>()) {
			r.material = new Material(r.material);
		}
		if(TryGetComponent(out Renderer m)) {
			m.material = new Material(m.material);
		}
	}

	public virtual bool OnColliderClicked() {
        return false;
    }


}

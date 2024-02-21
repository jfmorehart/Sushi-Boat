using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	public virtual void OnColliderClicked() {
        Debug.Log(gameObject.name + "clicked");
    }
}

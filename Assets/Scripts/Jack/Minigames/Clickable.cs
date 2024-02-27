using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	public virtual bool OnColliderClicked() {
        Debug.Log(gameObject.name + "clicked");
        return false;
    }
}

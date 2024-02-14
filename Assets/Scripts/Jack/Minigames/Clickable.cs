using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	public virtual void OnColldierClicked() {
        Debug.Log(gameObject.name + "clicked");
    }
}

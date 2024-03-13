using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
	public virtual bool OnColliderClicked() {
        return false;
    }
}

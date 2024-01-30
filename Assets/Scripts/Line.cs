using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	public Vector3 origin;
	float width = 0.1f;

	private void Update()
	{
		Vector3 parentPosition = transform.parent.position;
		transform.position = (parentPosition + origin) * 0.5f;
		float delta = origin.y - parentPosition.y;
		transform.localScale = new Vector3(width, Mathf.Abs(delta));
	}
}

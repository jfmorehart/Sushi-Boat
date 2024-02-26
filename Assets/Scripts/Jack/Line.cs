using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	Vector2 origin;
	public Vector3 hookOffset = new Vector3(0, 0.1f, 0);

	float width = 0.1f;

	private void Awake()
	{
		origin = transform.position;
	}
	private void Update()
	{
		Vector2 parentPosition = transform.parent.position + hookOffset;
		transform.position = 0.5f * (origin + parentPosition);
		float delta = origin.y - parentPosition.y;
		transform.localScale = new Vector3(width, Mathf.Abs(delta));
	}
}

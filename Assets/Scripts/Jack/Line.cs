using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	public Vector2 origin;
	public Vector3 hookOffset = new Vector3(0, 0.1f, 0);

	float width = 0.1f;

	private void Awake()
	{
		origin = new Vector2(transform.position.x, origin.y);
	}
	private void Update()
	{
		transform.parent.rotation = Quaternion.identity;
		Vector2 parentPosition = transform.parent.position + hookOffset;

		//workaround to fix rotation issues

		transform.position = 0.5f * (origin + parentPosition);
		float delta = origin.y - parentPosition.y;
		transform.localScale = new Vector3(width, Mathf.Abs(delta));
	}
}

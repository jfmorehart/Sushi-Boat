using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	public Transform topAnchor;
	public Transform lure;

	float width = 0.1f;

	private void Update()
	{

		//workaround to fix rotation issues

		transform.position = 0.5f * ((Vector2)topAnchor.position + (Vector2)lure.position);
		Vector2 rdelt = lure.position - transform.position;
		float zrot = Mathf.Atan2(rdelt.y, rdelt.x);
		transform.eulerAngles = new Vector3(0, 0, zrot * Mathf.Rad2Deg + 90);
		float delta = topAnchor.position.y - lure.position.y;
		transform.localScale = new Vector3(width, Mathf.Abs(delta));
	}
}

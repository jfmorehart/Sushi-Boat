using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public static Selection instance;
	public Vector2 mouseWorldPosition;

	private void Awake()
	{
		if(instance == null || instance == this) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	// Update is called once per frame
	void Update()
    {
		Vector3 mposQuery = Input.mousePosition; //mouse pos in pixel coords
		mposQuery.z = 10; // distance from camera to geometry
		mouseWorldPosition = Camera.main.ScreenToWorldPoint(mposQuery);

		if (Input.GetMouseButtonDown(0)) {

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);
            if (hit != null) { 
	            if(hit.TryGetComponent(out Clickable c)){
                    c.OnColliderClicked();
		        }
	        }
        }
	}
}

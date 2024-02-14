using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mposQuery = Input.mousePosition; //mouse pos in pixel coords
            mposQuery.z = 10; // distance from camera to geometry
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mposQuery);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);
            if (hit != null) { 
	            if(hit.TryGetComponent(out Clickable c)){
                    c.OnColldierClicked();
		        }
	        }
        }
	}
}

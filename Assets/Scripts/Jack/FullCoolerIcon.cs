using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullCoolerIcon : MonoBehaviour
{
	CameraController cam;
	CoolerSlot[] slots;
	RawImage image;

	public float freq;
	public float amp;
    

	private void Start()
	{
		cam = Camera.main.GetComponent<CameraController>();
		slots = (CoolerSlot[])FindObjectsByType(typeof(CoolerSlot), FindObjectsSortMode.None);
		image = GetComponent<RawImage>();
	}
	// Update is called once per frame
	void Update()
    {
		if (cam.trackingHook && CoolerIsFull()) {
			Animate();
			if (!image.enabled)
			{
				image.enabled = true;
			}
		}
		else {
			if (image.enabled)
			{
				image.enabled = false;
			}
		}
    }

	void Animate() {
		transform.Rotate(new Vector3(0, 0, amp * Mathf.Sin(Time.time * freq)));
	}

	bool CoolerIsFull() { 
		foreach(CoolerSlot cs in slots) { 
			if(cs.itemOnStation == null) {
				return false;
			}
		}
		return true;
    }
}

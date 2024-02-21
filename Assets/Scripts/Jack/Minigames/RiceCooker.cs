using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceCooker : Clickable
{
	public bool cooking;
	public bool ready;
	public float riceTimer;
	public float riceCookTime;
	public float riceBurnTime;

	public SpriteRenderer rice;
	
	public override void OnColliderClicked()
	{
		base.OnColliderClicked();
		if (!cooking) {
			riceTimer = 0;
			cooking = true;
			rice.color = Color.grey;
			//start cooking
			
		}
		else if(riceTimer > riceCookTime){
			//give player rice :) 
		}
	}
	private void Update()
	{
		if (cooking)
		{
			riceTimer += Time.deltaTime;
			if (riceTimer > riceCookTime && !ready)
			{
				RiceReady();
			}
			if (riceTimer > riceBurnTime)
			{
				cooking = false;
				RiceBurnt();
			}
		}
	}
	void RiceReady() {
		ready = true;
		rice.color = Color.white;
    }
	void RiceBurnt()
	{
		ready = false;
		rice.color = Color.black;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Station
{
	public Item cutfish;

	public override bool OnItemAdd(Item item)
	{
		Debug.Log(item + " " + item.itemName);
		if (item.itemName == "whole fish") {
			return base.OnItemAdd(cutfish);
		}
		else {
			return false;
		}

	}
}

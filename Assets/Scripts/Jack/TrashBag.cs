using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : Station
{
	public override bool OnItemAdd(Item item)
	{
		return true;
	}
}

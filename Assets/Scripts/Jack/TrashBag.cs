using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : Station
{
	public AudioClip dispose;
	public override bool OnItemAdd(ItemInstance item)
	{
		if (GetComponent<SquashStretch>())
		{
			GetComponent<SquashStretch>().SS(1.2f);
		}
		SoundManager.Instance.PlaySoundEffect(dispose);
		return true;
	}
}

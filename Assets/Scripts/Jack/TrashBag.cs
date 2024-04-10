using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : Station
{
	public AudioClip dispose;
	public override bool OnItemAdd(ItemInstance item)
	{
		SoundManager.Instance.PlaySoundEffect(dispose);
		return true;
	}
}

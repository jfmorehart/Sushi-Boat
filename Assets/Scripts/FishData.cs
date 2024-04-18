using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FishData : ScriptableObject
{
    public int fishIndex;
	public Item fishItem;
    public Sprite fishSprite;

    public float swimSpeed;
    public Vector2 mouthPos;
    public RuntimeAnimatorController animController;
}

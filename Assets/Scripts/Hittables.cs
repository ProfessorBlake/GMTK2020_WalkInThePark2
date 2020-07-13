using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittables : MonoBehaviour
{
	public SpriteRenderer SprRend;
	public Sprite HitSprite;

	private bool _hit;

	public virtual bool GetHit()
	{
		if(!_hit)
		{
			_hit = true;
			SprRend.sprite = HitSprite;
			return true;
		}
		return false;
	}
}

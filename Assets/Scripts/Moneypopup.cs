using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneypopup : MonoBehaviour
{
	private float _time = 1f;

	private void Update()
	{
		transform.position += new Vector3(0f, Time.deltaTime, 0f);
		_time -= Time.deltaTime;
		if(_time <= 0f)
		{
			Destroy(gameObject);
		}
	}
}

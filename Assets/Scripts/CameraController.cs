using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private float _shake;
	private Vector3 _moveTarget;
	private float _shakeMulti = 3f;
	private Vector3 _startPos;

	private void Start()
	{
		_startPos = transform.position;
	}

	private void Update()
	{
		if(_shake > 0f)
		{
			_shake -= Time.deltaTime;
		}

		transform.position = Vector3.Lerp(transform.position, _startPos + new Vector3(Random.Range(-_shake, _shake) * _shakeMulti, Random.Range(-_shake, _shake) * _shakeMulti, 0f), Time.deltaTime * 2f);
	}

	public void AddShake(float f)
	{
		_shake = f;
	}
}

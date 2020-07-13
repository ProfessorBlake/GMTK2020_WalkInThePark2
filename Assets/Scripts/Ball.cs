using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public float MoveSpeed = 1f;
	public Vector2 Velocity;

	private float _moveDelay;
	private float _moveDelayReset;

	private void Awake()
	{
		_moveDelayReset = GameManager._delayReset;
	}

	public void Throw(Vector2 vel)
	{
		Velocity = vel.normalized;
	}

	private void Update()
	{
		_moveDelay -= Time.deltaTime;
		if (_moveDelay <= 0f)
		{
			_moveDelay = _moveDelayReset;
			transform.position += (Vector3)Velocity * MoveSpeed;
		}
	}

	public void Hit()
	{
		GetComponent<Collider2D>().enabled = false;
	}
}

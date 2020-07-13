using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kid : MonoBehaviour
{
	public Ball BallPrefab;

	private bool _dragging;
	private Vector2 _launchVector;

	private void OnMouseDrag()
	{
		_dragging = true;
		_launchVector = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void Update()
	{
		if(Input.GetMouseButtonUp(0))
		{
			if (_dragging)
			{
				Instantiate(BallPrefab, transform.position, Quaternion.identity).Throw(_launchVector);
			}
			_dragging = false;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)_launchVector);
	}
}

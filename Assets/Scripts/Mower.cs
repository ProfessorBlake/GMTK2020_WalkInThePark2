using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Mower : MonoBehaviour
{
	[Header("Tiles")]
	public Tilemap GrassTilemap;
	public Tile CutTile;

	[Header("Movement")]
	public Vector3 MoveDirection = Vector3.down;
	public Vector3 MoveInput = Vector3.down;
	public float MoveMagnitude = 0.2f;
	public Vector2 MoveMagnitudeRange;

	[Header("UI")]
	public TMP_Text MoneyText;
	public TMP_Text MoneyPopupPrefab;
	public TMP_Text PercentCutText;
	public Transform PopupContainer;
	public CameraController Cam;

	[Header("Audio")]
	public AudioSource AS;
	public AudioClip SFX_MowerRun;
	public AudioClip SFX_HitHouse;
	public AudioClip SFX_HitMailbox;
	public AudioClip SFX_HitBall;

	private float _moveDelay;
	private float _moveDelayReset;
	private int _score;
	private int _totalGrass = 999;
	private int _totalCut = 0;

	private void Start()
	{
		_moveDelayReset = GameManager._delayReset;
		BoundsInt bounds = GrassTilemap.cellBounds;
		TileBase[] allTiles = GrassTilemap.GetTilesBlock(bounds);

		for (int x = 0; x < bounds.size.x; x++)
		{
			for (int y = 0; y < bounds.size.y; y++)
			{
				Sprite s = GrassTilemap.GetSprite(new Vector3Int(x, y, 0));
				if (s && s != CutTile.sprite);
				{
					_totalGrass++;
				}
			}
		}
	}

	private void Update()
	{
		//MoveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		//if(MoveInput.sqrMagnitude > 0f)
			//MoveDirection = MoveInput;

		_moveDelay -= Time.deltaTime;
		if(_moveDelay <= 0f)
		{
			_moveDelay = _moveDelayReset;
			MowTile();
			Move();
		}
	}

	private void MowTile()
	{
		for (float x = -0.5f; x <= 0.5f; x+=1)
		{
			for (float y = -0.5f; y <= 0.5f; y += 1)
			{
				int xpos = Mathf.RoundToInt(transform.position.x + x);
				int ypos = Mathf.RoundToInt(transform.position.y + y);
				Debug.DrawLine(transform.position, new Vector3(xpos, ypos));
				Vector3Int pos = new Vector3Int(xpos,ypos,0);
				if (GrassTilemap.GetTile(pos) != CutTile)
				{
					GrassTilemap.SetTile(pos, CutTile);
					AddMoney(2);
					_totalCut++;
					float perc = ((float)_totalCut / (float)_totalGrass);
					PercentCutText.text = ((Mathf.Round(perc * 1000f) / 1000f) * 1000f).ToString() + "%";
				}
			}
		}
	}

	private void Move()
	{
		transform.position += MoveDirection * MoveMagnitude;
		float ang = (Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg) + 90f;

		transform.eulerAngles = new Vector3(0f, 0f, ang);

		MoveDirection = Vector2.Lerp(MoveDirection, new Vector2(Mathf.Sign(Random.Range(-1f, 1f)), Mathf.Sign(Random.Range(-1f, 1f))), 0.02f).normalized * MoveMagnitude;
		MoveMagnitude = Mathf.Lerp(MoveMagnitude, MoveMagnitudeRange.y, 0.025f);
	}

	private void AddMoney(int money)
	{
		_score += money;
		MoneyText.text = "$" + _score.ToString();
		TMP_Text popup = Instantiate(MoneyPopupPrefab, PopupContainer);
		popup.text = "<size=0.7>$</size>" + money.ToString();
		popup.transform.position = transform.position + new Vector3(Random.Range(-0.75f,0.75f), Random.Range(1f, 2f), 0f);
		if (money > 0)
			popup.color = Color.yellow;
		else
			popup.color = Color.red;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Ball ballhit = collision.gameObject.GetComponent<Ball>();
		if (ballhit)
		{
			MoveDirection = Vector3.Lerp(MoveDirection, ballhit.Velocity.normalized, 0.5f);
			ballhit.Hit();
			AS.PlayOneShot(SFX_HitBall, 4f);
		}
		else
		{
			MoveMagnitude = MoveMagnitudeRange.x;
			MoveDirection *= -1f;
			Cam.AddShake(0.5f);

			Wall wall = collision.gameObject.GetComponent<Wall>();
			if (wall)
			{
				return;
			}
			AS.PlayOneShot(SFX_HitHouse, 4f);
			AddMoney(-20);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hittables hit = collision.GetComponent<Hittables>();
		if(hit)
		{
			if(hit.GetHit())
			{
				AddMoney(-50);
				Cam.AddShake(0.75f);
				AS.PlayOneShot(SFX_HitMailbox,4f);
			}
		}
	}
}

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PlayerPhysics))]
public class PlayerControl : MonoBehaviour {

	public float gravity = 30;
	public float speed = 8;
	public float accel = 30;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float jumpFuel = 0;
	private float jumpSpeed = 500;

	private bool isGrown;
	private float growTimer;

	private PlayerPhysics playerPhysics;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (isGrown) {
			growTimer -= Time.deltaTime;
			if (growTimer <=0) {
				isGrown = false;
				transform.localScale = Vector3.one * 1f;
			}
		}

		if (playerPhysics.stopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}

		targetSpeed = speed;
		//targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed,targetSpeed,accel);

		amountToMove.x = currentSpeed;
		amountToMove.y -= playerPhysics.grounded ? 0 : gravity * Time.deltaTime;

		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			jumpFuel = 0.4f;
		}else if (!isButtonHold()) {
			jumpFuel = 0;
		}

		if (isButtonHold() && jumpFuel > 0) {
			float fuelUsed = Mathf.Min(Time.deltaTime, jumpFuel);
			jumpFuel -= fuelUsed;
			amountToMove.y = jumpSpeed * fuelUsed;
		}

		Vector2 delta = playerPhysics.Move(amountToMove * Time.deltaTime);
		
		Animation animation = GetComponentInChildren<Animation>();
		string clipName;
		//Debug.Log("Grounded TEST: " + playerPhysics.grounded);
		if (playerPhysics.grounded) {
			clipName = "PenguinRun"; 
		}else{
			clipName = "PenguinJump"; 
		}
		if (!animation.IsPlaying(clipName)) {
			animation.CrossFade(clipName,0.15F);
			Debug.Log("Animation: " + clipName);
		}
		animation["PenguinRun"].speed = delta.x*10f/transform.lossyScale.x;
		animation["PenguinJump"].speed = delta.x*10f/transform.lossyScale.x;
	}

	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;
		} else {
			float dir = Mathf.Sign(target - n);
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target -n)) ? n:target;
		}
	}
	
	private bool isButtonHold() {
		return (Input.touchCount > 0 || Input.GetButton("Jump") || Input.GetMouseButtonDown(0));
	}
	
	private bool isButtonDown() {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				return true;
			}
		}

		if (Input.GetButtonDown("Jump")) return true;
		return false;
	}

	public void Grow() {
		growTimer = 5;
		isGrown = true;
		transform.localScale = Vector3.one * 2f;
	}
}

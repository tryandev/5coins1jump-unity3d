using UnityEngine;
using System.Collections;

[RequireComponent((typeof(BoxCollider)))]
public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask;

	private BoxCollider boxCollider = null;
	private Vector3 colliderSize;
	private Vector3 colliderCenter;
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool stopped;

	private float skin = 0.001000047f;

	private Ray ray;
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider> ();
		colliderCenter = boxCollider.center;
		colliderSize = boxCollider.size;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 Move(Vector2 moveAmount) {
		Vector3 c = colliderCenter;
		Vector3 s = colliderSize * transform.lossyScale.x;

		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;

		Vector2 p = transform.position;

		grounded = false;
		//Debug.Log("Grounded set to FALSE");
		float x = 0;
		float y = 0;
		float width = 0;//s.y / 2;
		int i = 0;
		for (i = 0; i < 3; i++) {
			float dir = deltaY <= 0 ? -1:1;
			//Debug.Log("deltaY: " + deltaY + " " + dir);
			x = (p.x + c.x - s.x / 2) + s.x / 2 * i;
			y = p.y + c.y + s.y / 2 * dir;
			//y = p.y + c.y;

			ray = new Ray(new Vector2(x,y),new Vector2(0, dir));
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask)) {
				Debug.DrawRay(ray.origin,ray.direction*hit.distance,Color.red);

				float dst = hit.distance - width;
				if (Mathf.Abs(deltaY) >= dst-skin-0.00001f) {
					//Debug.Log("hit.distance: " + hit.distance);
					//Debug.Log("width: " + width);
					//Debug.Log("A) " + deltaY + " = " + "("+dst+"-"+skin+")*"+dir+"");
					deltaY = (dst-skin)*dir;
					//Debug.Log("B) " + deltaY + " = " + "("+dst+"-"+skin+")*"+dir+"");
					grounded = dir <= 0;
					break;
				}else{
					//Debug.Log("Mathf.Abs("+deltaY+") >= "+dst+"-"+skin+"");
					//Debug.Log("Mathf.Abs("+deltaY+") >= "+(dst-skin)+"");
				}
			}
			if (i==3) Debug.Log("3 Grounded set to "+grounded+": " + deltaY +"           y : " + y);
		}
		//Debug.Log("deltaY: " + deltaY);

		stopped = false;
		for (i = 0; i < 3; i++) {
			float dir = Mathf.Sign (deltaX);
			x = p.x + c.x + s.x / 2 * dir;
			y = p.y + c.y - s.y / 2 + s.y / 2 * i;

			ray = new Ray(new Vector3(x,y),new Vector2(dir, 0));
			Debug.DrawRay(ray.origin,ray.direction);

			if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask)) {
				float dst = Vector3.Distance(ray.origin, hit.point);
				
				if (dst > skin){ 
					deltaX = dir * (dst - skin);
				} else {
					deltaX = 0;
				}
				stopped = true;
				break;
			}
			
		}

		if (!stopped && !grounded) {
		Vector3 playerDir = new Vector3(deltaX, deltaY);
		Vector3 o = new Vector3(p.x + c.x + s.x /2 * Mathf.Sign(deltaX), p.y + c.y + s.y / 2 * Mathf.Sign(deltaY));
		ray = new Ray(o,playerDir.normalized);
		Debug.DrawRay(ray.origin,ray.direction);
		if (Physics.Raycast(ray, out hit, Mathf.Sqrt (deltaX * deltaX + deltaY * deltaY), collisionMask)) {
			if (hit.normal == Vector3.up) {
				grounded = true;
				deltaY = 0;
				Debug.Log("Ground");
			}else{
				stopped = true;
				deltaX = 0;
				Debug.Log("Stopped");
			}
		}
		}

		Vector2 finalTransform = new Vector2 (deltaX, deltaY);
		transform.Translate (finalTransform);
		//Debug.Log("deltaXY: " + finalTransform);
		return finalTransform;
	}


}

using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Transform target;
	public float trackSpeed;


	public void SetTarget(Transform t) {
		target = t;
	}

	void FixedUpdate() {
		if (target) {
			float x = LagTowards(transform.position.x, target.position.x+4.5f, trackSpeed);
			float y = LagTowards(transform.position.y, target.position.y+1, trackSpeed);
			transform.position = new Vector3(x,y, transform.position.z);
			Material material = transform.FindChild("Background").gameObject.renderer.material;
			material.SetTextureOffset("_MainTex", new Vector2(x/200, 0));
		}
	}
	
	private float LagTowards(float n, float target, float a) {
		return n + (target - n) * a;
	}
}

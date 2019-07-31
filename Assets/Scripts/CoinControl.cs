using UnityEngine;
using System.Collections;

public class CoinControl : MonoBehaviour {
	
	private Transform model;
	//private Transform glow;
	// Use this for initialization
	void Start () {
		//glow = transform.FindChild("Glow");
		model = transform.FindChild("Model");
	}
	
	// Update is called once per frame
	void Update () {
		model.Rotate(Vector3.up*Time.deltaTime * 100f);
	}

	void OnTriggerEnter(Collider other) {
		if (this.name == "Fish") other.gameObject.SendMessage("Grow");
		Destroy(this.gameObject);
		Debug.Log("ItemCollect - this: " + this.name + ", other: " + other.name);
	}
}

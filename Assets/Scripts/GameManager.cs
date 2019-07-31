using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public GameObject groundPiece;

	private GameCamera cam;
	
	private float pieceNumber = 0;
	private Queue pieceList = new Queue();
	private float pieceSize = 15f;

	void Start () {
		cam = GetComponent<GameCamera> ();
		SpawnPlayer();
		SpawnGround(0);
	}
	
	void SpawnPlayer() {
		player = Instantiate (player, Vector3.zero, Quaternion.identity) as GameObject;
		player.transform.Translate (Vector3.up * 3f);
		//player.transform.localScale = Vector3.one * 3f;
		cam.SetTarget(player.transform);
	}
	
	void SpawnGround(float positionX) {
		GameObject piece = Instantiate (groundPiece, Vector3.zero, Quaternion.identity) as GameObject;
		piece.transform.Translate (Vector3.up * -0.5f - Vector3.left * positionX);
		piece.transform.Translate (Vector3.up * Random.Range(-2.0f, 2.0f));
		pieceList.Enqueue (piece);
		if (pieceList.Count > 5) {
			Destroy(pieceList.Dequeue() as GameObject);
		}
	}

	void Update() {
		float playerX = player.transform.position.x;
		float playerDiscreetX = Mathf.Round(playerX / pieceSize);
		if (pieceNumber < playerDiscreetX + 1) {
			pieceNumber++;
			SpawnGround(pieceNumber * pieceSize);
		}
	}
}

using UnityEngine;
using System.Collections;

public class AvalancheController : MonoBehaviour {

	public GameObject boulderPrefab;
	public GameObject warning;

	public bool startAvalanches = false;

	public float currentHeight;
	public float currentTime;

	// Update is called once per frame
	void Update() {
		if (startAvalanches) {
			currentTime += Time.deltaTime;

			var timeTillBoulder = Mathf.Max(30f - currentHeight * 0.1f, 3f);
			if (currentTime > timeTillBoulder) {

				DropBoulder(1f);
				DropBoulder(2f);
				if (currentHeight > 100) {
					DropBoulder(3f);
				}
				if (currentHeight > 150) {
					DropBoulder(4f);
				}

				currentTime = timeTillBoulder * Random.Range(0.0f, 0.3f);
			}
		}
	}

	public void StopTheRocks() {
		startAvalanches = false;
	}

	public void StartAvalanches() {
		currentHeight = 0;
		currentTime = 0;
		startAvalanches = true;
	}

	public void UpdateHeight(float height) {
		currentHeight = height;
	}

	private void DropBoulder(float offSet) {
		warning.SetActive(true);

		var boulderPosition = Camera.main.transform.position;
		boulderPosition.z = -1.2f;
		boulderPosition.y += 25+ offSet;
		boulderPosition.x = Random.Range(-6f, 6f);

		var newBoulder = GameObject.Instantiate(boulderPrefab, this.transform);
		newBoulder.transform.position = boulderPosition;

		//I release you unto this world
		newBoulder.SetActive(true);

		newBoulder.GetComponentInChildren<Rigidbody>().AddForceAtPosition(
			new Vector3(
				Random.Range(-100f, 100f),
				0,
				0),

			new Vector3(
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f),
				Random.Range(-1f, 1f)));

		newBoulder.GetComponent<Boulder>().Init(OnCollision);
	}

	public void OnCollision(PlayerController pc) {
		pc.KnockedOut();
	}
}

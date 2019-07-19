using UnityEngine;
using System.Collections;

public class RockGenerator : MonoBehaviour {

	private GameObject rockPrefab = null;

	[ContextMenu("Generate Rocks")]
	void GenerateRocks() {
		Debug.Log("Generating Rocks - Start");

		if (rockPrefab == null) {
			rockPrefab = transform.Find("RockPrefab").gameObject;
			rockPrefab.SetActive(false);
		}



		Debug.Log("Generating Rocks - End");
	}
}

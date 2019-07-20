using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	// Use this for initialization
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

	}

	public void SetGraphic() {
		var display = transform.Find("Display");
		var activeIndex = Random.Range(0, display.childCount);
		var index = 0;
		foreach (Transform child in display) {
			if (activeIndex == index) {
				child.gameObject.SetActive(true);
			}
			else {
				child.gameObject.SetActive(false);
			}

			index++;
		}
	}
}

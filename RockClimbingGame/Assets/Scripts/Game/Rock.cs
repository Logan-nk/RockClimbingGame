using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public float weightLimit;
	public float damage = 0f;
	public float damageMax = 300f;

	public MeshRenderer renderer;

	public GameObject display;
	public GameObject breakParticles; 

	public bool CheckDamage(float weight) {
		var diff = weight - weightLimit;
		damage += diff > 0 ? diff : 0;

		renderer.material.color = new Color(0.5f,
			(1.0f - damage / damageMax) * 0.5f,
			(1.0f - damage / damageMax) *0.5f);

		if (damage > damageMax) {
			display.SetActive(false);
			breakParticles.SetActive(true);
			return true;
		}

		return false;
	}

	public void SetGraphic() {
		display = transform.Find("Display").gameObject;
		breakParticles = transform.Find("DustEffect").gameObject;
		var activeIndex = Random.Range(0, display.transform.childCount);
		var index = 0;
		foreach (Transform child in display.transform) {
			if (activeIndex == index) {
				child.gameObject.SetActive(true);
				renderer = child.GetComponentInChildren<MeshRenderer>();
			}
			else {
				child.gameObject.SetActive(false);
			}

			index++;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public float weightLimit;
	public float damage = 0f;
	public float damageMax = 300f;

	public SpriteRenderer sprite;

	public bool CheckDamage(float weight) {
		var diff = weight - weightLimit;
		damage += diff > 0 ? diff : 0;

		sprite.color = new Color(1.0f,
			1.0f - (damage / damageMax),
			1.0f - (damage / damageMax));

		if (damage > damageMax) {
			gameObject.SetActive(false);
			return true;
		}

		return false;
	}

	public void SetGraphic() {
		var display = transform.Find("Display");
		var activeIndex = Random.Range(0, display.childCount);
		var index = 0;
		foreach (Transform child in display) {
			if (activeIndex == index) {
				child.gameObject.SetActive(true);
				sprite = child.GetComponentInChildren<SpriteRenderer>();
			}
			else {
				child.gameObject.SetActive(false);
			}

			index++;
		}
	}
}

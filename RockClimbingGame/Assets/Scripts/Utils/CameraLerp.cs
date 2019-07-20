using UnityEngine;
using System.Collections.Generic;

public class CameraLerp : MonoBehaviour {

	public GameObject characters;

	private List<Player> players;

	public void LateUpdate() {
		if(players == null) {
			players = new List<Player>();
			foreach (var player in characters.GetComponentsInChildren<Player>(true)) {
				players.Add(player);
			}
		}

		var targetPosition = new Vector3();
		var activePlayers = 0;
		foreach (var player in players) {
			if (player.gameObject.activeInHierarchy) {
				targetPosition += player.hip.transform.position;
				activePlayers++;
			}
		}

		if (activePlayers != 0) {
			targetPosition /= activePlayers;

			var pos = transform.position;
			pos.y = Mathf.Lerp(pos.y, targetPosition.y, 0.5f);
			transform.position = pos;
		}
	}
}

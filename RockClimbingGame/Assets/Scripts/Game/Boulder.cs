using UnityEngine;
using System.Collections;
using System;

public class Boulder : MonoBehaviour {

	public Action<PlayerController> onHitEvent;

	public void Init(Action<PlayerController> onHit) {
		onHitEvent = onHit;
	}

	public void OnCollisionEnter(Collision collision) {
		if(collision.rigidbody != null) {
			var rigParent = collision.rigidbody.transform.parent;//Rig
			if(rigParent != null) {
				var modelParent = rigParent.parent;//Model
				if (modelParent != null) {
					var climberParent = modelParent.parent;//Climber
					if (climberParent != null) {
						var controllerParent = climberParent.parent;//Climber Controller
						if (controllerParent != null) {
							var pc = controllerParent.GetComponentInChildren<PlayerController>();
							if (pc != null) {
								onHitEvent?.Invoke(pc);
							}
						}
					}
				}
			}
		}
	}
}

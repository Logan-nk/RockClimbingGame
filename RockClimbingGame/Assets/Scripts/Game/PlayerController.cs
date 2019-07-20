using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
    public Player player;
	public GameObject chainPrefab;

	public float armStrength = 35f;
	public float torsoStrength = 100f;
	public float weight = 100f;
	public float currentWeight = 100f;

	public float maxDistance = 2;
    private Vector3 leftHandStoredPos, rightHandStoredPos;

	private List<PlayerController> tetheredAllies;

    public float controllerNum = 1;

    float hAxis;
    float vAxis;
    bool leftLegControl;
    bool rightLegControl;
    bool leftHandControl;
    bool rightHandControl;

	bool lockControls = false;

	bool isHoldingLeftHand, isHoldingRightHand, isHoldingLeftLeg, isHoldingRightLeg;

	public void LockInPlace() {
		UpdateInput();

		lockControls = true;
	}

	public void UnlockAndTether() {
		lockControls = false;

		var parent = this.transform.parent.parent;
		foreach(var controller in parent.GetComponentsInChildren<PlayerController>()) {
			if(controller.controllerNum == controllerNum + 1 && controller.gameObject.activeInHierarchy) {
				//attach link
				AddTetheredPlayer(controller);
				controller.AddTetheredPlayer(this);

				var chain = GameObject.Instantiate(chainPrefab, player.hip.transform);

				var start = chain.transform.FindDeepChild("CarabinerStart");
				var hinge = controller.player.hip.gameObject.AddComponent<HingeJoint>();
				hinge.connectedBody = start.GetComponent<Rigidbody>();
				hinge.autoConfigureConnectedAnchor = false;
				hinge.connectedAnchor = new Vector3(0.5f, 0, 0);

				var end = chain.transform.FindDeepChild("CarabinerEnd");
				var endHinge = end.GetComponent<HingeJoint>();
				endHinge.connectedBody = player.hip;
				endHinge.autoConfigureConnectedAnchor = false;
				endHinge.connectedAnchor = new Vector3(-0.5f, 0, 0);

				break;
			}
		}
	}

	public void AddTetheredPlayer(PlayerController player) {
		if(tetheredAllies == null) {
			tetheredAllies = new List<PlayerController>();
		}

		tetheredAllies.Add(player);
	}

	private void ResetPlayer() {
        //Set player start position

        isHoldingLeftHand = false;
        isHoldingRightHand = false;
        isHoldingLeftLeg = false;
        isHoldingRightLeg = false;
    }

    private void UpdateInput() {
		if (lockControls) return;

        leftLegControl = Input.GetAxis("LeftLeg_" + controllerNum) > 0;
        rightLegControl = Input.GetAxis("RightLeg_" + controllerNum) > 0;
        hAxis = Input.GetAxis("Horizontal_" + controllerNum);
        vAxis = Input.GetAxis("Vertical_" + controllerNum);
        leftHandControl = Input.GetButton("LeftHand_" + controllerNum);
        rightHandControl = Input.GetButton("RightHand_" + controllerNum);
    }

    private void UpdateGripControls() {
        if (isHoldingLeftHand != leftHandControl) {
            Debug.Log("Left Hand Grip: " + leftHandControl);
            isHoldingLeftHand = leftHandControl;
            if (!isHoldingLeftHand) {
				//player.leftHand.connectedBody = leftHand;
				player.LetGoRockLeftHand();
			}
            else {
                isHoldingLeftHand = player.TryGrabRockLeftHand();
            }
        }

        if (isHoldingRightHand != rightHandControl) {
            Debug.Log("Right Hand Grip: " + rightHandControl);
            isHoldingRightHand = rightHandControl;
            if (!isHoldingRightHand) {
				// player.rightHand.connectedBody = rightHand;
				player.LetGoRockRightHand();
			}
            else {
               isHoldingRightHand = player.TryGrabRockRightHand();
            }
        }

		if (isHoldingLeftLeg != leftLegControl) {
			Debug.Log("Left Leg Grip: " + leftLegControl);
			isHoldingLeftLeg = leftLegControl;
			if (!isHoldingLeftLeg) {
				player.LetGoRockLeftLeg();
			}
			else {
				isHoldingLeftLeg = player.TryGrabRockLeftLeg();
			}
		}

		if (isHoldingRightLeg != rightLegControl) {
			Debug.Log("Right Leg Grip: " + rightLegControl);
			isHoldingRightLeg = rightLegControl;
			if (!isHoldingRightLeg) {
				player.LetGoRockRightLeg();
			}
			else {
				isHoldingRightLeg = player.TryGrabRockRightLeg();
			}
		}
	}

    private void UpdateControlPositions() {
		if(isHoldingLeftHand || isHoldingRightHand || isHoldingLeftLeg || isHoldingRightLeg) {
			player.torso.AddForce(new Vector3(hAxis * torsoStrength, -vAxis * torsoStrength, 10));
		}

        if (!isHoldingLeftHand) {
			player.leftHand.AddForce(new Vector3(hAxis* armStrength, -vAxis* armStrength, 5));
        }

        if (!isHoldingRightHand) {
			player.rightHand.AddForce(new Vector3(hAxis* armStrength, -vAxis* armStrength, 5));
		}

		if (!isHoldingLeftLeg) {
			player.leftLeg.AddForce(new Vector3(hAxis * armStrength, -vAxis * armStrength, 5));
		}

		if (!isHoldingRightLeg) {
			player.rightLeg.AddForce(new Vector3(hAxis * armStrength, -vAxis * armStrength, 5));
		}

	}

	public void UpdateRockDamage() {
		var currentWeight = CalculateCurrentWeight();
	
		if (isHoldingLeftHand) {
			var rock = player.rockManager.GetClosestRockToPoint(player.leftHand.transform.position, 0.15f);
			if (rock == null || rock.CheckDamage(currentWeight)) {
				isHoldingLeftHand = false;
				player.LetGoRockLeftHand();
			}
		}

		if (isHoldingRightHand) {
			var rock = player.rockManager.GetClosestRockToPoint(player.rightHand.transform.position, 0.15f);
			if (rock == null || rock.CheckDamage(currentWeight)) {
				isHoldingRightHand = false;
				player.LetGoRockRightHand();
			}
		}

		if (isHoldingLeftLeg) {
			var rock = player.rockManager.GetClosestRockToPoint(player.leftLeg.transform.position, 0.15f);
			if (rock == null || rock.CheckDamage(currentWeight)) {
				isHoldingLeftLeg = false;
				player.LetGoRockLeftLeg();
			}
		}

		if (isHoldingRightLeg) {
			var rock = player.rockManager.GetClosestRockToPoint(player.rightLeg.transform.position, 0.15f);
			if (rock == null || rock.CheckDamage(currentWeight)) {
				isHoldingRightLeg = false;
				player.LetGoRockRightLeg();
			}
		}
	}

	private float CalculateCurrentWeight() {
		var stableLimbs = 0;
		if (isHoldingLeftHand) stableLimbs++;
		if (isHoldingRightHand) stableLimbs++;
		if (isHoldingLeftLeg) stableLimbs++;
		if (isHoldingRightLeg) stableLimbs++;

		var targetWeight = weight;
		var allyWeight = 0f;
		var allySupport = false;
		if (tetheredAllies != null) {
			foreach (var ally in tetheredAllies) {
				if (ally == this) {
					continue;
				}

				if (ally.isHoldingLeftHand || ally.isHoldingRightHand || ally.isHoldingLeftLeg || ally.isHoldingRightLeg) {
					continue;
				}

				Debug.Log("supporting 1 Ally");
				allyWeight += ally.weight;

				foreach (var ally2 in ally.tetheredAllies) {
					if (ally2 == ally) {
						continue;
					}
					if (ally2.isHoldingLeftHand || ally2.isHoldingRightHand || ally2.isHoldingLeftLeg || ally2.isHoldingRightLeg) {
						allySupport = true;
						continue;
					}

					Debug.Log("supporting 2 Allies");
					allyWeight += ally2.weight;

					foreach (var ally3 in ally2.tetheredAllies) {
						if (ally3 == ally2) {
							continue;
						}
						if (ally3.isHoldingLeftHand || ally3.isHoldingRightHand || ally3.isHoldingLeftLeg || ally3.isHoldingRightLeg) {
							allySupport = true;
							continue;
						}

						Debug.Log("supporting 3 Allies");
						allyWeight += ally3.weight;
					}
				}
			}
		}

		targetWeight += (allySupport ? allyWeight * 0.5f : allyWeight);

		currentWeight = currentWeight > targetWeight ? targetWeight : currentWeight += 0.5f;

		return currentWeight / stableLimbs;
	}

    // Update is called once per frame
    void LateUpdate() {
        UpdateInput();

        UpdateGripControls();

        UpdateControlPositions();

		UpdateRockDamage();
    }

    //Hand_Jnt_lh
    //Hand_Jnt_rh
}

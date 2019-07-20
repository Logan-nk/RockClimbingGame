using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
    public Player player;

	public float armStrength = 35f;
	public float torsoStrength = 100f;

	public float maxDistance = 2;
    private Vector3 leftHandStoredPos, rightHandStoredPos;

    public float controllerNum = 1;

    float hAxis;
    float vAxis;
    bool leftLegControl;
    bool rightLegControl;
    bool leftHandControl;
    bool rightHandControl;

    bool isHoldingLeftHand, isHoldingRightHand, isHoldingLeftLeg, isHoldingRightLeg;

    private void ResetPlayer() {
        //Set player start position

        isHoldingLeftHand = false;
        isHoldingRightHand = false;
        isHoldingLeftLeg = false;
        isHoldingRightLeg = false;
    }

    private void UpdateInput() {
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

    // Update is called once per frame
    void Update() {
        UpdateInput();

        UpdateGripControls();

        UpdateControlPositions();
    }

    //Hand_Jnt_lh
    //Hand_Jnt_rh
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Rigidbody leftHand, rightHand, leftLeg, rightLeg;
	public RockGenerator rockManager;

    public void SetLeftHandPos(float pos) {
        //leftHand
    }

    public void SetRightHandPos(float pos) {

    }

    public bool CanGrabRock() {
        var value = false;
        return value;
    }

    public void TryGrabRockLeftHand() {

		leftHand.isKinematic = true;

		Debug.Log("Tried to grab rock at: " + leftHand.transform.position);
		var rock = rockManager.GetClosestRockToPoint(leftHand.transform.position);

		if (rock != null) {

		}
	}

	public void LetGoRockLeftHand() {
		leftHand.isKinematic = false;
	}

    public void TryGrabRockRightHand() {

		rightHand.isKinematic = true;

		Debug.Log("Tried to grab rock at: " + rightHand.transform.position);

		var rock = rockManager.GetClosestRockToPoint(rightHand.transform.position);

		if (rock != null) {

		}
	}

	public void LetGoRockRightHand() {
		rightHand.isKinematic = false;
	}

	public void TryGrabRockLeftLeg() {

    }

    public void TryGrabRockRightLeg() {

    }
}

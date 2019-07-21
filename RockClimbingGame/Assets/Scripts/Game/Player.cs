using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Rigidbody leftHand, rightHand, leftLeg, rightLeg, torso, hip;
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

    public bool TryGrabRockLeftHand() {
		Debug.Log("Tried to grab rock at: " + leftHand.transform.position);
		var rock = rockManager.GetClosestRockToPoint(leftHand.transform.position, 0.1f);

		if (rock != null) {
			leftHand.isKinematic = true;
			return true;
		}

		return false;
	}

	public void LetGoRockLeftHand() {
		leftHand.isKinematic = false;
	}

    public bool TryGrabRockRightHand() {
		Debug.Log("Tried to grab rock at: " + rightHand.transform.position);
		var rock = rockManager.GetClosestRockToPoint(rightHand.transform.position, 0.1f);

		if (rock != null) {
			rightHand.isKinematic = true;
			return true;
		}

		return false;
	}

	public void LetGoRockRightHand() {
		rightHand.isKinematic = false;
	}

	public bool TryGrabRockLeftLeg() {
		Debug.Log("Tried to grab rock at: " + leftLeg.transform.position);
		var rock = rockManager.GetClosestRockToPoint(leftLeg.transform.position, 0.1f);

		if (rock != null) {
			leftLeg.isKinematic = true;
			return true;
		}

		return false;
	}

	public void LetGoRockLeftLeg() {
		leftLeg.isKinematic = false;
	}

	public bool TryGrabRockRightLeg() {
		Debug.Log("Tried to grab rock at: " + rightLeg.transform.position);
		var rock = rockManager.GetClosestRockToPoint(rightLeg.transform.position, 0.1f);

		if (rock != null) {
			rightLeg.isKinematic = true;
			return true;
		}

		return false;
	}

	public void LetGoRockRightLeg() {
		rightLeg.isKinematic = false;
	}
}

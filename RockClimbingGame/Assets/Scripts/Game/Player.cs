using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public SpringJoint leftHand, rightHand, leftLeg, rightLeg;

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
        Debug.Log("Tried to grab rock at: " + leftHand.transform.position);
    }

    public void TryGrabRockRightHand() {
        Debug.Log("Tried to grab rock at: " + rightHand.transform.position);
    }

    public void TryGrabRockLeftLeg() {

    }

    public void TryGrabRockRightLeg() {

    }
}

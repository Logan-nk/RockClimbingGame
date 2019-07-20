using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody leftHand, rightHand, leftLeg, rightLeg;
    public Player player;

    public float maxDistance = 2;
    private Vector3 leftHandStoredPos, rightHandStoredPos;


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
        leftLegControl = Input.GetAxis("LeftLeg") > 0;
        rightLegControl = Input.GetAxis("RightLeg") > 0;
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        leftHandControl = Input.GetButton("LeftHand");
        rightHandControl = Input.GetButton("RightHand");
    }

    private void UpdateGripControls() {
        if (isHoldingLeftHand != leftHandControl) {
            Debug.Log("Left Hand Grip: " + leftHandControl);
            isHoldingLeftHand = leftHandControl;
            leftHand.transform.position = new Vector3(
                player.leftHand.transform.position.x,
                player.leftHand.transform.position.y,
                0);
            leftHandStoredPos = leftHand.transform.position;
            if (!isHoldingLeftHand) {
                player.leftHand.connectedBody = leftHand;
            }
            else {
                player.TryGrabRockLeftHand();
            }
        }

        if (isHoldingRightHand != rightHandControl) {
            Debug.Log("Right Hand Grip: " + rightHandControl);
            isHoldingRightHand = rightHandControl;

            rightHand.transform.position = new Vector3(
                player.rightHand.transform.position.x,
                player.rightHand.transform.position.y,
                0);
            rightHandStoredPos = rightHand.transform.position;
            if (!isHoldingRightHand) {
                player.rightHand.connectedBody = rightHand;
            }
            else {
                player.TryGrabRockRightHand();
            }
        }

        //if (isHoldingLeftLeg != leftLegControl) {

        //}

        //if (isHoldingRightLeg != rightLegControl) {

        //}
    }

    private void UpdateControlPositions() {
        if (!isHoldingLeftHand) {
            var xPos = leftHandStoredPos.x + (hAxis);
            var yPos = leftHandStoredPos.y + (-vAxis);

            leftHand.transform.position = new Vector3(
               xPos,
                yPos,
                0);
        }

        if (!isHoldingRightHand) {
            var xPos = rightHandStoredPos.x + (hAxis);
            var yPos = rightHandStoredPos.y + (-vAxis);

            rightHand.transform.position = new Vector3(
               xPos,
                yPos,
                0);
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

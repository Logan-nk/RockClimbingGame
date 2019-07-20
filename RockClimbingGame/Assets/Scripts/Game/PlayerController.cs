using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody leftHand, rightHand, leftLeg, rightLeg;

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

    // Update is called once per frame
    void Update() {
        UpdateInput();

        if(isHoldingLeftHand != leftHandControl) {

        }

        if (isHoldingRightHand != rightHandControl) {

        }

        if (isHoldingLeftLeg != leftLegControl) {

        }

        if (isHoldingRightLeg != rightLegControl) {

        }


    }

    //Hand_Jnt_lh
    //Hand_Jnt_rh
}

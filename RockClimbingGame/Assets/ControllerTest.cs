using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTest : MonoBehaviour {

    float ltaxis;
    float rtaxis;
    float hAxis;
    float vAxis;
    bool xbox_lb;
    bool xbox_rb;

    public Text debugText;

    private void Start() {
        ltaxis = Input.GetAxis("LeftLeg");
        rtaxis = Input.GetAxis("RightLeg");
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        xbox_lb = Input.GetButton("LeftHand");
        xbox_rb = Input.GetButton("RightHand");
    }

    void ControllerCheck() {

        debugText.text =
            string.Format(
                "LTrigger: {0:0.000} RTrigger: {1:0.000}\n" +
                "Horizontal: {2:0.000} Vertical: {3:0.000}\n" +
                "LB: {4} RB: {5}",
                ltaxis, rtaxis,
                xbox_lb, xbox_rb,
                hAxis, vAxis);
    }

    void Update() {
        // Camera Rig Movement Control

        ControllerCheck();
    }
}




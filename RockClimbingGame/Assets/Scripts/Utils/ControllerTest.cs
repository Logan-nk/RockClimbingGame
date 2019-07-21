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

    private void Start() {
        
    }

    void ControllerCheck() {
		ltaxis = Input.GetAxis("LeftLeg");
		rtaxis = Input.GetAxis("RightLeg");
		hAxis = Input.GetAxis("Horizontal");
		vAxis = Input.GetAxis("Vertical");
		xbox_lb = Input.GetButton("LeftHand");
		xbox_rb = Input.GetButton("RightHand");


		Debug.Log(string.Format(
			"LeftLeg{0:0.00} RightLeg{1:0.00} HAxis {2:0.00} VAxis {3:0.00} LB {4} RB {5}", 
			ltaxis, rtaxis, hAxis, vAxis, xbox_lb, xbox_rb));
    }

    void Update() {

        ControllerCheck();
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    bool initialised = false;

    List<int> waitingForPlayers;
    Dictionary<int, int> playerLookup;
    int playerCount = 0;

    bool leftLegControl;
    bool rightLegControl;
    bool leftHandControl;
    bool rightHandControl;

    bool submit;

    Animator uiAnimator;

    private void BeginClimb() {
        foreach (var player in waitingForPlayers) {
            FindUtil.Child(this.transform, "Climber" + player).gameObject.SetActive(false);
        }

        uiAnimator.SetTrigger("StartClimb");
    }

    private bool CheckInput(int index) {
        leftLegControl = Input.GetAxis("LeftLeg_" + index) > 0;
        rightLegControl = Input.GetAxis("RightLeg_" + index) > 0;
        leftHandControl = Input.GetButton("LeftHand_" + index);
        rightHandControl = Input.GetButton("RightHand_" + index);

        return leftHandControl && leftLegControl && rightHandControl && rightLegControl;
    }

    private void PositionPlayer(int player) {
        ++playerCount;
        playerLookup.Add(playerCount, player);

        var transform = FindUtil.Child(this.transform, "Player" + playerCount + "StartPos");
        var climber = FindUtil.Child(this.transform, "Climber" + player);
        climber.gameObject.SetActive(true);
        climber.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z);

        //attach to previous player
        var container = FindUtil.Child(this.transform, "Player" + playerCount);
        FindUtil.Child(container, "JoinMessage").gameObject.SetActive(false);
    }

    private void Init() {
        waitingForPlayers = new List<int> { 1, 2, 3, 4 };
        playerLookup = new Dictionary<int, int>();
        //Reset positions & ui
        foreach (var player in waitingForPlayers) {
            var climber = FindUtil.Child(this.transform, "Climber" + player);
            climber.gameObject.SetActive(false);

            var container = FindUtil.Child(this.transform, "Player" + player);
            FindUtil.Child(container, "JoinMessage").gameObject.SetActive(true);
        }

        uiAnimator.SetTrigger("NewGame");
        initialised = true;
    }

    private void Restart() {
        //tear down
        Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    void Start() {
        uiAnimator = FindUtil.Child<Animator>(this.transform, "Ui");
        Init();
    }

    // Update is called once per frame
    void Update() {
        if (!initialised) return;
        var remove = 0;
        foreach(var player in waitingForPlayers) {
            if (CheckInput(player)) {
                PositionPlayer(player);
                remove = player;
            }
        }

        if(playerCount > 1) {
            if (playerCount > 1)
                submit = Input.GetButton("Submit");
            if (submit) BeginClimb();
        }

        if(remove != 0) {
            waitingForPlayers.Remove(remove);
        }
    }
}

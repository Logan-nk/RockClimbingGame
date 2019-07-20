using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public WallManager wallManager;
    int heightReached;


    bool initialised = false;
    bool climbStarted = false;

    List<int> waitingForPlayers;
    Dictionary<int, int> playerLookup;
    Dictionary<int, Player> playerModelLookup;
    int playerCount = 0;
    Animator uiAnimator;

    bool leftLegControl;
    bool rightLegControl;
    bool leftHandControl;
    bool rightHandControl;
    bool submit;

    private void BeginClimb() {
        foreach (var player in waitingForPlayers) {
            FindUtil.Child(this.transform, "Climber" + player).gameObject.SetActive(false);
        }

        uiAnimator.SetTrigger("StartClimb");
        climbStarted = true;
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

        playerModelLookup.Add(playerCount, FindUtil.Child<Player>(climber, "RockClimberBlueModel"));

        //attach to previous player
        var container = FindUtil.Child(this.transform, "Player" + playerCount);
        FindUtil.Child(container, "JoinMessage").gameObject.SetActive(false);
    }

    private void CheckScore() {
        var highest = 0f;
        foreach(var player in playerModelLookup.Keys) {
            highest = Mathf.Max((playerModelLookup[player].hip.transform.position.y - 7), highest);
        }

        Debug.Log(highest);
        heightReached = Mathf.RoundToInt(highest);

    }

    private void Init() {
        waitingForPlayers = new List<int> { 1, 2, 3, 4 };
        playerLookup = new Dictionary<int, int>();
        playerModelLookup = new Dictionary<int, Player>();
        //Reset positions & ui
        foreach (var player in waitingForPlayers) {
            var climber = FindUtil.Child(this.transform, "Climber" + player);
            climber.gameObject.SetActive(false);

            var container = FindUtil.Child(this.transform, "Player" + player);
            FindUtil.Child(container, "JoinMessage").gameObject.SetActive(true);
        }

        uiAnimator.SetTrigger("NewGame");
        initialised = true;
        wallManager.NewLevel();
    }

    private void Restart() {
        //tear down
        Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        climbStarted = false;
        heightReached = 0;
    }

    void Start() {
        uiAnimator = FindUtil.Child<Animator>(this.transform, "Ui");
        Init();
    }

    // Update is called once per frame
    void Update() {
        if (!initialised) return;
        if (!climbStarted) {
            var remove = 0;
            foreach (var player in waitingForPlayers) {
                if (CheckInput(player)) {
                    PositionPlayer(player);
                    remove = player;
                }
            }

            if (playerCount > 1) {
                if (playerCount > 1)
                    submit = Input.GetButton("Submit");
                if (submit) BeginClimb();
            }

            if (remove != 0) {
                waitingForPlayers.Remove(remove);
            }
        }
        else {
            CheckScore();
            if(heightReached >= wallManager.wallHeight * wallManager.wallCount) {
                wallManager.AddWall();
            }
        }
    }
}

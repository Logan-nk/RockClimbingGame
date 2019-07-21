using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public WallManager wallManager;
    int heightReached;

    CustomButton restartButton;
    CustomButton quitButton;
    TextMeshProUGUI scoreText;

    bool initialised = false;
    bool climbStarted = false;

    List<int> waitingForPlayers;
    Dictionary<int, int> playerLookup;
    Dictionary<int, PlayerController> playerModelLookup;
    int playerCount = 0;
    Animator uiAnimator;

    bool leftLegControl;
    bool rightLegControl;
    bool leftHandControl;
    bool rightHandControl;
    bool submit;
    float vAxis;

    private void BeginClimb() {
        foreach (var player in waitingForPlayers) {
            FindUtil.Child(this.transform, "Climber" + player).gameObject.SetActive(false);
        }

        waitingForPlayers = new List<int>();

        foreach (var controller in playerModelLookup) {
            controller.Value.UnlockAndTether();
        }

        uiAnimator.SetTrigger("StartClimb");
        climbStarted = true;
        Camera.main.GetComponent<CameraLerp>().enabled = true;
        scoreText.gameObject.SetActive(true);
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

        playerModelLookup.Add(playerCount, FindUtil.Child<PlayerController>(climber, "DummyController"));

        //attach to previous player
        var container = FindUtil.Child(this.transform, "Player" + playerCount);
        FindUtil.Child(container, "JoinMessage").gameObject.SetActive(false);

        playerModelLookup[playerCount].LockInPlace();
    }

    private void CheckScore() {
        var highest = 0f;
        foreach (var player in playerModelLookup.Keys) {
            highest = Mathf.Max((playerModelLookup[player].player.hip.transform.position.y - 7), highest);
        }

        Debug.Log(highest);
        heightReached = Mathf.RoundToInt(highest);
        scoreText.text = heightReached.ToString();

    }

    private bool CheckGameNotOver() {
        var cameraHeight = Camera.main.transform.position.y - 5;

        foreach (var player in playerModelLookup.Keys) {
            if (playerModelLookup[player].player.hip.transform.position.y > cameraHeight) return true;
        }

        return false;
    }

    private void GameOver() {
        Camera.main.GetComponent<CameraLerp>().enabled = false;
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x,
            10,
            Camera.main.transform.position.z);

        uiAnimator.SetTrigger("GameOver");
        climbStarted = false;
        playerCount = 0;

        foreach (var player in playerModelLookup.Keys) {
            var sound = FindUtil.Child<AudioSource>(this.transform, "Climber" + player);
            sound.Play();
        }
    }

    private void Init() {
        scoreText.gameObject.SetActive(false);
        waitingForPlayers = new List<int> { 1, 2, 3, 4 };
        playerLookup = new Dictionary<int, int>();
        playerModelLookup = new Dictionary<int, PlayerController>();
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
        SceneManager.LoadScene("LoadingScene");
        CustomButton.ClearGroup("buttons");
        climbStarted = false;
        heightReached = 0;
    }

    private void CheckMenuInput() {
        vAxis = Input.GetAxis("Vertical_1") + Input.GetAxis("Vertical_2")
            + Input.GetAxis("Vertical_4") + Input.GetAxis("Vertical_3");

        submit = Input.GetButton("Submit");

        if (vAxis <= 0) {
            if(!restartButton.GetIsFocused())
                restartButton.Focus();
            if (submit) {
                Restart();
                submit = false;
            }
        }
        else {
            if (!quitButton.GetIsFocused())
                quitButton.Focus();
            if (submit) {
                submit = false;
                Application.Quit();
            }
        }


    }

    void Start() {
        uiAnimator = FindUtil.Child<Animator>(this.transform, "Ui");
        scoreText = FindUtil.Child<TextMeshProUGUI>(this.transform, "Score");
        Init();

        restartButton = FindUtil.Child<CustomButton>(this.transform, "RestartButton");
        restartButton.SetFocusableState(ButtonTransitionStyle.Highlight);
        restartButton.AddToGroup("buttons");
        quitButton = FindUtil.Child<CustomButton>(this.transform, "QuitButton");
        quitButton.SetFocusableState(ButtonTransitionStyle.Highlight);
        quitButton.AddToGroup("buttons");

        
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

            if (waitingForPlayers.Count == 0) {
                CheckMenuInput();
            }

            if (playerCount > 1) {
                if (playerCount > 1)
                    submit = Input.GetButton("Submit");
                if (submit) {
                    BeginClimb();
                    submit = false;
                    return;
                }
            }

            if (remove != 0) {
                waitingForPlayers.Remove(remove);
            }


        }
        else {
            CheckScore();
            if (heightReached >= wallManager.wallHeight * wallManager.wallCount) {
                wallManager.AddWall();
            }
            if (!CheckGameNotOver()) GameOver();
        }
    }
}

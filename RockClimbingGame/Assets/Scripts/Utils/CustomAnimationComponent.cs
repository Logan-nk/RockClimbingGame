using UnityEngine;

public abstract class CustomAnimationComponent : MonoBehaviour {

    public GameObject target;

    public float duration = 1;

    public AnimationCurve curve;

    public bool isPlaying = true;
    public bool loop = true;
    public bool disableOnEnd = false;
    public bool disableObjectOnEnd = false;
    public bool stopPlayingOnEnd = false;
    public bool resetOnEnable = true;

    private float elapsed = 0;
    public float speedMultiplier = 1;

    void Start() {

    }

    void Update() {
        if (!isPlaying) return;

        if (target == null) {
            Debug.LogError("Custom Animator had no target. Disabling: "
                + AnimationUtils.GetHeirarchyPathToGameObject(gameObject));
            enabled = false;
            return;
        }

        elapsed += (Time.deltaTime / duration) * speedMultiplier;

        if (!loop && elapsed > 1.0f) {
            UpdateAnimation(1f);
            if (disableObjectOnEnd) {
                gameObject.SetActive(false);
            }
            if (disableOnEnd) {
                enabled = false;
            }
            else if (stopPlayingOnEnd) {
                isPlaying = false;
            }
            return;
        }

        var currentTime = (elapsed % 1.0f);

        UpdateAnimation(currentTime);
    }

    public void Trigger() {
        Reset();
        isPlaying = true;
    }

    public abstract void UpdateAnimation(float currentTime);

    protected virtual void Reset() {
        elapsed = 0;
        isPlaying = true;
    }

    // we want to reset on enable
    public virtual void OnEnable() {
        if (resetOnEnable) {
            Reset();
        }
    }
}

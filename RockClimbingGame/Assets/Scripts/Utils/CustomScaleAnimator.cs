using UnityEngine;


public class CustomScaleAnimator : CustomAnimationComponent {

    public override void UpdateAnimation(float currentTime) {
        var scale = curve.Evaluate(currentTime);
        target.transform.localScale = Vector3.one * scale;
    }
}


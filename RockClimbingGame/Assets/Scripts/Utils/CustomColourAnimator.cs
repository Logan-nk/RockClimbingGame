using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomColourAnimator : CustomAnimationComponent {

    public Color color1 = Color.white;
    public Color color2 = Color.white;

    SpriteRenderer targetSprite;
    Image img;
    TextMeshProUGUI text;

    private void Start() {
        targetSprite = target.GetComponent<SpriteRenderer>();
        img = target.GetComponent<Image>();
        text = target.GetComponent<TextMeshProUGUI>();
    }

    public override void UpdateAnimation(float currentTime) {
        var curveValue = curve.Evaluate(currentTime);
        var col = Color.Lerp(color1, color2, curveValue);
        if (targetSprite != null) {
            targetSprite.color = col;
        }
        else if (img != null) {
            img.color = col;
        }
        else if (text != null) {
            text.color = col;
        }
        else {
            Debug.LogError("CustomColorAnimator: Missing color target");
        }
    }
}


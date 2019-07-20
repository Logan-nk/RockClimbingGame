using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


    public enum ButtonTransitionStyle {
        Swap,
        Highlight,
        Lock
    }

public class CustomButton : Button {

    Action onHold;
    Action onRelease;
    Action onClick;

    public GameObject active;
    public GameObject inactive;
    public GameObject locked;
    public GameObject highlighted;

    float delay = 0;
    bool pressed;

    ButtonTransitionStyle style;

    string groupId = null;

    private bool isFocused;

    static Dictionary<string, List<CustomButton>> buttonGroups = new Dictionary<string, List<CustomButton>>();

    public void SetOnClick(Action onClick) {
        this.onClick = onClick;
    }

    public void SetHoldAndRelease(Action onHold, Action onRelease) {
        this.onHold = onHold;
        this.onRelease = onRelease;
    }

    override public void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        pressed = true;
    }

    override public void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        onRelease?.Invoke();
        pressed = false;
    }

    public void SetFocusableState(ButtonTransitionStyle style) {
        this.style = style;

        switch (style) {
            case ButtonTransitionStyle.Swap:
                active = FindUtil.Child(transform, "ButtonActive").gameObject;
                inactive = FindUtil.Child(transform, "ButtonInactive").gameObject;
                break;
            case ButtonTransitionStyle.Highlight:
                highlighted = FindUtil.Child(transform, "Highlighted").gameObject;
                break;
            case ButtonTransitionStyle.Lock:
                locked = FindUtil.Child(transform, "ButtonLocked").gameObject;
                break;
        }

    }

    public bool GetIsFocused() {
        return isFocused;
    }

    public void AddToGroup(string groupId) {

        this.groupId = groupId;

        List<CustomButton> group;
        bool hasGroup = buttonGroups.TryGetValue(groupId, out group);

        if (!hasGroup) group = buttonGroups[groupId] = new List<CustomButton>();

        group.Add(this);
    }

    public void Focus() {
        if (groupId != null) {
            HideButtonGroup(groupId);
        }
        switch (style) {
            case ButtonTransitionStyle.Swap:
                // CHANGE STATE
                active.SetActive(true);
                inactive.SetActive(false);
                isFocused = true;
                break;
            case ButtonTransitionStyle.Highlight:
                highlighted.SetActive(true);
                isFocused = true;
                break;
        }
    }

    //for buttons not in a group
    public void Unfocus() {
        switch (style) {
            case ButtonTransitionStyle.Swap:
                // CHANGE STATE
                active.SetActive(false);
                inactive.SetActive(true);
                isFocused = false;

                break;
            case ButtonTransitionStyle.Highlight:
                highlighted.SetActive(false);
                isFocused = false;
                break;
        }
    }

    // we need to be able to clear a whole group when unloading a ui view
    public static void ClearGroup(string groupId) {
        buttonGroups.Remove(groupId);

    }

    public static void HideButtonGroup(string groupId) {

        List<CustomButton> group;
        buttonGroups.TryGetValue(groupId, out group);

        foreach (var button in group) {
            switch (button.style) {
                case ButtonTransitionStyle.Swap:
                    button.active.SetActive(false);
                    button.inactive.SetActive(true);
                    button.isFocused = false;
                    break;
                case ButtonTransitionStyle.Highlight:
                    button.highlighted.SetActive(false);
                    button.isFocused = false;
                    break;
            }
        }
    }

    public void SetLocked(bool isLocked) {
        locked.SetActive(isLocked);
    }

    private void Update() {
        if (pressed) {
            delay += Time.deltaTime;
            if (delay > 0.2) {
                onHold?.Invoke();
                pressed = false;
            }
        }
        else {
            if (delay > 0 && delay < 0.2) {
                onClick?.Invoke();
            }
            delay = 0;
        }
    }

    // null is valid
    private TMP_Text GetText(string name) {
        var container = FindUtil.Child(transform, name, true);
        if (container != null) {
            var txt = FindUtil.Child(container, "Text", true);
            if (txt != null) {
                return txt.GetComponent<TMP_Text>();
            }
        }
        return null;
    }
    /*
    public void Localize(LocalizedText.FormatTextHandler formatText) {
        GetText("ButtonActive")?.Localize(formatText);
        GetText("ButtonInactive")?.Localize(formatText);
        GetText("Highlighted")?.Localize(formatText);
        GetText("ButtonLocked")?.Localize(formatText);
        var txt = FindUtil.Child(transform, "Text", true);
        if (txt != null) {
            txt.GetComponent<TMP_Text>().Localize(formatText);
        }
        // objects only set on state changed
        //GetText(gameObject)?.Localize(formatText);
        //GetText(active)?.Localize(formatText);
        //GetText(inactive)?.Localize(formatText);
        //GetText(locked)?.Localize(formatText);
        //GetText(highlighted)?.Localize(formatText);
    }
    
    public void Localize(string key, params object[] args) {
        GetText("ButtonActive")?.Localize(key, args);
        GetText("ButtonInactive")?.Localize(key, args);
        GetText("Highlighted")?.Localize(key, args);
        GetText("ButtonLocked")?.Localize(key, args);
        var txt = FindUtil.Child(transform, "Text", true);
        if (txt != null) {
            txt.GetComponent<TMP_Text>().Localize(key, args);
        }

        //GetText(gameObject)?.Localize(key, args);
        //GetText(active)?.Localize(key, args);
        //GetText(inactive)?.Localize(key, args);
        //GetText(locked)?.Localize(key, args);
        //GetText(highlighted)?.Localize(key, args);
    }
    */

}


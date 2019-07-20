using UnityEngine;

public static class AnimationUtils {

    public static string GetHeirarchyPathToGameObject(GameObject go) {
        var path = go.name;
        var current = go;
        while (current.transform.parent != null) {
            path = current.transform.parent.gameObject.name + "/" + path;
            current = current.transform.parent.gameObject;
        }

        return path;
    }
}


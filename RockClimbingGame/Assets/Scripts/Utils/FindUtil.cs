using UnityEngine;
using UnityEngine.SceneManagement;

public static class FindUtil {

    // NOTE: we require a custom recursive find function when searching inside a Transform
    // because Transform.Find() is different to GameObject.Find(), and does not
    // search through the entire heirarchy

    public static Transform Child(Transform transform, string name, bool allowNull = false) {
        var child = ChildRecursive(transform, name);
        if (child == null && allowNull == false) {
            Debug.LogError("Could not find child with name: " + name + " in object: " + transform.name);
        }
        return child;
    }

    // This function will reliably found disabled objects unlike the built in find function
    public static Transform FindTopLevelChild(Transform parent, string name) {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform transform in children) {
            if (transform.name == name) {
                return transform;
            }
        }
        return null;
    }

    // wrapping Child function, so we can display the not found message correctly for the recursive search
    private static Transform ChildRecursive(Transform transform, string name) {
        if (transform == null) {
            Debug.LogError("Attempting to find child with name: " + name + " inside null container.");
            return null;
        }
        var result = FindTopLevelChild(transform, name);
        if (result != null) return result;
        foreach (Transform child in transform) {
            result = ChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }

    public static Transform Child(GameObject gameObject, string name) {
        if (gameObject == null) {
            Debug.LogError("Attempting to find child with name: " + name + " inside null container.");
            return null;
        }
        return FindUtil.Child(gameObject.transform, name);
    }

    public static T Child<T>(Transform gameObject, string name)
        where T : Component {
        if (gameObject == null) {
            Debug.LogError("Attempting to find child with name: " + name + " inside null container.");
            return null;
        }
        var child = Child(gameObject.transform, name);
        if (child != null) {
            var behaviour = child.GetComponent<T>();
            if (behaviour == null) {
                Debug.LogError("Found object with name: " + name + ", but could not find a component with the specified type" + typeof(T).ToString());
            }
            return behaviour;
        }
        return null;
    }

    public static Transform Find(string name) {
        var found = GameObject.Find(name);
        if (found == null) {
            Debug.LogError("Could not find object with name: " + name);
        }
        return found.transform;
    }

    public static T Find<T>(string name) where T : Behaviour {
        var found = GameObject.Find(name);
        if (found == null) {
            Debug.LogError("Could not find object with name: " + name);
            return null;
        }
        var component = found.GetComponent<T>();
        if (component == null) {
            Debug.LogError("Could not component of specified type on container with name: " + found.name);
            return null;
        }
        return component;
    }

    public static T Find<T>() where T : Behaviour {
        var allObjects = GameObject.FindObjectsOfType<T>();
        if (allObjects.Length == 0) {
            Debug.LogError("Could not component of specified type");
            return null;
        }
        // just return the first one
        return allObjects[0];
    }

    public static T FindComponentInScene<T>(string sceneName, string name) where T : Behaviour {
        var transform = FindInScene(sceneName, name);
        var t = transform.GetComponent<T>();
        if (t == null) {
            Debug.LogError("Could not find Component with name: " + name);
        }
        return t;
    }

    public static Transform FindInScene(string sceneName, string name) {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid()) {
            Debug.LogError("FindUtil.FindInScene() - could not load find loaded scene: " + sceneName);
            return null;
        }

        var rootGameObjects = scene.GetRootGameObjects();
        foreach (var go in rootGameObjects) {
            var allTransforms = go.GetComponentsInChildren<Transform>();
            foreach (var t in allTransforms) {
                if (t.name == name) {
                    return t;
                }
            }
        }
        Debug.LogError("Could not find child with name: " + name);
        return null;
    }

    private static Transform FindInScenes(string name) {
        var count = SceneManager.sceneCount;
        for (var i = 0; i < count; i++) {
            var scene = SceneManager.GetSceneAt(i);
            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var go in rootGameObjects) {
                var allTransforms = go.GetComponentsInChildren<Transform>();
                foreach (var t in allTransforms) {
                    if (t.name == name) {
                        return t;
                    }
                }
            }
        }
        Debug.LogError("Could not find child with name: " + name);
        return null;
    }
}

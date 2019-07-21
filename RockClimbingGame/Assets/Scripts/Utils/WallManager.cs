using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {

    public GameObject wallPrefab;
    public float wallHeight;

    public int wallCount { get; private set; }
    private List<GameObject> wallList;

    public void NewLevel() {
        DestroyLevel();
        wallCount = 2;
	}

    private void DestroyLevel() {
        if (wallList != null) {
            foreach (var wall in wallList)
                UnityEngine.Object.Destroy(wall);
        }
        wallList = new List<GameObject>();
    }

    public void AddWall() {
        var wall = UnityEngine.Object.Instantiate(wallPrefab);
        wall.SetActive(true);
        wall.transform.position = new Vector3(
            wall.transform.position.x,
            wall.transform.position.y + (wallHeight * wallCount),
            wall.transform.position.z);

        wallList.Add(wall);

        if(wallList.Count > 3) {
            UnityEngine.Object.Destroy(wallList[0]);
            wallList.RemoveAt(0);
        }

		wallCount++;
	}

    void Update() {

    }
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RockGenerator : MonoBehaviour {
	
	const int colomnCount = 21;
	const float colomnSize = 1f;
	const float colomnRandom = 0.5f;
	const int rowCount = 200;
	const float rowSize = 1;
	const float rowRandom = 0.5f;
	
	private Rock[,] rocks;
	
	private Dictionary<int, List<Rock>> closestRocks;
	private Dictionary<int, List<Rock>> startingRocks;

	[ContextMenu("Generate Rocks")]
	void GenerateRocks() {
		Debug.Log("Generating Rocks - Start");

		ClearRocks();
		
		rocks = new Rock[colomnCount, rowCount];
		closestRocks = new Dictionary<int, List<Rock>>();
		startingRocks = new Dictionary<int, List<Rock>>();

		var rockPrefab = transform.Find("RockPrefab").gameObject;
		rockPrefab.SetActive(false);

		for (var height = 0; height < rowCount; height++) {
			for (var width = 0; width < colomnCount; width++) {

				rocks[width, height] = GameObject.Instantiate(rockPrefab, this.transform).GetComponent<Rock>();

				if ((height == 0 || height == 1) &&
					(width == 3 || width == 4 || width == 7 || width == 8 || width == 12 || width == 13 || width == 16 || width == 17)) {
					rocks[width, height].transform.position = new Vector3(
						((width - Mathf.FloorToInt(colomnCount / 2f)) * colomnSize) + 0.5f * colomnSize,
						((height + 1) * rowSize) + 0.5f * rowSize,
						0);

					if(width == 3 || width == 4) {
						if(false == startingRocks.ContainsKey(0)) {
							startingRocks.Add(0, new List<Rock>());
						}
						startingRocks[0].Add(rocks[width, height]);
					}
					if (width == 7 || width == 8) {
						if (false == startingRocks.ContainsKey(1)) {
							startingRocks.Add(1, new List<Rock>());
						}
						startingRocks[1].Add(rocks[width, height]);
					}
					if (width == 12 || width == 13) {
						if (false == startingRocks.ContainsKey(2)) {
							startingRocks.Add(2, new List<Rock>());
						}
						startingRocks[2].Add(rocks[width, height]);
					}
					if (width == 16 || width == 17) {
						if (false == startingRocks.ContainsKey(3)) {
							startingRocks.Add(3, new List<Rock>());
						}
						startingRocks[3].Add(rocks[width, height]);
					}

					rocks[width, height].weightLimit = 999f;
				}
				else {
					rocks[width, height].transform.position = new Vector3(
						((width - Mathf.FloorToInt(colomnCount / 2f)) * colomnSize) + (UnityEngine.Random.value * colomnRandom + 0.25f) * colomnSize,
						((height + 1) * rowSize) + (UnityEngine.Random.value* rowRandom + 0.25f) * rowSize,
						0);

					rocks[width, height].weightLimit = 125f - (UnityEngine.Random.value * 50f) - (height * 0.4f);
				}

				rocks[width, height].gameObject.SetActive(true);
				rocks[width, height].name = "Rock-" + width + "-" + height;
				rocks[width, height].GetComponent<Rock>().SetGraphic();

			}
		}

		BuildClosestRocksLookup();
		

		Debug.Log("Generating Rocks - End");
	}

	/*void RebuildRockList() {
		rocks = new GameObject[colomnCount, rowCount];
		closestRocks = new Dictionary<int, List<Rock>>();

		foreach (Transform child in transform) {
			if (child.name != "RockPrefab") {
				var rockData = child.name.Split('-');

				var widthIndex = int.Parse(rockData[1]);
				var heightIndex = int.Parse(rockData[2]);

				rocks[widthIndex, heightIndex] = child.gameObject;
			}
		}

		BuildClosestRocksLookup();

	}*/

	void BuildClosestRocksLookup() {
		closestRocks = new Dictionary<int, List<Rock>>();

		for (var rowIndex = 0; rowIndex < rowCount; rowIndex++) {
			for (var colomnIndex = 0; colomnIndex < colomnCount; colomnIndex++) {
				var surroundingRocks = new List<Rock>();

				if (colomnIndex > 0) {
					if (rowIndex > 0) {
						surroundingRocks.Add(rocks[colomnIndex - 1, rowIndex - 1].GetComponent<Rock>());
					}

					surroundingRocks.Add(rocks[colomnIndex - 1, rowIndex].GetComponent<Rock>());

					if (rowIndex < rowCount - 1) {
						surroundingRocks.Add(rocks[colomnIndex - 1, rowIndex + 1].GetComponent<Rock>());
					}
				}

				if (rowIndex > 0) {
					surroundingRocks.Add(rocks[colomnIndex, rowIndex - 1].GetComponent<Rock>());
				}

				surroundingRocks.Add(rocks[colomnIndex, rowIndex].GetComponent<Rock>());

				if (rowIndex < rowCount - 1) {
					surroundingRocks.Add(rocks[colomnIndex, rowIndex + 1].GetComponent<Rock>());
				}

				if (colomnIndex < colomnCount - 1) {
					if (rowIndex > 0) {
						surroundingRocks.Add(rocks[colomnIndex + 1, rowIndex - 1].GetComponent<Rock>());
					}

					surroundingRocks.Add(rocks[colomnIndex + 1, rowIndex].GetComponent<Rock>());

					if (rowIndex < rowCount - 1) {
						surroundingRocks.Add(rocks[colomnIndex + 1, rowIndex + 1].GetComponent<Rock>());
					}
				}

				closestRocks.Add((rowIndex * colomnCount) + colomnIndex, surroundingRocks);
			}
		}
	}  

	public Rock GetClosestRockToPoint(Vector2 point, float distance = float.MaxValue) {
		//first get center index
		var colomnIndex = Mathf.FloorToInt((Mathf.FloorToInt(colomnCount / 2f) * colomnSize + point.x) / colomnSize);
		var rowIndex = Mathf.FloorToInt(point.y / rowSize)-1;

		if (colomnIndex < 0 ||
			colomnIndex >= colomnCount ||
			rowIndex < 0 ||
			rowIndex >= rowCount) {
			return null;
		}

		//then get surrounding index
		var surroundingRocks = closestRocks[(rowIndex * colomnCount) + colomnIndex];

		//then check for closest rock
		Rock closestRock = null;
		foreach(var rock in surroundingRocks) {
			if (rock.gameObject.activeSelf) {
				var distanceToRock = (new Vector2(rock.transform.position.x, rock.transform.position.y) - point).SqrMagnitude();
				if (distance > distanceToRock) {
					distance = distanceToRock;
					closestRock = rock;
				}
			}
		}

		return closestRock;
	}

	public void Start() {
		if (rocks == null) {
			GenerateRocks();
		}
	}

	public void Update() {
		/*if(rocks == null) {
			RebuildRockList();
		}

		var rock = GetClosestRockToPoint(new Vector2(rockPrefab.transform.position.x, rockPrefab.transform.position.y));

		if(rock != null) {
			Debug.Log(rock.name);
		}*/
	}

	[ContextMenu("Clear Rocks")]
	void ClearRocks() {
		var toDestroy = new List<GameObject>();
		foreach(Transform child in transform) {
			if(child.name != "RockPrefab") {
				toDestroy.Add(child.gameObject);
			}
		}

		foreach(var item in toDestroy) {
			DestroyImmediate(item);
		}
	}
}

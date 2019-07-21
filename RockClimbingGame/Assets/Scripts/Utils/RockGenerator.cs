using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class RockGenerator : MonoBehaviour {
	
	const int colomnCount = 15;

	const float colomnRandom = 0.5f;
	const float rowRandom = 0.5f;

	public const float colomnSize = 1.5f;
	public const float rowSize = 1.5f;
	
	private Dictionary<int, List<Rock>> rowsOfRocks;

	public int currentHeight = 0;
	public int currentIndexToRemove = 0;

	public void GenerateRocks(int rowCount) {

		if (rowsOfRocks == null) {
			rowsOfRocks = new Dictionary<int, List<Rock>>();
		}

		var rockPrefab = transform.Find("RockPrefab").gameObject;
		rockPrefab.SetActive(false);

		var totalAfterNew = currentHeight + rowCount;

		for (; currentHeight < totalAfterNew; currentHeight++) {
			rowsOfRocks.Add(currentHeight, new List<Rock>());
			for (var width = 0; width < colomnCount; width++) {

				var newRock = GameObject.Instantiate(rockPrefab, this.transform).GetComponent<Rock>();
				rowsOfRocks[currentHeight].Add(newRock);

				if (currentHeight == 0 || currentHeight == 1) {
					newRock.transform.position = new Vector3(
						((width - Mathf.FloorToInt(colomnCount / 2f)) * colomnSize) + 0.5f * colomnSize,
						((currentHeight + 1) * rowSize) + 0.5f * rowSize,
						0);

					newRock.weightLimit = 999f;
				}
				else {
					newRock.transform.position = new Vector3(
						((width - Mathf.FloorToInt(colomnCount / 2f)) * colomnSize) + (UnityEngine.Random.value * colomnRandom + 0.25f) * colomnSize,
						((currentHeight + 1) * rowSize) + (UnityEngine.Random.value* rowRandom + 0.25f) * rowSize,
						0);

					newRock.weightLimit = 125f - (UnityEngine.Random.value * 30f) - (currentHeight * 0.1f);
				}

				newRock.gameObject.SetActive(true);
				newRock.name = "Rock-" + width + "-" + currentHeight;
				newRock.SetGraphic();
			}
		}
		
		while(rowsOfRocks.Count > 50) {
			foreach (var rock in rowsOfRocks[currentIndexToRemove]) {
				Destroy(rock.gameObject);
			}
			rowsOfRocks.Remove(currentIndexToRemove);

			currentIndexToRemove++;
		}
	}

	public Rock GetClosestRockToPoint(Vector2 point, float distance = float.MaxValue) {
		//first get center index
		var colomnIndex = Mathf.FloorToInt((Mathf.FloorToInt(colomnCount / 2f) * colomnSize + point.x) / colomnSize);
		var rowIndex = Mathf.FloorToInt(point.y / rowSize)-1;

		if (colomnIndex < 0 ||
			colomnIndex >= colomnCount ||
			rowIndex < 0) {
			return null;
		}
		
		if(false == rowsOfRocks.ContainsKey(rowIndex)) {
			Debug.Log(colomnIndex + ", " + rowIndex);
		}

		var surroundingRocks = new List<Rock>();
		if(colomnIndex > 0) {
			surroundingRocks.Add(rowsOfRocks[rowIndex][colomnIndex - 1]);
			if(rowIndex > 0) surroundingRocks.Add(rowsOfRocks[rowIndex-1][colomnIndex - 1]);
			if (rowIndex < (currentHeight+1)) surroundingRocks.Add(rowsOfRocks[rowIndex + 1][colomnIndex - 1]);
		}
		
		surroundingRocks.Add(rowsOfRocks[rowIndex][colomnIndex]);
		if (rowIndex > 0) surroundingRocks.Add(rowsOfRocks[rowIndex - 1][colomnIndex]);
		if (rowIndex < (currentHeight - 1)) surroundingRocks.Add(rowsOfRocks[rowIndex + 1][colomnIndex]);
		
		if (colomnIndex < (colomnCount-1)) {
			surroundingRocks.Add(rowsOfRocks[rowIndex][colomnIndex + 1]);
			if (rowIndex > 0) surroundingRocks.Add(rowsOfRocks[rowIndex - 1][colomnIndex + 1]);
			if (rowIndex < (currentHeight + 1)) surroundingRocks.Add(rowsOfRocks[rowIndex + 1][colomnIndex + 1]);
		}

		//check for closest rock
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
}

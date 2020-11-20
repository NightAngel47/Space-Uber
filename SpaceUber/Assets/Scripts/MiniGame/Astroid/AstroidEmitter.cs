/*
 * AstroidEmitter.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: 
 */

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AstroidEmitter : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject astroidPrefab;
	[SerializeField] float minimumSpawnDelay = 2;
	[SerializeField] float maximumSpawnDelay = 5;
	AstroidMiniGame miniGameManager;
	List<int> indexesInUse = new List<int>();
	public bool stop = false;

	private void Start()
	{
		miniGameManager = FindObjectOfType<AstroidMiniGame>();
		StartCoroutine(SpawnAstroids());
	}

	IEnumerator SpawnAstroids()
	{
		int index;
		while (miniGameManager.requiredAstroids > 0 && miniGameManager.damageTillFailure > 0)
		{
			yield return new WaitForSeconds(Random.Range(minimumSpawnDelay, maximumSpawnDelay));
			index = Random.Range(0, spawnPoints.Length);
			if (!indexesInUse.Contains(index))
			{
				indexesInUse.Add(index);
				if(indexesInUse.Count == spawnPoints.Length/2) { indexesInUse.Clear(); }
				Instantiate(astroidPrefab, spawnPoints[index].position, new Quaternion(), transform);
			}
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
class Wave : MonoBehaviour {
	[SerializeField]
	private float Delay = 2;

	[SerializeField]
	private List<SpawnEntity> spawnList = new List<SpawnEntity>();

	void Wait(float seconds, Action action) {
		StartCoroutine(_wait(seconds, action));
	}

	IEnumerator _wait(float time, Action callback) {
		yield return new WaitForSeconds(time);
		callback();
	}

	void Start() {
		// Start spawning with a delay
		Wait(Delay, StartSpawnEntityScripts);
	}

	void StartSpawnEntityScripts() {
		foreach (var spawnScript in spawnList) {
			spawnScript.StartSpawning();
		}
	}

	void Update() {
		if (spawnList.All(s => s.SpawningDone && s.SpawnedEntities.All(e => e == null))) {
			foreach (var script in spawnList) {
				script.Destroy();
			}
			Destroy(this);
		}
	}
}

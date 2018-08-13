using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour {
	[SerializeField]
	public List<Waypoint> Neighbours = new List<Waypoint>();

	[SerializeField]
	public float Distance;

	public Waypoint Previous;

	// Use this for initialization
	void Start() {
		
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f);
		foreach (var neighbour in Neighbours.Where(n => n != null)) {
			Gizmos.DrawLine(transform.position, neighbour.transform.position);
		}
	}
}

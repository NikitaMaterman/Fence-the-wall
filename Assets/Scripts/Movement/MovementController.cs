using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour {
	[SerializeField]
	public Boolean AddRotation;

	[SerializeField]
	public float WalkSpeed = 1.0f;
	public float RotationSpeed = 1.0f;
	public GameObject GroundObject;

	private float traversalTime;
	private float traversedTime;
	private Stack<Vector3> path = new Stack<Vector3>();
	private Vector3 position;
	
	// Use this for initialization
	void Start() {
		GroundObject = GameObject.FindGameObjectWithTag("Ground");
	}

	// A*
	public void NavigateTo(Vector3 targetPosition) {
		path = new Stack<Vector3>();
		var closest = LocalSearchFromPoint(transform.position);
		var target = LocalSearchFromPoint(targetPosition);
		if (closest == null || target == null || closest == target) {
			return;
		}

		var closedList = new List<Waypoint>();
		var openList = new SortedList<float, Waypoint> {
			{ 0, closest },
		};
		closest.Previous = null;
		closest.Distance = 0f;

		Waypoint current = null;
		while (openList.Any()) {
			current = openList.Values[0];
			openList.RemoveAt(0);
			closedList.Add(current);

			if (current == target) {
				break;
			}

			foreach (var neighbour in current.Neighbours.Where(n => n != null)) {
				if (closedList.Contains(neighbour) || openList.ContainsValue(neighbour)) {
					continue;
				}

				neighbour.Previous = current;
				neighbour.Distance = current.Distance + (neighbour.transform.position - current.transform.position).magnitude;

				var targetDistance = (neighbour.transform.position - target.transform.position).magnitude;
				openList.Add(neighbour.Distance + targetDistance, neighbour);
			}
		}

		if (current == target) {
			// Ensure final position is added
			path.Push(targetPosition);

			while (current != null) {
				path.Push(current.transform.position);
				current = current.Previous;
			}

			// Ensure current position is added
			path.Push(transform.position);
		}
	}

	void Update() {
		if (path != null) {
			if (path.Count > 0) {
				if (traversedTime < traversalTime) {
					traversedTime += Time.deltaTime;
					if (traversedTime > traversalTime) {
						traversedTime = traversalTime;
					}

					var target = path.Peek();

					// Calculate position based on linear path
					transform.position = Vector3.Lerp(position, target, traversedTime / traversalTime);

					if (GroundObject != null) {
						RaycastHit hit;
						Ray ray = new Ray(new Vector3(transform.position.x, 90, transform.position.z), Vector3.down);
						if (GroundObject.GetComponent<Collider>().Raycast(ray, out hit, 2.0f * 90)) {
							transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
						}
					}

					// Calculate rotation based on direction
					var additionalRotation = AddRotation ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 0, 0);
					var rotation = additionalRotation * Quaternion.LookRotation((target - transform.position).normalized, Vector3.up);

					// Smooth rotation to avoid snap rotations
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
				} else {
					position = path.Pop();
					if (path.Count > 0) {
						traversedTime = 0;
						traversalTime = (position - path.Peek()).magnitude / WalkSpeed;
					}
				}
			} else {
				// Destination reached - subtract a heart
				if (WorldManager.Instance != null) {
					WorldManager.Instance.RemainingHearts--;
				}

				Destroy(gameObject);
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f);
		if (!path.Any())
		{
			return;
		}

		Gizmos.DrawLine(transform.position, path.Peek());
	}

	private Waypoint LocalSearchFromPoint(Vector3 point) {
		Waypoint result = null;

		// Get snapshot for this frame, don't cache over frames
		GameObject[] globalWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

		// Calculate all waypoint distances & select both the waypoint and distance for later use
		var waypointDistanceEnumerable = globalWaypoints.Select(waypoint => new { distance = (waypoint.transform.position - point).magnitude, waypoint });

		// Get smallest distance (we're only interested in the first)
		var waypointDistance = waypointDistanceEnumerable.OrderBy(x => x.distance).FirstOrDefault();
		if (waypointDistance != null) {
			result = waypointDistance.waypoint.GetComponent<Waypoint>();
		}

		return result;
	}
}

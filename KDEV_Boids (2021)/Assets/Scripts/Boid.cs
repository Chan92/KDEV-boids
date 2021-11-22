using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
	public Vector3 velocity;

	public bool debugVelocity;

	private void OnDrawGizmos() {
		if(debugVelocity) {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.transform.position, velocity);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager:MonoBehaviour {
	public GameObject boidPrefab;
	public int boidAmount = 20;

	[Space(10)]
	public float coheasionEffect = 100f;
	public float seperationEffect = 100f;
	public float alignmentEffect = 8f;
	public float moveSpeed = 10f;

	[Space(10)]
	public Vector3 fieldSize = new Vector3(15f, 10f, 5f);

	List<Boid> boids = new List<Boid>();
	Vector3 cohesion, seperation, alignment, boundPosition;

	void Start() {
		CreateBoids();
	}

	void Update() {
		UpdateBoidPositions();
	}

	void CreateBoids() {
		for(int i = 0; i < boidAmount; i++) {
			GameObject newBoid = Instantiate(boidPrefab);
			Boid b = newBoid.GetComponent<Boid>();

			float randomX = Random.Range(-fieldSize.x, fieldSize.x);
			float randomY = Random.Range(-fieldSize.y, fieldSize.y);
			float randomZ = Random.Range(0, fieldSize.z);
			newBoid.transform.position = new Vector3(randomX, randomY, randomZ);

			newBoid.name = "Boid " + i;
			newBoid.transform.parent = transform;
			boids.Add(b);
		}
	}

	void UpdateBoidPositions() {
		foreach(Boid b in boids) {
			cohesion = Cohesion(b);
			seperation = Seperation(b);
			alignment = Alignment(b);

			boundPosition = BoundPosition(b);

			b.velocity = b.velocity + cohesion + seperation + alignment + boundPosition;
			b.velocity = b.velocity.normalized * moveSpeed;
			b.transform.position = b.transform.position + b.velocity * Time.deltaTime;
		}
	}

	//Rule1: fly towards center of mass of neighbouring boids
	public Vector3 Cohesion(Boid boidJ) {
		Vector3 pcj = Vector3.zero;

		foreach(Boid b in boids) {
			if(b != boidJ) {
				pcj += b.transform.position;
			}
		}

		pcj = pcj / (boids.Count - 1);

		return (pcj - boidJ.transform.position) / coheasionEffect;
	}

	//Rule2: keep small distance of other objects (boids included)
	public Vector3 Seperation(Boid boidJ) {
		Vector3 c = Vector3.zero;

		foreach(Boid b in boids) {
			if(b != boidJ) {
				float dist = Vector3.Distance(b.transform.position, boidJ.transform.position);
				if(dist < seperationEffect) {
					c = c - (b.transform.position - boidJ.transform.position);
				}
			}
		}

		return c;
	}

	//Rule3: boids try to match velocity with nearby boids
	public Vector3 Alignment(Boid boidJ) {
		Vector3 pvj = Vector3.zero;

		foreach(Boid b in boids) {
			if(b != boidJ) {
				pvj = pvj + b.velocity * Time.deltaTime;
			}
		}

		pvj = pvj / (boids.Count - 1);

		return (pvj - boidJ.velocity * Time.deltaTime) / alignmentEffect;
	}

	public Vector3 BoundPosition(Boid b) {
		Vector3 v = Vector3.zero;

		if(b.transform.position.x < -fieldSize.x) {
			v.x = 10;
		} else if(b.transform.position.x > fieldSize.x) {
			v.x = -10;
		}

		if(b.transform.position.y < -fieldSize.y) {
			v.y = 10;
		} else if(b.transform.position.y > fieldSize.y) {
			v.y = -10;
		}

		if(b.transform.position.z < -fieldSize.z) {
			v.z = 10;
		} else if(b.transform.position.z > fieldSize.z) {
			v.z = -10;
		}

		return v;
	}
}
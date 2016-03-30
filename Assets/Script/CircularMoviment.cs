using UnityEngine;
using System.Collections;

public class CircularMoviment : MonoBehaviour {


	/*
	void OnDrawGizmos()
	{
		Gizmos.DrawSphere(_centre, 0.1f);
		Gizmos.DrawLine(_centre, transform.position);
	}*/


	public Transform center;
	public float degreesPerSecond = -65.0f;

	private Vector3 v;

	void Start() {
		v = transform.position - center.position;
	}

	void Update () {
		v = Quaternion.AngleAxis (degreesPerSecond * Time.deltaTime, Vector3.forward) * v;
		transform.position = center.position + v;
	}

}




using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public float mouseSensitivity   = 0.000000000000000001f;
	private Vector3 lastPosition  ;
	public Camera cam;

	void Update()
	{
		if (Input.GetMouseButtonDown(2))
		{
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(2))
		{
			Vector3 delta = Input.mousePosition - lastPosition;
			transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
			lastPosition = Input.mousePosition;
		}


		if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
		{
			Camera.main.orthographicSize++;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
		{
			Camera.main.orthographicSize--;
		}


	}

}

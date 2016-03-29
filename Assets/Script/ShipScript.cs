using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShipScript : MonoBehaviour {

	Rigidbody rb;
	BoxCollider shippcollider;
	Vector3 startPosition;
	protected Dropdown dropMovimento;

	private float base_size = 0.4444f;
	private float speed = 1;
	private bool colidiu = false;
	private int random_id = 0;
	public GameObject cubo;

	//	rb.MovePosition(startPosition + (this.transform.up * 3));


	/*

boardgamegeek.com/thread/1337514/dimensions-radii-and-arc-lenghts-maneuver-template

Turn inside radii in mm: 25.0, 53.0, 80.0
Turn outside radii in mm: 45.0, 73.0, 100.0

35mm(0,388),63mm,90mm


Bank inside radii in mm: 70.0, 120.0, 170.0
Bank outside radii in mm: 90.0, 140.0, 190.0


10 - 90
x - 4

10 - 90 
x = 3.5


	
	 */
	// Use this for initialization
	void Start () {


	}

	protected void carregaComponentes (){


		rb = GetComponent<Rigidbody> ();
		shippcollider = GetComponent<BoxCollider> ();
		dropMovimento = GameObject.FindWithTag ("Movimentos").GetComponent<Dropdown>() as Dropdown;
		random_id = Random.Range (1,100); //melhorar

	}


	public void move_foward(int foward){
		colidiu = false;
		startPosition = this.transform.position;
		StartCoroutine(fowardsmoothMovement(startPosition + (this.transform.up * ((base_size * foward) + base_size))));

	} 




	public void move_keyturn(int foward){
	 	colidiu = false;
		startPosition = this.transform.position;
		StartCoroutine(	fowardsmoothMovementKey(startPosition + (this.transform.up * ((base_size * foward) + base_size))));
	
	}


	protected IEnumerator fowardsmoothMovementKey (Vector3 end){
		
		yield return StartCoroutine(fowardsmoothMovement (end));
		if (!colidiu) {
			this.transform.Rotate (new Vector3 (0, 0, 180));
		}

	
	}


	protected IEnumerator fowardsmoothMovement (Vector3 end)
	{

		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		int count  = 0;
		while (sqrRemainingDistance > float.Epsilon && !colidiu) {

			Vector3 newPosition = Vector3.MoveTowards (this.transform.position, end, speed * Time.deltaTime);
			rb.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		
			yield return null;

		}

	}


	public void move_turn(int distancia, string lado){
		colidiu = false;
		startPosition = this.transform.position;

		float raio = 0;
		if (distancia == 1) {
			raio = 0.388f;
		}else  if (distancia == 2) {
			raio = 0.7f;
		}else  if (distancia == 3) {
			raio = 1f;
		}

		if (lado.Equals ("esquerda")) {
			raio = raio * 1;
		}

		Vector3 centrocircul = startPosition + new Vector3 (raio, 0f,0f);

		Instantiate (cubo, centrocircul, Quaternion.identity) ; 

		float myAngleInDegrees = 0f * Mathf.Deg2Rad;


		float x = centrocircul.x + raio *  Mathf.Sin(myAngleInDegrees) ;
		float y = centrocircul.y + raio * Mathf.Cos(myAngleInDegrees) ;


		Vector3 end = new  Vector3 (x,y,startPosition.z);

		Instantiate (cubo, end, Quaternion.identity) ;  
		StartCoroutine(	fowardAngLeMovement(end));


	} 

	protected IEnumerator fowardAngLeMovement (Vector3 end)
	{
		
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		int count  = 0;
		while (sqrRemainingDistance > float.Epsilon && !colidiu) {

			Vector3 newPosition = Vector3.MoveTowards (this.transform.position, end, speed * Time.deltaTime);
			rb.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			yield return null;

		}

	}

	private Vector3 currentPosition;

	public Vector3 PolarCoordinates(float angle, float radius)
	{
		// Assuming movement is on XY plane
		Vector3 relativePosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);

		return currentPosition + relativePosition;
	}


	void OnTriggerEnter(Collider other) {


		if (other.CompareTag ("Shipp"))   {
			colidiu = true;
			return;
		}


	
	}


	// Update is called once per frame

}

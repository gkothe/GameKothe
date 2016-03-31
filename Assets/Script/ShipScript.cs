using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShipScript : MonoBehaviour {

	protected Rigidbody rb;
	BoxCollider shippcollider;
	Vector3 startPosition;
	protected Dropdown dropMovimento;
	public Transform pontocentral;

	private float base_size = 0.4444f;
	public float speed = 1;
	private bool colidiu = false;
	private int random_id = 0;
	public GameObject cubo;
	public GameObject esfera;
	public GameObject cilindro;
	protected Transform turnleft1;
	protected Transform turnleft2;
	protected Transform turnleft3;
	protected Transform turnright1;
	protected Transform turnright2;
	protected Transform turnright3;




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
		GameObject teste = (GameObject)Instantiate (esfera, pontocentral.transform.position, Quaternion.identity) ; 
		pontocentral = teste.GetComponent<Transform>();


		turnleft1 = transform.Search ("TurnLeft1");
		turnleft2 = transform.Search ("TurnLeft2");
		turnleft3 = transform.Search ("TurnLeft3");;
		turnright1= transform.Search ("TurnRight1");
		turnright2 = transform.Search ("TurnRight2");;
		turnright3 = transform.Search ("TurnRight3");;

	}


	public void move_foward(int foward){
		colidiu = false;
		startPosition = this.transform.position;
		StartCoroutine(fowardsmoothMovement(startPosition + (this.transform.forward * ((base_size * foward) + base_size))));

	} 	


	public void move_keyturn(int foward){
	 	colidiu = false;
		startPosition = this.transform.position;
		StartCoroutine(	fowardsmoothMovementKey(startPosition + (this.transform.forward * ((base_size * foward) + base_size))));
	
	}


	protected IEnumerator fowardsmoothMovementKey (Vector3 end){
		
		yield return StartCoroutine(fowardsmoothMovement (end));
		if (!colidiu) {
			this.transform.Rotate (new Vector3 (0, 180, 0));
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


	public void move_turn(int distancia, string lado, Transform centro){
		colidiu = false;
		startPosition = new Vector3(0,0,0);






		float myAngleInDegrees = 90f * Mathf.Deg2Rad;
		Debug.Log ("Inicio-------");
		Debug.Log (myAngleInDegrees);
		Debug.Log (Mathf.Sin (myAngleInDegrees));
		Debug.Log (Mathf.Cos (myAngleInDegrees));

		Vector3 centrocircul = centro.transform.localPosition;

		Instantiate (cubo, centrocircul, Quaternion.identity) ; 

		//Debug.Log (Vector2.Angle(new Vector2(startPosition.x,startPosition.y),new Vector2(centrocircul.x,centrocircul.y)));


		float raio = 0;


		raio = Vector2.Distance (new Vector2(centrocircul.x,centrocircul.z ),new Vector2(startPosition.x,startPosition.z  ));	
		//raio = Vector3.Distance (centrocircul, startPosition);	
		Debug.Log ("raio: " + raio);

		 
	/*	float x = centrocircul.x +  raio  *  Mathf.Sin(myAngleInDegrees) ;
		float z = centrocircul.z + raio * Mathf.Cos(myAngleInDegrees) ;

		
		Debug.Log ("x: " + x);
		Debug.Log ("z: " + z);

	

		*/


		Vector3 randomCircle = new Vector3(Mathf.Cos(myAngleInDegrees), 0,Mathf.Sin(myAngleInDegrees));
		Vector3 end = centro.transform.TransformPoint(randomCircle * raio);



	//	Vector3 end = new  Vector3 (x,startPosition.y,z);

		Instantiate (cubo, end, Quaternion.identity) ;  
		StartCoroutine(	fowardAngLeMovement(end,raio));

		Debug.Log ("fim-------");
	} 





	protected IEnumerator fowardAngLeMovement (Vector3 end,float raio)
	{


		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon && !colidiu) {

			Vector3 newPosition = Vector3.MoveTowards (this.transform.position, end, speed * Time.deltaTime);
		//	Vector3 newPosition =  Quaternion.AngleAxis (speed * Time.deltaTime, Vector3.forward) * end;
			Vector3  dir = pontocentral.position - transform.position;

			Instantiate (cilindro, dir, Quaternion.identity) ; 



			rb.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			yield return null;

		}

	}



	void OnTriggerEnter(Collider other) {


		if (other.CompareTag ("Shipp"))   {
			colidiu = true;
			return;
		}


	
	}


	// Update is called once per frame

}

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


	//	rb.MovePosition(startPosition + (this.transform.up * 3));

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

	void OnTriggerEnter(Collider other) {


		if (other.CompareTag ("Shipp"))   {
			colidiu = true;
			return;
		}


	
	}


	// Update is called once per frame
	void Update () {
	



	}
}

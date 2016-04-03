	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;
	public class ShipScript : MonoBehaviour {

		protected Rigidbody rb;
		Collider shippcollider;
		Vector3 startPosition;
		protected Dropdown dropMovimento;
		//public Transform pontocentral;

		private float base_size = 0.4f;
		public float speed = 995;
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

		protected Transform Bankleft1;
		protected Transform Bankleft2;
		protected Transform Bankleft3;
		protected Transform Bankright1;
		protected Transform Bankright2;
		protected Transform Bankright3;
		protected Transform spaw_movimento;
		public  LayerMask LayerShip;
		Collider collider;




		// Use this for initialization
		void Start () {


		}

		protected void carregaComponentes (){


			rb = GetComponent<Rigidbody> ();
			shippcollider = GetComponent<Collider> ();
			dropMovimento = GameObject.FindWithTag ("Movimentos").GetComponent<Dropdown>() as Dropdown;
			random_id = Random.Range (1,100); //melhorar
			//GameObject teste = (GameObject)Instantiate (esfera, pontocentral.transform.position, Quaternion.identity) ; 
			//pontocentral = teste.GetComponent<Transform>();
			


			turnleft1 = transform.Search ("TurnLeft1");
			turnleft2 = transform.Search ("TurnLeft2");
			turnleft3 = transform.Search ("TurnLeft3");;
			turnright1= transform.Search ("TurnRight1");
			turnright2 = transform.Search ("TurnRight2");;
			turnright3 = transform.Search ("TurnRight3");;


			Bankleft1   = transform.Search ("BankLeft1");
			Bankleft2  = transform.Search  ("BankLeft2");;
			Bankleft3   = transform.Search ("BankLeft3");;
			Bankright1  = transform.Search ("BankRight1");;
			Bankright2  = transform.Search ("BankRight2");;
			Bankright3  = transform.Search ("BankRight3");;


			spaw_movimento = transform.Search ("spaw_movimento");;

		}


		public void move_foward(int foward){
			colidiu = false;
			startPosition = spaw_movimento.transform.position;
			StartCoroutine(fowardsmoothMovement(startPosition + (this.transform.forward * ((base_size * foward) + base_size))));

		} 	


		public void move_keyturn(int foward){
		 	colidiu = false;
			startPosition = spaw_movimento.transform.position;
			StartCoroutine(	fowardsmoothMovementKey(startPosition + (this.transform.forward * ((base_size * foward) + base_size))));
		
		}


		protected IEnumerator fowardsmoothMovementKey (Vector3 end){
			
			yield return StartCoroutine(fowardsmoothMovement (end));
			if (!colidiu) {
				this.transform.Rotate (new Vector3 (0, 180, 0));
			}

		
		}








		public void move_Bank( string lado, Transform centro){
			colidiu = false;
			startPosition = spaw_movimento.localPosition; // new Vector3(0,0,0);


			float raio = 0;
			Vector3 centrocircul = centro.transform.localPosition;
			raio = Vector2.Distance (new Vector2 (centrocircul.x, centrocircul.z), new Vector2 (startPosition.x, startPosition.z));		



			float myAngleInDegrees = 0;

			Vector3 randomCircle = new Vector3 (0, 0, 0);
			Vector3 end =  new Vector3 (0, 0, 0);

			int angulo = 0;
			int xval = 0;

		
		

			if (lado.Equals ("direita")) {

				 myAngleInDegrees = 135f * Mathf.Deg2Rad;
				 randomCircle = new Vector3 (Mathf.Cos (myAngleInDegrees), 0, Mathf.Sin (myAngleInDegrees));
				 end = centro.transform.TransformPoint (randomCircle * raio);

				xval = 135;
				angulo = 180;

			} else {
				 myAngleInDegrees = 45f * Mathf.Deg2Rad;
				 randomCircle = new Vector3 (Mathf.Cos (myAngleInDegrees), 0, Mathf.Sin (myAngleInDegrees));
				 end = centro.transform.TransformPoint (randomCircle * raio);


				xval = 0;
				angulo = 45;
			};


			ArrayList pontos = new ArrayList();
			Vector3 novoponto = new Vector3(0,0,0);
			for(int x = xval ;x < angulo; x++){

				randomCircle = new Vector3 (Mathf.Cos (x * Mathf.Deg2Rad), 0, Mathf.Sin (x * Mathf.Deg2Rad));
				novoponto = centro.transform.TransformPoint(randomCircle * (raio ));
				//Instantiate (cubo, novoponto, Quaternion.identity) ;  
				pontos.Add (novoponto);

			}

			Debug.Log (pontos);

			if (lado.Equals ("direita")) {
				pontos.Reverse();
			}

			StartCoroutine(	fowardAngLeMovement(end,pontos,centrocircul,lado));


		}
		

	public Vector3 testaPonto(ArrayList pontos, Vector3 end, string lado, int graus, int interacao ){

		//Debug.Log ("interação: " + interacao);
		//o graus vai ser o count do array de pontos, para assim fazer o caclulo contrario da rotação
		// a interação vai  servir para saber qdo dos graus diminuir, se interessação for 0, qer dize q esta testando o end point, e a rotação é a rot maxima final ( 90 ou 45)

		Vector3 halfextent = new Vector3 (0.2f, 0.025f, 0.2f);//TODO teste tamanho da nave/2
		Quaternion rot;

		if (lado.Equals ("direita")) {
			rot = Quaternion.Euler (0, (transform.rotation.eulerAngles.y + (graus - interacao)), 0); 
		} else {
			rot = Quaternion.Euler (0, (transform.rotation.eulerAngles.y - (graus - interacao)), 0); 
		}

	
		Collider[] hitColliders = null;
		if (interacao == 0) { //se a interação for zero, vai castar uma box no ponto final pra ve se colide com alguma nave
			//hitColliders = Physics.OverlapBox (end, halfextent, rot, LayerShip, QueryTriggerInteraction.Ignore);
			hitColliders = Physics.OverlapBox (end, halfextent, rot, LayerShip);
		} else if (interacao != graus) {    //senao vai castar uma box no ponto em qestao pra ve se colide com alguma nave
			hitColliders = Physics.OverlapBox ((Vector3)pontos [pontos.Count - interacao - 1], halfextent, rot, LayerShip);
		} else {
			return transform.position;
		}


		bool colide = false;
		for (int x = 0; x < hitColliders.Length; x++) {
			if(!hitColliders[x].Equals(shippcollider)){
				colide = true;
			}
		}

			if (colide) { //se ele acho um collider q é diferente do collider da ship q esta movendo, ele vai tenta outro ponto
				pontos[pontos.Count - interacao - 1] = null;
				return	testaPonto (pontos, end, lado, graus, interacao + 1);		
			} else {
				if (interacao == 0) {
					return end;
				} else {
					return (Vector3)pontos [pontos.Count - interacao - 1];
				}
			}


	}


		public void move_turn( string lado, Transform centro){
			colidiu = false;
			startPosition = spaw_movimento.localPosition; // new Vector3(0,0,0);


			float myAngleInDegrees = 90f * Mathf.Deg2Rad;

			Vector3 centrocircul = centro.transform.localPosition;

			float raio = Vector2.Distance (new Vector2(centrocircul.x,centrocircul.z ),new Vector2(startPosition.x,startPosition.z  ));	
		
			Vector3 randomCircle = new Vector3(Mathf.Cos(myAngleInDegrees), 0,Mathf.Sin(myAngleInDegrees));
			Vector3 end = centro.transform.TransformPoint (randomCircle * raio);

			Instantiate (esfera, end, Quaternion.identity) ;  
	 
			int angulo = 0;
			int xval =  0;


				if(lado.Equals("direita")){
					angulo = 180;
					xval = 90;
				}else{
					xval = 0;
					angulo = 90;
				};
			

			ArrayList pontos = new ArrayList();
			Vector3 novoponto = new Vector3(0,0,0);
			for(int x = xval ;x < angulo; x++){

				randomCircle = new Vector3 (Mathf.Cos (x * Mathf.Deg2Rad), 0, Mathf.Sin (x * Mathf.Deg2Rad));
				novoponto = centro.transform.TransformPoint(randomCircle * (raio ));
				Instantiate (cubo, novoponto, Quaternion.identity) ;  
				pontos.Add (novoponto);

			}




			if (lado.Equals ("direita")) {
				 pontos.Reverse();
			}


			end = 	testaPonto(pontos,end,lado,90,0);
		pontos.TrimToSize();
			StartCoroutine(	fowardAngLeMovement(end,pontos,centrocircul,lado));

		
		} 



		protected IEnumerator fowardsmoothMovement (Vector3 end)
		{

			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			while (sqrRemainingDistance > float.Epsilon && !colidiu) {

				Vector3 newPosition = Vector3.MoveTowards (this.transform.position, end, speed * Time.deltaTime);
				//Instantiate (cubo, newPosition, Quaternion.identity) ; 
				rb.MovePosition (newPosition);
				sqrRemainingDistance = (transform.position  - end).sqrMagnitude;

				yield return null;

			}

		}

		protected IEnumerator fowardAngLeMovement (Vector3 end,ArrayList pontos, Vector3 centro, string lado)
		{

			float sqrRemainingDistance = 0;
			Quaternion rot;
			Vector3 newPosition;

				for(int x = 0;x < pontos.Count; x++){
					if(pontos[x] != null){
							
						
							Vector3 novoponto = (Vector3)pontos [x];  
							sqrRemainingDistance = (transform.position - novoponto).sqrMagnitude;  //usar transofrm do movimento?

							while (sqrRemainingDistance > float.Epsilon && !colidiu) {

								newPosition =  Vector3.MoveTowards (this.transform.position, novoponto, speed * Time.deltaTime);
																																Instantiate (cubo, novoponto, Quaternion.identity) ;  
								rb.MovePosition (newPosition);
								sqrRemainingDistance = (transform.position - novoponto).sqrMagnitude; //usar transofrm do movimento?
								yield return null;

							}

							if (lado.Equals ("direita")) {
								rot = Quaternion.Euler (0, (transform.rotation.eulerAngles.y + (1)), 0); 
							} else {
								rot = Quaternion.Euler (0, (transform.rotation.eulerAngles.y + (-1)), 0); 
							}


							transform.rotation = rot;
					
					}
				}


			//pra ter ctz q chego no ponto final
			sqrRemainingDistance = (transform.position - end).sqrMagnitude; //usar transofrm do movimento?

			while (sqrRemainingDistance > float.Epsilon && !colidiu) {

				newPosition = Vector3.MoveTowards (this.transform.position, end, speed * Time.deltaTime);
																													Instantiate (cubo, newPosition, Quaternion.identity) ;  
				rb.MovePosition (newPosition);
				sqrRemainingDistance = (transform.position - end).sqrMagnitude; //usar transofrm do movimento?
				yield return null;

			}

		}



		void OnTriggerEnter(Collider other) {

		/*
			if (other.CompareTag ("Shipp"))   {
				colidiu = true;
				return;
			}
		*/

		
		}


		// Update is called once per frame

	}

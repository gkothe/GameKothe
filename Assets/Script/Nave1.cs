﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Nave1 : ShipScript {



	//public Transform centre;

	//public Vector3 playerRadius = new Vector3(0, 0.5f, -5);





	// Use this for initialization
	void Start () {
		
		base.carregaComponentes ();
		//move_turn(3,"esquerda");

	 
	

	}

	/*
	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.velocity = movement * 5f;

		rb.position = new Vector3 (
			rb.position.x,
			0.0f,
			rb.position.z
		);

	//	rb.rotation = Quaternion.Euler (0f,0.0f,0.45f );
		//rb.AddForce(movement * speed);
	}
	*/


	public  float turnSpeed = 1.5f; // rotation speed control


	void Update () {


		//Instantiate (cubo, this.transform.position, Quaternion.identity) ; 

	

		/*
		// update direction each frame:
		Vector3  dir = pontocentral.position - transform.position;

		Instantiate (cilindro, dir, Quaternion.identity) ; 
		// calculate desired rotation:
		Quaternion  rot = Quaternion.LookRotation(dir);
		// Slerp to it over time:
		transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.5f * Time.deltaTime);
		// move in the current forward direction at specified speed:
		transform.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
		*/

	}

	public void OptionsMovimento(){
		dropMovimento.ClearOptions();
		dropMovimento.options.Add (new Dropdown.OptionData() {text="---"});//0
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Foward 1"});//1
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Foward 2"});//2
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Foward 3"});//3
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Foward 4"});//4
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Foward 5"});//5
		dropMovimento.options.Add (new Dropdown.OptionData() {text="KeyTurn 1"});//6
		dropMovimento.options.Add (new Dropdown.OptionData() {text="KeyTurn 2"});//7
		dropMovimento.options.Add (new Dropdown.OptionData() {text="KeyTurn 3"});//8
		dropMovimento.options.Add (new Dropdown.OptionData() {text="KeyTurn 4"});//9
		dropMovimento.options.Add (new Dropdown.OptionData() {text="KeyTurn 5"});//10
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Right 1"}); //11
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Right 2"}); //12
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Right 3"}); //13
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Left 1"}); //14
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Left 2"}); //15
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Left 3"}); //16

		dropMovimento.options.Add (new Dropdown.OptionData() {text="Bank Right 1"}); //17
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Bank Right 2"}); //18
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Bank Right 3"}); //19
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Bank Left 1"}); //20
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Bank Left 2"}); //21
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Bank Left 3"}); //22

	}


	public void movimento(){

		if (dropMovimento.value == 1) {
			move_foward (1);
		}else  if (dropMovimento.value == 2) {
			move_foward (2);
		}else  if (dropMovimento.value == 3) {
			move_foward (3);
		}else  if (dropMovimento.value == 4) {
			move_foward (4);
		}else  if (dropMovimento.value == 5) {
			move_foward (5);
		}else  if (dropMovimento.value == 6) {
			move_keyturn (1);
		}else  if (dropMovimento.value == 7) {
			move_keyturn (2);	
		}else  if (dropMovimento.value == 8) {
			move_keyturn (3);
		}else  if (dropMovimento.value == 9) {
			move_keyturn (4);
		}else  if (dropMovimento.value == 10) {
			move_keyturn (5);
		}else  if (dropMovimento.value == 11) {
			move_turn ("direita",turnright1);
		}else  if (dropMovimento.value == 12) {
			move_turn ("direita",turnright2);
		}else  if (dropMovimento.value == 13) {
			move_turn("direita",turnright3);
		}else  if (dropMovimento.value == 14) {
			move_turn("esquerda",turnleft1);
		}else  if (dropMovimento.value == 15) {
			move_turn("esquerda",turnleft2);
		}else  if (dropMovimento.value == 16) {
			move_turn("esquerda",turnleft3);
		}


		else  if (dropMovimento.value == 17) {
			move_Bank ("direita",Bankright1);
		}else  if (dropMovimento.value == 18) {
			move_Bank ("direita",Bankright2);
		}else  if (dropMovimento.value == 19) {
			move_Bank("direita",Bankright3);
		}else  if (dropMovimento.value == 20) {
			move_Bank("esquerda",Bankleft1);
		}else  if (dropMovimento.value == 21) {
			move_Bank("esquerda",Bankleft2);
		}else  if (dropMovimento.value == 22) {
			move_Bank ( "esquerda", Bankleft3);
		}

	}


}

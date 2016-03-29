using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Nave1 : ShipScript {



	// Use this for initialization
	void Start () {
		
		base.carregaComponentes ();


	}

	void Update () {

		Instantiate (cubo, this.transform.position, Quaternion.identity) ; 


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
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Left 1"}); //13
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Left 2"}); //14
		dropMovimento.options.Add (new Dropdown.OptionData() {text="Turn Left 3"}); //15

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
			move_turn (1,"direita");
		}else  if (dropMovimento.value == 12) {
			move_turn (2,"direita");
		}else  if (dropMovimento.value == 13) {
			move_turn(3,"direita");
		}else  if (dropMovimento.value == 14) {
			move_turn(1,"esquerda");
		}else  if (dropMovimento.value == 15) {
			move_turn(2,"esquerda");
		}else  if (dropMovimento.value == 16) {
			move_turn(3,"esquerda");
		}

	}


}

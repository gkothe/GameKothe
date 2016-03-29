using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEngine.UI;

public class GM : MonoBehaviour {

	public GameObject board;
	public Button btnGo;
	public Dropdown dropMovimento;
	private GameObject SelectedPiece;	// Selected Piece
	private Component SelectedPiece_script;	// Selected Piece
	public Camera PlayerCam;			// Camera used by the player
	private Type script;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetMouseInputs();
	}
		
	public void GoBotao(){

		MethodInfo theMethod = script.GetMethod("movimento");
		theMethod.Invoke (SelectedPiece_script,null);

	}
		
	void GetMouseInputs()
	{	
		Ray _ray;
		RaycastHit _hitInfo;

		// Select a piece if the gameState is 0 or 1

			// On Left Click
			if(Input.GetMouseButtonDown(0))
			{
				_ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click

				// Raycast and verify that it collided
				if(Physics.Raycast (_ray,out _hitInfo))
				{
					// Select the piece if it has the good Tag
					
				if(_hitInfo.collider.gameObject.tag == ("Shipp"))
				{

					SelectPiece(_hitInfo.collider.gameObject); 
				}

					
				}
			}

	}

	private void SelectPiece(GameObject _PieceToSelect)
	{

		dropMovimento.ClearOptions ();
		// Unselect the piece if it was already selected
		if(_PieceToSelect  == SelectedPiece)
		{
			SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
			SelectedPiece = null;

		}
		else
		{
			// Change color of the selected piece to make it apparent. Put it back to white when the piece is unselected
			if(SelectedPiece)
			{
				SelectedPiece.GetComponent<Renderer>().material.color = Color.white;
			}
			SelectedPiece = _PieceToSelect;
			SelectedPiece.GetComponent<Renderer>().material.color = Color.blue;


			script = Type.GetType (SelectedPiece.name);//obs:Nome do prefab = nome do script
			SelectedPiece_script = SelectedPiece.GetComponent(script);
			MethodInfo theMethod = script.GetMethod("OptionsMovimento");
			theMethod.Invoke (SelectedPiece_script,null);

		}
	}
}

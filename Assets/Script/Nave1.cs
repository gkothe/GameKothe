using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Nave1 : ShipScript
{

    // Use this for initialization
    void Start()
    {

        base.carregaComponentes();

    }



    public IEnumerator rotate()
    {

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + (1f)), 0);

        }

    }




    void Update()
    {

        /*
                if (Input.GetMouseButton(1)) {
                    Tiro_basic ();

                }
                if (Input.GetMouseButton(0)) {

                    transform.rotation =  Quaternion.Euler (0, (transform.rotation.eulerAngles.y + (1f )), 0); 
                }

            */
    }

    public void OptionsMovimento()
    {
        dropMovimento.ClearOptions();


        dropMovimento.options.Add(new Dropdown.OptionData() { text = "---" });//0
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 1" });//1
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 2" });//2
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 3" });//3
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 4" });//4
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 5" });//5
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 1" });//6
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 2" });//7
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 3" });//8
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 4" });//9
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 5" });//10
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Right 1" }); //11
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Right 2" }); //12
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Right 3" }); //13
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Left 1" }); //14
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Left 2" }); //15
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Left 3" }); //16

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Right 1" }); //17
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Right 2" }); //18
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Right 3" }); //19
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Left 1" }); //20
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Left 2" }); //21
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Left 3" }); //22
        dropMovimento.value = 0;

    }


    public void movimento()
    {

        if (dropMovimento.value == 1)
        {
            move_foward(1);
        }
        else if (dropMovimento.value == 2)
        {
            move_foward(2);
        }
        else if (dropMovimento.value == 3)
        {
            move_foward(3);
        }
        else if (dropMovimento.value == 4)
        {
            move_foward(4);
        }
        else if (dropMovimento.value == 5)
        {
            move_foward(5);
        }
        else if (dropMovimento.value == 6)
        {
            move_keyturn(1);
        }
        else if (dropMovimento.value == 7)
        {
            move_keyturn(2);
        }
        else if (dropMovimento.value == 8)
        {
            move_keyturn(3);
        }
        else if (dropMovimento.value == 9)
        {
            move_keyturn(4);
        }
        else if (dropMovimento.value == 10)
        {
            move_keyturn(5);
        }
        else if (dropMovimento.value == 11)
        {
            move_turn("direita", turnright1);
        }
        else if (dropMovimento.value == 12)
        {
            move_turn("direita", turnright2);
        }
        else if (dropMovimento.value == 13)
        {
            move_turn("direita", turnright3);
        }
        else if (dropMovimento.value == 14)
        {
            move_turn("esquerda", turnleft1);
        }
        else if (dropMovimento.value == 15)
        {
            move_turn("esquerda", turnleft2);
        }
        else if (dropMovimento.value == 16)
        {
            move_turn("esquerda", turnleft3);
        }


        else if (dropMovimento.value == 17)
        {
            move_Bank("direita", Bankright1);
        }
        else if (dropMovimento.value == 18)
        {
            move_Bank("direita", Bankright2);
        }
        else if (dropMovimento.value == 19)
        {
            move_Bank("direita", Bankright3);
        }
        else if (dropMovimento.value == 20)
        {
            move_Bank("esquerda", Bankleft1);
        }
        else if (dropMovimento.value == 21)
        {
            move_Bank("esquerda", Bankleft2);
        }
        else if (dropMovimento.value == 22)
        {
            move_Bank("esquerda", Bankleft3);
        }

    }


}

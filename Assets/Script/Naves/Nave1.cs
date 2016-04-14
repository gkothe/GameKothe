using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Nave1 : ShipScript
{

    // Use this for initialization
    void Start()
    {
     //   funStart(); //usar qdo usar naves direto pelo editor

    }
    public void funStart() {
        base_size = 0.4f;
        HealthIni = 100;
        ShieldIni = 50;
        base.carregaComponentes();
        OptionsMovimento(); //carrega os movimentos na classe
        dropMovimento.ClearOptions();
        OptionsAcao(); //carrega as ação na classe
        dropMovimento.ClearOptions();

        //StartCoroutine(rotate());

    }


    public IEnumerator rotate()
    {

        while (true)
        {
            
            transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + (1f)), 0);
            yield return new WaitForSeconds(0.05f);
        }

    }

    
    void Update() { 
        
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

        movimentos = new Dictionary<string, int>();


        int cont = 1;


        dropMovimento.options.Add(new Dropdown.OptionData() { text = "---" });//0
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 1" });//1
        movimentos.Add("Foward 1", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 2" });//2
        movimentos.Add("Foward 2", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 3" });//3
        movimentos.Add("Foward 3", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 4" });//4
        movimentos.Add("Foward 4", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Foward 5" });//5
        movimentos.Add("Foward 5", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 1" });//6
        movimentos.Add("KeyTurn 1", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 2" });//7
        movimentos.Add("KeyTurn 2", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 3" });//8
        movimentos.Add("KeyTurn 3", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 4" });//9
        movimentos.Add("KeyTurn 4", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "KeyTurn 5" });//10
        movimentos.Add("KeyTurn 5", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Right 1" }); //11
        movimentos.Add("Turn Right 1", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Right 2" }); //12
        movimentos.Add("Turn Right 2", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Right 3" }); //13
        movimentos.Add("Turn Right 3", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Left 1" }); //14
        movimentos.Add("Turn Left 1", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Left 2" }); //15
        movimentos.Add("Turn Left 2", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Turn Left 3" }); //16
        movimentos.Add("Turn Left 3", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Right 1" }); //17
        movimentos.Add("Bank Right 1", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Right 2" }); //18
        movimentos.Add("Bank Right 2", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Right 3" }); //19
        movimentos.Add("Bank Right 3", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Left 1" }); //20
        movimentos.Add("Bank Left 1", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Left 2" }); //21
        movimentos.Add("Bank Left 2", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Bank Left 3" }); //22
        movimentos.Add("Bank Left 3", cont++);

        dropMovimento.value = movimento_armazenado;
        dropMovimento.RefreshShownValue();

    }
    
    public void OptionsAcao()
    {
        dropMovimento.ClearOptions();

        acoes = new Dictionary<string, int>();
        
        int cont = 1;
        
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Escolha uma ação" });//0
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Evasão - 10%" });//1
        acoes.Add("Evasão - 10%", cont++);

        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Concentração - 10%" });//2
        acoes.Add("Concentração - 10%", cont++);
        
        dropMovimento.options.Add(new Dropdown.OptionData() { text = "Damage - 10%" });//4
        acoes.Add("Damage - 10%", cont++);

       
        dropMovimento.value = acao_armazenada;
        dropMovimento.RefreshShownValue();

    }
    
    public void movimento()
    {

        if (!gm.emMovimento)
        {
            gm.emMovimento = true;

            if (movimento_armazenado == (int)movimentos["Foward 1"])
            {
                move_foward(1);
            }
            else if (movimento_armazenado == (int)movimentos["Foward 2"])
            {
                move_foward(2);
            }
            else if (movimento_armazenado == (int)movimentos["Foward 3"])
            {
                move_foward(3);
            }
            else if (movimento_armazenado == (int)movimentos["Foward 4"])
            {
                move_foward(4);
            }
            else if (movimento_armazenado == (int)movimentos["Foward 5"])
            {
                move_foward(5);
            }

            else if (movimento_armazenado == (int)movimentos["KeyTurn 1"])
            {
                move_keyturn(1);
            }
            else if (movimento_armazenado == (int)movimentos["KeyTurn 2"])
            {
                move_keyturn(2);
            }
            else if (movimento_armazenado == (int)movimentos["KeyTurn 3"])
            {
                move_keyturn(3);
            }
            else if (movimento_armazenado == (int)movimentos["KeyTurn 4"])
            {
                move_keyturn(4);
            }
            else if (movimento_armazenado == (int)movimentos["KeyTurn 5"])
            {
                move_keyturn(5);
            }

            else if (movimento_armazenado == (int)movimentos["Turn Right 1"])
            {
                move_turn("direita", turnright1);
            }
            else if (movimento_armazenado == (int)movimentos["Turn Right 2"])
            {
                move_turn("direita", turnright2);
            }
            else if (movimento_armazenado == (int)movimentos["Turn Right 3"])
            {
                move_turn("direita", turnright3);
            }
            else if (movimento_armazenado == (int)movimentos["Turn Left 1"])
            {
                move_turn("esquerda", turnleft1);
            }
            else if (movimento_armazenado == (int)movimentos["Turn Left 2"])
            {
                move_turn("esquerda", turnleft2);
            }
            else if (movimento_armazenado == (int)movimentos["Turn Left 3"])
            {
                move_turn("esquerda", turnleft3);
            }

            else if (movimento_armazenado == (int)movimentos["Bank Right 1"])
            {
                move_Bank("direita", Bankright1);
            }
            else if (movimento_armazenado == (int)movimentos["Bank Right 2"])
            {
                move_Bank("direita", Bankright2);
            }
            else if (movimento_armazenado == (int)movimentos["Bank Right 3"])
            {
                move_Bank("direita", Bankright3);
            }
            else if (movimento_armazenado == (int)movimentos["Bank Left 1"])
            {
                move_Bank("esquerda", Bankleft1);
            }
            else if (movimento_armazenado == (int)movimentos["Bank Left 2"])
            {
                move_Bank("esquerda", Bankleft2);
            }
            else if (movimento_armazenado == (int)movimentos["Bank Left 3"])
            {
                move_Bank("esquerda", Bankleft3);
            }
            

        //    afterMovimento();
           faseAcao();
        }
    }

}

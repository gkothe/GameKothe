using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour {

    public string nomeArma ;
    public int atkmin ;
    public int atkmax ;
    public float SP;//shield piercing
    public float baseprecision ;

    public float getBaseprecision() {
        return baseprecision;
    }

    public float getSP()
    {
        return SP;
    }

    public int getAtkmin()
    {
        return atkmin;
    }

    public int getAtkmax()
    {
        return atkmax;
    }

    public string getNomeArma()
    {
        return nomeArma;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCounter : MonoBehaviour {

    public GameObject full;
    public GameObject empty;

    public void setFull() 
    {
        full.SetActive(true);
        empty.SetActive(false);
    }

    public void setEmpty()
    {
        full.SetActive(false);
        empty.SetActive(true);
    }

}

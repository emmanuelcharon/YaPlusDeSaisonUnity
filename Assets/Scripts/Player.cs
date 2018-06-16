using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int gameScore;
	public int roundScore;

    public Vector3 initialPosition;
    public Vector3 initialScale;

	public void Awake()
	{
        initialPosition = this.transform.position;
        initialScale = this.transform.localScale;
	}

}

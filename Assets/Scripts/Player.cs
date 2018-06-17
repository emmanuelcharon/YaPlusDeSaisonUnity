using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int gameScore;
	public int roundScore;

    public Vector3 initialPosition;
    public Vector3 initialScale;

    public GameObject focus;
    public SpriteRenderer lightOn;

	public void Awake()
	{
        initialPosition = this.transform.position;
        initialScale = this.transform.localScale;
	}
	public void Start()
	{
        iTween.FadeTo(lightOn.gameObject, iTween.Hash(
            "alpha", 0f, "time", 0.5f, 
            "easetype", iTween.EaseType.linear,
            "looptype", iTween.LoopType.pingPong
        ));

        SetFocus(false);
	}

    public void SetFocus(bool on) {
        focus.gameObject.SetActive(on);
    }






}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FruitGO : MonoBehaviour, IPointerClickHandler {

	
	public Global.FruitType fruitType;
	[HideInInspector] public GameManager gm;

	public void Start() {
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		Debug.Log("Clicked a " + fruitType + ". Active : " + gm.activePlayer.name);
        if (gm.playerCanPlay)
        {
            gm.playerPickedFruit(this);
        }
	}




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoFruit : MonoBehaviour, IPointerClickHandler {

    [HideInInspector] public GameManager gm;

    public void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Clicked the NoFruit option. Active : " + gm.activePlayer.name);
        if(gm.playerCanPlay) {
            gm.playerPickedNoFruit();
        }
    }
}

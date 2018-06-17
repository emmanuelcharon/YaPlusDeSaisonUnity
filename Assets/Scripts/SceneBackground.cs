using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBackground : MonoBehaviour {

    public SpriteRenderer spring;
    public SpriteRenderer summer;
    public SpriteRenderer autumn;
    public SpriteRenderer winter;
    [HideInInspector] SpriteRenderer currentFocus;
    [HideInInspector] Global.Season currentFocusSeason;


	void Awake () {
        foreach (SpriteRenderer sr in new SpriteRenderer[] { summer, autumn, winter }) {
            iTween.FadeTo(sr.gameObject, iTween.Hash(
            "alpha", 0f, "time", 0.1f, "easetype", iTween.EaseType.linear));
        }
        currentFocus = spring;
	}
	
    public void setFocus(Global.Season season) {
        if(currentFocusSeason == season) {
            return;
        }    

        if(currentFocus != null) {
            iTween.FadeTo(currentFocus.gameObject, iTween.Hash(
                "alpha", 0f, "time", 1f, "easetype", iTween.EaseType.linear));
        }

        currentFocusSeason = season;
        currentFocus = getBackgroundForSeason(season);
       
        iTween.FadeTo(currentFocus.gameObject, iTween.Hash(
                "alpha", 1f, "time", 1f, "easetype", iTween.EaseType.linear));


    }

    SpriteRenderer getBackgroundForSeason(Global.Season season) {
        switch (season)
        {
            case Global.Season.Spring:
                return spring;
            case Global.Season.Summer:
                return summer;
            case Global.Season.Autumn:
                return autumn;
            case Global.Season.Winter:
                return winter;
            default:
                Debug.LogError(season);
                return null;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource spring, summer, autumn, winter;
    public AudioSource boo, cheer, crown, money;


    public void ChangeMusic(Global.Season season) {

        spring.Stop();
        summer.Stop();
        autumn.Stop();
        winter.Stop();

        GetMusicForSeason(season).Play();
    }

    AudioSource GetMusicForSeason(Global.Season season)
    {
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
                Debug.LogError("season " + season);
                return null;
        }
    }


}

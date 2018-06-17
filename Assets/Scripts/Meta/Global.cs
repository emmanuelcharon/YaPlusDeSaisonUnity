using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

    public static int SceneIndex_Menu = 0;
    public static int SceneIndex_Main = 1;

    public enum Season { Spring, Summer, Autumn, Winter };

    public enum FruitType { Carrot, Peas, Garlic, Broccoli, Spinach, Cucumber, Tomatoe, Strawberry, Corn, Artichoke, Cauliflower, Mushroom, Grapes, Pumkin, Beetroot, SweetPotatoe, Orange, Pear, Cabbage, BrusselSprout };

    public static Global s;

    public SoundManager sounds;

	void Awake()
	{
        if (s!=null) {
            Destroy(this.gameObject);
            return;
        }

        s = this;
        DontDestroyOnLoad(this.gameObject);
	}


    public static Season GetSeasonFromRoundNumber(int round) {

        switch (round%4) 
        {
            case 1:
                return Season.Spring;
            case 2:
                return Season.Summer;
            case 3:
                return Season.Autumn;
            case 0:
                return Season.Winter;
            default:
                Debug.LogError("round " + round);
                return Season.Spring;
        }
    }

    public static List<FruitType> GetFruitsForSeason(Season season) {
        switch (season)
        {
            case Season.Spring:
                return new List<FruitType>() { FruitType.Carrot, FruitType.Peas, FruitType.Garlic, FruitType.Broccoli, FruitType.Spinach };
            case Season.Summer:
                return new List<FruitType>() { FruitType.Cucumber, FruitType.Tomatoe, FruitType.Strawberry, FruitType.Corn, FruitType.Artichoke };
            case Season.Autumn:
                return new List<FruitType>() { FruitType.Cauliflower, FruitType.Mushroom, FruitType.Grapes, FruitType.Pumkin, FruitType.Beetroot };
            case Season.Winter:
                return new List<FruitType>() { FruitType.SweetPotatoe, FruitType.Orange, FruitType.Pear, FruitType.Cabbage, FruitType.BrusselSprout };
            default:
                Debug.LogError("season " + season);
                return null;
        }
    }

    public static bool IsFruitInSeason(FruitType fruit, Season season)
    {
        return GetFruitsForSeason(season).Contains(fruit);
    }

    public static Season SeasonForFruit(FruitType ft) {
        foreach(Season season in System.Enum.GetValues(typeof(Season))) {
            if (GetFruitsForSeason(season).Contains(ft))
            {
                return season;
            }
        }
        Debug.LogError(ft);
        return Season.Spring;
    }

    public static string SeasonName(Season season) {
        switch (season)
        {
            case Season.Spring:
                return "Printemps";
            case Season.Summer:
                return "Ete";
            case Season.Autumn:
                return "Automne";
            case Season.Winter:
                return "Hiver";
            default:
                Debug.LogError("season " + season);
                return null;
        }
    } 

    public static string FruitName(FruitType fruit) {
        switch (fruit)
        {
            case FruitType.Carrot:
                return "Carotte";
            case FruitType.Peas:
                return "Petits pois";
            case FruitType.Garlic:
                return "Ail";
            case FruitType.Broccoli:
                return "Brocoli";
            case FruitType.Spinach:
                return "Epinard";
            case FruitType.Cucumber:
                return "Concombre";
            case FruitType.Tomatoe:
                return "Tomate";
            case FruitType.Strawberry:
                return "Fraise";
            case FruitType.Corn:
                return "Mais";
            case FruitType.Artichoke:
                return "Artichaut";
            case FruitType.Cauliflower:
                return "Choux Fleurs";
            case FruitType.Mushroom:
                return "Champignon";
            case FruitType.Grapes:
                return "Raisins";
            case FruitType.Pumkin:
                return "Citrouille";
            case FruitType.Beetroot:
                return "Betterave";
            case FruitType.SweetPotatoe:
                return "Patate Douce";
            case FruitType.Orange:
                return "Orange";
            case FruitType.Pear:
                return "Poire";
            case FruitType.Cabbage:
                return "Chou";
            case FruitType.BrusselSprout:
                return "Choux de Bruxelles";
            default:
                Debug.LogError("fruit type " + fruit);
                return null;
        }
    }

    public static FruitType getRandomFruit()
    {
        //return FruitType.Cauliflower;
        var fruitTypes = System.Enum.GetValues(typeof(FruitType));
        return (FruitType) fruitTypes.GetValue(Random.Range(0, fruitTypes.Length));
    }

    public static FruitType getRandomFruitBiased(Season currentSeason) {

        if(Random.value < 0.25f) {
            var fruitTypes = GetFruitsForSeason(currentSeason);
            return (FruitType) fruitTypes[Random.Range(0, fruitTypes.Count)];
        } else {
            return getRandomFruit();
        }

    }


}

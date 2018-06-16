using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefabs : MonoBehaviour {

    // spring
    public GameObject carrot;
    public GameObject peas;
    public GameObject garlic;
    public GameObject broccoli;
    public GameObject spinach;
    
    // summer
    public GameObject cucumber;
    public GameObject tomatoe;
    public GameObject strawberry;
    public GameObject corn;
    public GameObject artichoke;

    // autumn
    public GameObject cauliflower;
    public GameObject mushroom;
    public GameObject grapes;
    public GameObject pumkin;
    public GameObject beetroot;

    // winter
    public GameObject sweetPotato;
    public GameObject orange;
    public GameObject pear;
    public GameObject cabbage;
    public GameObject brusselSprout;


    public GameObject FruitPrefab(Global.FruitType fruit) {
        switch (fruit)
        {
            case Global.FruitType.Carrot:
                return carrot;
            case Global.FruitType.Peas:
                return peas;
            case Global.FruitType.Garlic:
                return garlic;
            case Global.FruitType.Broccoli:
                return broccoli;
            case Global.FruitType.Spinach:
                return spinach;
            case Global.FruitType.Cucumber:
                return cucumber;
            case Global.FruitType.Tomatoe:
                return tomatoe;
            case Global.FruitType.Strawberry:
                return strawberry;
            case Global.FruitType.Corn:
                return corn;
            case Global.FruitType.Artichoke:
                return artichoke;
            case Global.FruitType.Cauliflower:
                return cauliflower;
            case Global.FruitType.Mushroom:
                return mushroom;
            case Global.FruitType.Grapes:
                return grapes;
            case Global.FruitType.Pumkin:
                return pumkin;
            case Global.FruitType.Beetroot:
                return beetroot;
            case Global.FruitType.SweetPotatoe:
                return sweetPotato;
            case Global.FruitType.Orange:
                return orange;
            case Global.FruitType.Pear:
                return pear;
            case Global.FruitType.Cabbage:
                return cabbage;
            case Global.FruitType.BrusselSprout:
                return brusselSprout;
            default:
                Debug.LogError("fruit type " + fruit);
                return null;
        }
    }

}

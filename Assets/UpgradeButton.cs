using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public int price;
    public Text txtMult;
    public Text txtCoin;

    private int mult;
    private int coinAll;

    private void OnEnable()
    {
        mult = PlayerPrefs.GetInt("multiplier", 1);
        transform.GetChild(0).GetComponent<Text>().text += "X" + (mult + 1).ToString();
        gameObject.GetComponent<Button>().interactable = (price < PlayerPrefs.GetInt("coins", 0));
    }

    public void OnClick()
    {
        coinAll = PlayerPrefs.GetInt("coins", 0) - 50;
        PlayerPrefs.SetInt("coins", coinAll);
        PlayerPrefs.SetInt("multiplier", ++mult);
        PlayerPrefs.Save();
        gameObject.GetComponent<Button>().interactable = (price < coinAll);
        txtMult.text = "X" + mult.ToString();
        txtCoin.text = coinAll.ToString();
    }
}

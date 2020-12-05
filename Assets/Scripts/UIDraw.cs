using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIDraw : MonoBehaviour
{
    public Text _best;
    public Text _mult;

    [HideInInspector]
    public bool gameOver = false;
    public static UIDraw instance;

    int best = 0;

    void Start()
    {
        instance = this;

        best = PlayerPrefs.GetInt("bestScore", 0);
        _best.text = best.ToString();
        _mult.text = "X"+PlayerPrefs.GetInt("multiplier", 1).ToString();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}

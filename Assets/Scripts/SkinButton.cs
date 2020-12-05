using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkinButton : MonoBehaviour
{
    GameObject player;
    public string skinName;
    public int value = 0;
   
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnClick()
    {
        GameObject obj = Resources.Load("skin/"+skinName) as GameObject;
        player.transform.GetChild(0).GetComponentInChildren<MeshFilter>().mesh = obj.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        PlayerPrefs.SetString("skin", skinName);
    }

    private void OnEnable()
    {
       GetComponent<Button>().interactable = (value < PlayerPrefs.GetInt("bestScore", 0));
    }
}

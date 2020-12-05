using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint;

    public PlatformTile tilePrefab; 
    public float movingSpeed = 12;
    public int tilesToPreSpawn = 20; //количество тайлов в буфере
    public int tilesWithoutObstacles = 5; //количество тайлов без препятствий

    List<PlatformTile> spawnedTiles = new List<PlatformTile>();
    int nextTileToActivate = -1;
    [HideInInspector]
    public bool gameOver = false;
    static bool gameStarted = false;
    public Text scoreTxt;
    public int score = 0;
    int multiplier;
    int scoreboost = 0;

    public static GroundGenerator instance;

    void Start()
    {
        instance = this;

        multiplier = PlayerPrefs.GetInt("multiplier", 1);
        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= tilePrefab.startPoint.localPosition;
            PlatformTile spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity) as PlatformTile;
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAllObstacles();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandomObstacle();
            }

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed), Space.World);
            score += multiplier;
            scoreboost++;
            scoreTxt.text = score.ToString();
            
            if (scoreboost > 1000)
            {
                movingSpeed += 5;
                scoreboost = 0;
            }
        }

        if (mainCamera.WorldToViewportPoint(spawnedTiles[0].endPoint.position).z < 0)
        {
            PlatformTile tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.ActivateRandomObstacle();
            spawnedTiles.Add(tileTmp);
        }
    }
}
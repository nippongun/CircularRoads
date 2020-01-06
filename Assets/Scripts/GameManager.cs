using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    static GameManager instance;
    public RoadCreator road;
    public PlayerController player;
    private void Awake()
    {
        StartGame();
        road = GameObject.Find("RoadCreator").GetComponent<RoadCreator>();
        road.GenerateRoad();

        player = player.GetComponent<PlayerController>();
        player.transform.position = new Vector2(road.path[0].x + 1, road.path[0].y);
        player.transform.rotation = Quaternion.Euler(road.path[2] - road.path[0]);         
    }
    private void StartGame()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

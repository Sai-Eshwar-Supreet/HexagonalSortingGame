using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance => instance;
    private static LevelManager instance;

    [SerializeField] List<LevelData> levelDataList;
    
    public LevelData CurrentLevelData => levelDataList[GameManager.CurrentLevel - 1];


    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }
}

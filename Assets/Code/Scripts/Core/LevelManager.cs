using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance => instance;
    private static LevelManager instance;

    [SerializeField] List<LevelData> levelDataList;

    private int currentLevel;
    
    public LevelData CurrentLevelData => levelDataList[currentLevel - 1];


    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
        currentLevel = 1;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{
    Menu,
    Play,
    Pause
}

[DisallowMultipleComponent]
[DefaultExecutionOrder(-2)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<GameState> OnStateChange;


    [Header("Game Progression")]
    [SerializeField] private AnimationCurve progressionCurve;
    [SerializeField] private List<ObjectFillController> fillControllers;

    [Header("References")]
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject levelObject;

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnergyManager energyManager;

    [Header("UI")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private ButtonPressHandler fillButton;

    private int playerLevel;
    private int playerExp;
    private int nextLevelExp;
    private float currentProgress = 0;

    private int tempExp = 0;

    public static GameState CurrentState { get; private set; }
    public static int CurrentLevel => Instance.playerLevel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else enabled = false;

        Application.targetFrameRate = 60;

        playerExp = PlayerPrefs.GetInt("PlayerExp", 0);
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        nextLevelExp = (int)progressionCurve.Evaluate(playerLevel + 1);
    }

    private void Start()
    {
        UpdaateFillColliders();
    }
    private void OnEnable()
    {
        fillButton.OnPressStateChange += UpdateBuildingFillProgress;
    }
    private void OnDisable()
    {
        fillButton.OnPressStateChange -= UpdateBuildingFillProgress;
    }

    private void UpdaateFillColliders()
    {
        for(int i = 0; i < fillControllers.Count; i++)
        {
            float previousExpMilestone = progressionCurve.Evaluate(playerLevel);
            currentProgress = (playerExp - previousExpMilestone) / (nextLevelExp - previousExpMilestone);
            float progress = (i < (playerLevel - 1)) ? 1 : (i == playerLevel - 1) ? currentProgress : 0;
            fillControllers[i].SetProgress(progress);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerExp", playerExp);
    }

    public void PlayGame()
    {
        if (energyManager.UseEnergy())
        {
            CurrentState = GameState.Play;
            tempExp = 0;
            OnStateChange?.Invoke(CurrentState);
            menuObject.SetActive(false);
            levelObject.SetActive(true);
        }
    }
    public void PauseGame()
    {
        CurrentState = GameState.Pause;
        OnStateChange?.Invoke(CurrentState);
    }

    public void ReplayGame()
    {
        levelObject.SetActive(false);
        levelObject.SetActive(true);
    }

    public void ExitGame()
    {
        CurrentState = GameState.Menu;
        OnStateChange?.Invoke(CurrentState);
        menuObject.SetActive(true);
        levelObject.SetActive(false);
    }

    public void AddExp(int exp)
    {
        tempExp += exp;
        if(playerExp + tempExp >= nextLevelExp)
        {
            CarouselNavigator.LockCarouselAt(playerLevel);
            playerLevel++;
            nextLevelExp = (int)progressionCurve.Evaluate(playerLevel + 1);
            playerExp += tempExp;
            GameManager.Instance.ExitGame();
            PlayerPrefs.SetInt("PlayerExp", playerExp);
            PlayerPrefs.SetInt("PlayerLevel", playerLevel);
            fillButton.gameObject.SetActive(true);
        }

        float previousExpMilestone = progressionCurve.Evaluate(playerLevel);
        currentProgress = (playerExp + tempExp - previousExpMilestone) / (nextLevelExp - previousExpMilestone);
        progressSlider.value = currentProgress;
    }
    private void UpdateBuildingFillProgress(bool status)
    {
        fillControllers[CurrentLevel - 2].UpdateProgress(status, () => {
            fillButton.gameObject.SetActive(false);
            CarouselNavigator.UnlockCarousel();
        });
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}

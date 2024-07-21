using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private int maxEnergy = 5;
    [SerializeField] private float refillRate = 1f; // refill rate in minutes
    [SerializeField] private TextMeshProUGUI currentEnergyText;
    [SerializeField] private TextMeshProUGUI refillTimeText;
    public int CurrentEnergy { get; private set; }

    private DateTime nextRefillTime;

    private readonly WaitForSeconds waitForASecond = new(1);

    private void Start()
    {
        LoadEnergyState();
        StartCoroutine(RefillEnergy());
    }

    private void LoadEnergyState()
    {
        CurrentEnergy = PlayerPrefs.GetInt("CurrentEnergy", maxEnergy);
        currentEnergyText.SetText(CurrentEnergy.ToString());

        string nextRefillTimeString = PlayerPrefs.GetString("NextRefillTime", DateTime.Now.AddMinutes(refillRate).ToString());
        nextRefillTime = DateTime.Parse(nextRefillTimeString);

        UpdateEnergyAfterTimeElapsed();
    }

    private void UpdateEnergyAfterTimeElapsed()
    {
        if (CurrentEnergy >= maxEnergy) return;

        TimeSpan timeElapsed = DateTime.Now - nextRefillTime;
        if (timeElapsed.TotalSeconds > 0)
        {
            AddEnergy(Mathf.FloorToInt((float)timeElapsed.TotalMinutes / refillRate));

            if (CurrentEnergy < maxEnergy)
            {
                nextRefillTime = DateTime.Now.AddMinutes(refillRate);
            }
            else
            {
                nextRefillTime = DateTime.Now;
            }
        }

        currentEnergyText.SetText(CurrentEnergy.ToString());
    }

    private IEnumerator RefillEnergy()
    {
        while (true)
        {
            if (CurrentEnergy < maxEnergy)
            {
                if (DateTime.Now < nextRefillTime)
                {
                    SetFormattedTime((nextRefillTime - DateTime.Now).TotalSeconds);
                }
                else
                {
                    AddEnergy(1);
                    nextRefillTime = DateTime.Now.AddMinutes(refillRate);
                    PlayerPrefs.SetString("NextRefillTime", nextRefillTime.ToString());

                    if (CurrentEnergy == maxEnergy) refillTimeText.gameObject.SetActive(false);
                }
            }

            currentEnergyText.SetText(CurrentEnergy.ToString());
            yield return waitForASecond;
        }
    }

    public bool UseEnergy()
    {
        if (CurrentEnergy > 0)
        {
            CurrentEnergy -= 1;
            PlayerPrefs.SetInt("CurrentEnergy", CurrentEnergy);
            currentEnergyText.SetText(CurrentEnergy.ToString());
            return true;
        }
        return false;
    }

    public void AddEnergy(int amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, maxEnergy);
        PlayerPrefs.SetInt("CurrentEnergy", CurrentEnergy);
        currentEnergyText.SetText(CurrentEnergy.ToString());
    }

    private void OnApplicationQuit()
    {
        SaveEnergyState();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveEnergyState();
        }
    }

    private void SaveEnergyState()
    {
        PlayerPrefs.SetInt("CurrentEnergy", CurrentEnergy);
        PlayerPrefs.SetString("NextRefillTime", nextRefillTime.ToString());
    }

    private void SetFormattedTime(double time)
    {
        int hours = Mathf.FloorToInt((float)time / 3600);
        int minutes = Mathf.FloorToInt(((float)time % 3600) / 60);
        int seconds = Mathf.FloorToInt((float)time % 60);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        if (!refillTimeText.gameObject.activeSelf) refillTimeText.gameObject.SetActive(true);

        refillTimeText.SetText(formattedTime);
    }
}

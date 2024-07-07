using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class LoadingUI : MonoBehaviour
{
    [SerializeField] private GameObject loadingObject;

    private static LoadingUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    public static void OpenCloseLoading(bool status) => instance.loadingObject.SetActive(status);
}

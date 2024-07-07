using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private HexGridLayout gridLayout;

    public void SwitchConfiguration(GridConfiguration configuration)
    {
        StartCoroutine(SwitchConfigurationAsync(configuration));
    }
    private IEnumerator SwitchConfigurationAsync(GridConfiguration configuration)
    {
        LoadingUI.OpenCloseLoading(true);
        yield return gridLayout.StartCoroutine(gridLayout.ClearGrid());

        gridLayout.SwitchConfiguration(configuration);

        yield return gridLayout.StartCoroutine(gridLayout.SetupGrid());
        LoadingUI.OpenCloseLoading(false);
    }
}

using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CarouselNavigator : MonoBehaviour
{
    private static CarouselNavigator instance;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Transform[] levelCarouselBuildings;

    private int currentCarouselIndex = 0;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else enabled = false;

        previousButton.onClick.AddListener(MoveToPrevious);
        nextButton.onClick.AddListener(MoveToNext);
    }
    private void OnDestroy()
    {
        previousButton.onClick.RemoveListener(MoveToPrevious);
        nextButton.onClick.RemoveListener(MoveToNext);
    }

    private void LockToCurrentLevel() => UpdateCarouselTarget(GameManager.CurrentLevel - 1);
    private void MoveToPrevious() 
        => UpdateCarouselTarget((currentCarouselIndex - 1 + levelCarouselBuildings.Length) % levelCarouselBuildings.Length);
    private void MoveToNext() => UpdateCarouselTarget((currentCarouselIndex + 1) % levelCarouselBuildings.Length);

    private void UpdateCarouselTarget(int index)
    {
        currentCarouselIndex = Mathf.Clamp(index, 0 , levelCarouselBuildings.Length - 1);
        vCam.Follow = levelCarouselBuildings[currentCarouselIndex];
    }
    public static void LockCarouselAt(int level)
    {
        instance.previousButton.interactable = false;
        instance.nextButton.interactable = false;
        instance.vCam.Follow = instance.levelCarouselBuildings[level - 1];
    }
    public static void UnlockCarousel()
    {
        instance.previousButton.interactable = true;
        instance.nextButton.interactable = true;
        instance.LockToCurrentLevel();
    }
}

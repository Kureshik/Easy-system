using DG.Tweening;
using UnityEngine;

public class LeftMenu : MonoBehaviour
{
    [SerializeField] GameObject CloseMunuPanel;

    RectTransform menuPos;
    // Start is called before the first frame update
    void Start()
    {
        menuPos = transform as RectTransform;
        SwipeDetection.SwipeEvent += OnSwipe;
    }
    
    private void OnSwipe()
    {
        Debug.Log("Swipe!");
        CloseMunuPanel.SetActive(true);
        menuPos.DOAnchorPosX(262.5f, 0.4f).SetEase(Ease.OutQuart);
    }

    public void CloseMenu()
    {
        CloseMunuPanel.SetActive(false);
        menuPos.DOAnchorPosX(-262.5f, 0.4f).SetEase(Ease.OutQuart);
    }
}

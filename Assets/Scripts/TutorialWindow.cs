using UnityEngine;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField] GameObject TutorialPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("First time", 1) == 1) TutorialPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        PlayerPrefs.SetInt("First time", 0);
        TutorialPanel.SetActive(false);
    }
}

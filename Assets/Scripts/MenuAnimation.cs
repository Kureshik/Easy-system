using DG.Tweening;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    GameObject _menu;
    public void OpenMenu(GameObject Menu)
    {
        Menu.transform.localPosition = new Vector3(0, -Screen.height, 0);
        Menu.SetActive(true);
        Menu.GetComponent<RectTransform>().DOLocalMoveY(0, 0.4f).SetEase(Ease.OutQuart); ;
    }

    public void CloseMenu(GameObject Menu)
    {
        _menu = Menu;
        Menu.GetComponent<RectTransform>().DOLocalMoveY(-Screen.height, 0.4f).SetEase(Ease.OutQuart).OnComplete(callback);
    }
    void callback()
    {
        _menu.transform.localPosition = new Vector3(0, 0, 0);
        _menu.SetActive(false);
    }
}

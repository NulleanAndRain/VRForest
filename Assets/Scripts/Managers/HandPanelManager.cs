using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPanelManager : MonoBehaviour
{
    public GameObject openButton;
    public GameObject handPanel;
    public GameObject closeButton;
    public GameObject albumButton;
    public GameObject shopButton;
    public GameObject albumContainer;
    public GameObject shopContainer;

    private void Start() => CloseHandPanel();
    public void OpenHandPanel()
    {
        openButton.SetActive(false);
        handPanel.SetActive(true);
    }
    public void CloseHandPanel()
    {
        openButton.SetActive(true);
        handPanel.SetActive(false);
        OpenAlbum(); // та просто  чтоб сразу отображался один из двух контейнеров
    }
    public void OpenAlbum()
    {
        albumContainer.SetActive(true);
        shopContainer.SetActive(false);
    }
    public void OpenShop()
    {
        albumContainer.SetActive(false);
        shopContainer.SetActive(true);
    }
}

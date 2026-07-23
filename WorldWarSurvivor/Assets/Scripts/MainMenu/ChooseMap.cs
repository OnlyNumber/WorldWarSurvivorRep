using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMap : MonoBehaviour
{

    private const int GameplayTestSceneIndex = 1;

    private int CurrentMap;

    public Button CloseButton;

    public Button SovietMap;

    public Button GermanMap;

    public Button SteppeMap;

    public Button StartExpedition;


    public GameObject ChooseMapWindow;

    public Camera ChooseMapCamera;

    private void Start()
    {
        CloseButton.onClick.AddListener(CloseWindow);

        StartExpedition.onClick.AddListener(LoadGameplayTest);

        SovietMap.onClick.AddListener(() => CurrentMap = 0);
        GermanMap.onClick.AddListener(() => CurrentMap = 1);
        SteppeMap.onClick.AddListener(() => CurrentMap = 2);

    }

    public void OpenExpedition()
    {
        ChooseMapWindow.SetActive(true);
    }
    
    private void ShowMissionRewards()
    {

    }

    private void LoadGameplayTest()
    {
        SceneManager.LoadScene(GameplayTestSceneIndex);
    }

    private void CloseWindow()
    {
        ChooseMapWindow.SetActive(false);

    }
}

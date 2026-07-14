using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const int GameplayTestSceneIndex = 1;

    public Button PlayButton;

    public Button ExitButton;

    public GameObject MainMenuCamera;

    [Space(5)]

    [Header("Recruit")]

    public GameObject RecruitCamera;

    public RecruitControl RecruitControl;


    private void Start()
    {

        PlayButton.onClick.AddListener(LoadGameplayTest);

        ExitButton.onClick.AddListener(ExitGame);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ActivateMenuAction();
    }

    private void ActivateMenuAction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity) || UICheck.IsPointerOverUIElement())
            return;

        var button = hit.collider.GetComponentInParent<MenuButton>();

        switch (button.Identifier)
        {
            case MenuButtons.StartExpedition:
                OpenStartExpedition();
                break;
            case MenuButtons.Crafting:
                OpenCrafting();

                break;
            case MenuButtons.Recruit:
                OpenRecruit();

                break;
            case MenuButtons.Exit:
                OpenExit();

                break;
        }
    }

    private void OpenStartExpedition()
    {
        Debug.Log("OpenStartExpedition");
    }

    private void OpenCrafting()
    {
        Debug.Log("OpenCrafting");

    }

    private void OpenRecruit()
    {
        MainMenuCamera.SetActive(false);
        RecruitCamera.SetActive(true);
        
        RecruitControl.OpenRecruits();

    }

    private void OpenExit()
    {
        Debug.Log("OpenExit");

    }

    private void LoadGameplayTest()
    {
        SceneManager.LoadScene(GameplayTestSceneIndex);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}

public enum MenuButtons
{
    StartExpedition,
    Crafting,
    Recruit,
    Exit
}
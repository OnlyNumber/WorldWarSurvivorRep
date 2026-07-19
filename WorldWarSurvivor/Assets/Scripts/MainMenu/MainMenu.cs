using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayButton;

    public Button SaveButton;


    public Button ExitButton;

    public Camera MainMenuCamera;

    [Space(5)]

    [Header("Recruit")]

    public RecruitControl RecruitControl;

    public ExpeditionControl ExpeditionControl;


    private void Start()
    {
        SaveButton.onClick.AddListener(BaseProgression.Instance.SaveInfo);

        CameraControl.MainCamera = MainMenuCamera;
        CameraControl.ChangeToCamera(MainMenuCamera);
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
        CameraControl.ChangeToCamera(ExpeditionControl.ExpeditionCamera);

        ExpeditionControl.OpenExpedition();
    }

    private void OpenCrafting()
    {
        Debug.Log("OpenCrafting");

    }

    private void OpenRecruit()
    {
        CameraControl.ChangeToCamera(RecruitControl.RecruitCamera);

        RecruitControl.OpenRecruits();

    }

    private void OpenExit()
    {
        Debug.Log("OpenExit");

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
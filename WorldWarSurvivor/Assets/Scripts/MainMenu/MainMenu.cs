using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const int GameplayTestSceneIndex = 1;

    public Button PlayButton;

    public Button ExitButton;

    private void Start() 
    {
        PlayButton.onClick.AddListener(LoadGameplayTest);

        ExitButton.onClick.AddListener(ExitGame);
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionWindow : MonoBehaviour
{
    [SerializeField] private RectTransform actionButtonsPlace;
    [SerializeField] private RectTransform InfoTextPlace;

    [SerializeField] private ActionButton ActionButtonPrefab;
    [SerializeField] private CharacteristicText CharacteristicTextPrefab;

    private List<ActionButton> currentButtons = new();
    private List<CharacteristicText> currentCharacteristics = new();

    public InventoryWindow inventoryWindow;

    public Button EndTurnButton;
    public Button InventoryButton;


    public static ActionWindow Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        EndTurnButton.onClick.AddListener(EndTurn);
        InventoryButton.onClick.AddListener(OpenInventory);

    }

    public void CreateButtons(List<string> text, List<bool> isAvailableButton)
    {
        for (int i = 0; i < text.Count; i++)
        {
            var button = Instantiate(ActionButtonPrefab, actionButtonsPlace);

            int buttonIndex = i;
            button.ActionText.text = text[i];
            button.Button.onClick.AddListener(() => CellSelecter.Instance.CurrentActionIndex = buttonIndex);
            button.Button.interactable = isAvailableButton[i];

            currentButtons.Add(button);
        }
    }

    public void CreateCharacteristics(List<string> text)
    {
        for (int i = 0; i < text.Count; i++)
        {
            var characteristicsText = Instantiate(CharacteristicTextPrefab, InfoTextPlace);

            int index = i;
            characteristicsText.InfoText.text = text[i];
            currentCharacteristics.Add(characteristicsText);
        }
    }

    public void UpdateCharacteristics()
    {

    }

    public void ClearButtons()
    {
        if (currentButtons.Count == 0)
            return;

        foreach (var item in currentButtons)
            Destroy(item.gameObject);

        currentButtons.Clear();
    }

    public void ClearCharacteristics()
    {
        foreach (var item in currentCharacteristics)
            Destroy(item.gameObject);

        currentCharacteristics.Clear();
    }

    public void ClearActionWindow()
    {
        ClearButtons();
        ClearCharacteristics();
    }

    public void EndTurn()
    {
        TurnController.SetNextTurn();
    }

    public void OpenInventory()
    {
        inventoryWindow.OpenWindow((CellSelecter.Instance.CurrentObject as Human).HumanInventory);
    }
}

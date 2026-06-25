using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionWindow : MonoBehaviour
{
    [SerializeField] private RectTransform actionButtonsPlace;
    [SerializeField] private RectTransform InfoTextPlace;

    public ActionButton ActionButtonPrefab;
    public CharacteristicText CharacteristicTextPrefab;
    public List<ActionButton> currentButtons = new();
    public List<CharacteristicText> currentCharacteristics = new();


    public static ActionWindow Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CreateButtons(List<string> text)
    {
        for (int i = 0; i < text.Count; i++)
        {
            var button = Instantiate(ActionButtonPrefab, actionButtonsPlace);

            int buttonIndex = i;
            button.ActionText.text = text[i];
            button.Button.onClick.AddListener(() => CellSelecter.Instance.CurrentActionIndex = buttonIndex);

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

    public void ClearActionWindow()
    {
        if (currentButtons.Count == 0)
            return;

        foreach (var item in currentButtons)
            Destroy(item.gameObject);

        currentButtons.Clear();

        foreach (var item in currentCharacteristics)
            Destroy(item.gameObject);

        currentCharacteristics.Clear();
    }


}

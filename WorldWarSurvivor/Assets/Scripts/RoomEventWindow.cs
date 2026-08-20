using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class RoomEventWindow : MonoBehaviour
{
    public TMP_Text Description;

    [SerializeField] private GameObject window;
    [SerializeField] private RectTransform placeForChoices;
    [SerializeField] private Button choiceButtonPrefab;

    private List<Button> _createdChoices = new();

    public void CreateChoices(params Choice[] choices)
    {
        foreach (var item in choices)
        {
            var choice = Instantiate(choiceButtonPrefab, placeForChoices);
            choice.GetComponentInChildren<TMP_Text>().text = item.Text;
            choice.onClick.AddListener(item.ChoiceAction.Invoke);
            _createdChoices.Add(choice);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(placeForChoices);
    }

    public void ClearChoices()
    {
        foreach (var item in _createdChoices)
        {
            item.onClick.RemoveAllListeners();
            Destroy(item.gameObject);
        }
        _createdChoices.Clear();
    }


    public void Show()
    {
        window.SetActive(true);
    }

    public void Hide()
    {
        window.SetActive(false);
        ClearChoices();

    }

}

public class Choice
{
    public string Text;
    public Action ChoiceAction;
}

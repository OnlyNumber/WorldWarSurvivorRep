using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanUIButton : MonoBehaviour
{
    public Button HumanImageButton;

    public Button AllInfo;

    public TMP_Text CurrentLevel;

    public TMP_Text HumanHealth;

    public TMP_Text MeleeSkill;
    public TMP_Text RangeSkill;

    [field: SerializeField]
    public GrabbingItem GrabbingItem
    {
        get;
        private set;
    }


}

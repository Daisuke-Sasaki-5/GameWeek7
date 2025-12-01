using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{ 
    public static UIManager Instance;

    [Header("Start UI")]
    public TextMeshProUGUI startText;

    private void Awake()
    {
        Instance = this;
    }

    // ==== Start UI ====
    public void ShowStartUI(bool show)
    {
        startText?.gameObject.SetActive(show);
    }
}

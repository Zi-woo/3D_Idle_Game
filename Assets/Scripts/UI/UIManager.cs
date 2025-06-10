using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Top UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text missionText;

    [Header("Skill UI")]
    [SerializeField] private TMP_Text[] SetSkillNameTexts;
    [SerializeField] private TMP_Text[] SetSkillCoolDownTexts;

    [Header("Inventory UI")] 
    [SerializeField] private Button InventoryButton;

    [SerializeField] private UIInventory inventoryUI;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        SetLevel(Player.Instance.PlayerStatusData.level);
        SetGold(Player.Instance.PlayerStatusData.gold);
        SetStage(Player.Instance.PlayerStatusData.currentStage);
        InventoryButton.onClick.AddListener(OnInventoryButtonClick);
    }
    private void OnInventoryButtonClick()
    {
        inventoryUI.Toggle();
    }
    public void SetLevel(int level)
    {
        levelText.text = $"Level {level}";
    }

    public void SetGold(int gold)
    {
        goldText.text = $"Gold {gold.ToString()}";
    }

    public void SetStage(int stage)
    {
        stageText.text = $"Stage {stage}";
    }

    public void SetMission(string mission)
    {
        missionText.text = mission;
    }

    public void SetSkillName(int index, float time)
    {
        if(index < 0 || index >= SetSkillNameTexts.Length)
        {
             return;
        }
        SetSkillNameTexts[index].text = time > 0 ? time.ToString("0.0") : "";
    }
}
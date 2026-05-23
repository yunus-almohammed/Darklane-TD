using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("UI")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinText;

    [Header("Base")]
    public int baseHP = 20;

    [Header("Coins")]
    public int coins = 0;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
    }

    void Start()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        if (hpText) hpText.text = baseHP.ToString();
        if (coinText) coinText.text = coins.ToString();
    }

    public void DamageBase(int dmg)
    {
        baseHP -= dmg;
        RefreshUI();

        if (baseHP <= 0)
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        RefreshUI();
    }
}

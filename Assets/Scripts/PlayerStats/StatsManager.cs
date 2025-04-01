using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class StatsManager : MonoBehaviour, IStats
{
    [SerializeField] private TextMeshProUGUI coinsUpgradeMenuTxt;
    [SerializeField] private TextMeshProUGUI coinsUITxt;
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private TextMeshProUGUI sawsTxt;
    [SerializeField] private TextMeshProUGUI sawSpeedTxt;
    [SerializeField] private TextMeshProUGUI damageIncreasedTxt;
    [SerializeField] private TextMeshProUGUI sawAddedTxt;
    [SerializeField] private TextMeshProUGUI sawSpeedIncreasedTxt;
    [SerializeField] private TextMeshProUGUI playerHealedTxt;
    [SerializeField] private TextMeshProUGUI notEnoughMoneyTxt;
    [SerializeField] private TextMeshProUGUI maxSawsReachedTxt;
    [SerializeField] private GameObject playerGO;
    [SerializeField] private List<GameObject> saws;

    private int damageIncreaseCost = 100;
    private int addSawCost = 300;
    private int increaseSawSpeedCost = 50;
    private int healCost = 100;

    public int Damage { get { return _damage; } }
    private int _damage = 100;
    private int initialDamage;

    public float SawsVelocity { get { return _sawsVelocity; } }
    private float _sawsVelocity = 90;
    private float initialSawVelocity;
    private int sawsCount = 1;
    private IDamageable _player;
    private Coroutine messageCo;
    private GameObject actualMessage;

    public int Coins { get { return _coins; } }
    private int _coins = 100000;

    private void Awake()
    {
        _player = playerGO.GetComponent<IDamageable>();
        initialSawVelocity = _sawsVelocity;
        initialDamage = _damage;
        damageTxt.text = _damage.ToString();
        sawSpeedTxt.text = _sawsVelocity.ToString();
    }

    public void SetPlayer(IDamageable player)
    {
        _player = player;
    }

    public void AddCoins()
    {
        _coins += 10;
        coinsUpgradeMenuTxt.text = _coins.ToString();
        coinsUITxt.text = _coins.ToString();
    }

    private void Buy(int cost)
    {
        _coins -= cost;
        coinsUpgradeMenuTxt.text = _coins.ToString();
        coinsUITxt.text = _coins.ToString();
    }

    public void AddSaws()
    {
        if (_coins < addSawCost)
        {
            ShowMessage(notEnoughMoneyTxt);
        }
        else
        {
            if (sawsCount < saws.Count)
            {
                ShowMessage(sawAddedTxt);
                Buy(addSawCost);
                saws[sawsCount - 1].SetActive(false);
                sawsCount++;
                saws[sawsCount - 1].SetActive(true);
                sawsTxt.text = sawsCount.ToString();
            }
            else
            {
                ShowMessage(maxSawsReachedTxt);
            }
        }
    }

    public void IncreaseDamage()
    {
        if (_coins < damageIncreaseCost)
        {
            ShowMessage(notEnoughMoneyTxt);
        }
        else
        {
            ShowMessage(damageIncreasedTxt);
            Buy(damageIncreaseCost);
            _damage += initialDamage;
            damageTxt.text = _damage.ToString();
        }
    }

    public void IncreaseSawsVelocity()
    {
        if (_coins < increaseSawSpeedCost)
        {
            ShowMessage(notEnoughMoneyTxt);
        }
        else
        {
            ShowMessage(sawSpeedIncreasedTxt);
            Buy(increaseSawSpeedCost);
            _sawsVelocity += initialSawVelocity / 2;
            sawSpeedTxt.text = _sawsVelocity.ToString();
        }
    }

    public void Heal()
    {
        if (_coins < healCost)
        {
            ShowMessage(notEnoughMoneyTxt);
        }
        else
        {
            ShowMessage(playerHealedTxt);
            _player.Heal();
            Buy(healCost);
        }
    }

    private void ShowMessage(TextMeshProUGUI text)
    {
        if (messageCo != null)
        {
            StopCoroutine(messageCo);
            actualMessage.SetActive(false);
        }
        messageCo = StartCoroutine(ShowMessageCo(text));
    }

    private IEnumerator ShowMessageCo(TextMeshProUGUI text)
    {
        actualMessage = text.gameObject;
        actualMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        actualMessage.SetActive(false);
    }
}

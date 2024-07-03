using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image _sliderImage;
    [SerializeField] private TMP_Text _hpText;
    private int _hp;
    private int _maxHp;

    public void SetHP(int hp)
    {
        _hp = Mathf.Clamp(hp, 0, _maxHp);
        UpdateUI();
    }

    public void SetMaxHP(int maxHp)
    {
        _maxHp = maxHp;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _sliderImage.fillAmount = (float)_hp / _maxHp;
        _hpText.text = $"{_hp}";
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectableButton : MonoBehaviour
{
    [SerializeField] private Image _outlineImage;
    private Button _button;
    public event Action OnClick;
    public bool Selected
    {
        get
        {
            return _outlineImage.color.a > 0.0f;
        }
        set
        {
            SetAlpha(value ? 1.0f : 0.0f);
        }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(() =>
        {
            OnClick?.Invoke();
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        OnClick = null;
    }

    private void SetAlpha(float alpha)
    {
        _outlineImage.color = new Color(_outlineImage.color.r, _outlineImage.color.g, _outlineImage.color.b, alpha);
    }
}
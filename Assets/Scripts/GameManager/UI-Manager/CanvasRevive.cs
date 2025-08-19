using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRevive : UICanvas
{
    [Header("Button")]
    [SerializeField] private Button _skipButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _countdownText;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    Coroutine _crtCountdown;
    private void Awake()
    {
        _skipButton.onClick.AddListener(SkipRevive);
    }
    private void OnEnable()
    {
        if (_crtCountdown != null)
        {
            StopCoroutine(_crtCountdown);
        }
        if (_crtCountdown == null)
        {
            _crtCountdown = StartCoroutine(CountDown(5));
        }
    }
    public void SkipRevive()
    {
        _audioSource.Stop();
        if (_crtCountdown != null)
        {
            StopCoroutine(_crtCountdown);
        }
        Close(0);
        UIManager.Instance.OpenUI<CanvasDefead>();
    }
    public IEnumerator CountDown(float seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            _audioSource.Play();
            _countdownText.text = (seconds - i).ToString();
            yield return new WaitForSeconds(1);
        }
        Close(0);
        UIManager.Instance.OpenUI<CanvasDefead>();
    }
}

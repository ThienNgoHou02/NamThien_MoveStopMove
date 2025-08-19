using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasDefead : UICanvas
{
    [Header("Button")]
    [SerializeField] private Button _continueButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _killerText;
    [SerializeField] private TextMeshProUGUI _goldRewardText;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    Coroutine _crtEnableButton;
    LevelManager levelMng;
    private void Awake()
    {
        _continueButton.onClick.AddListener(Continue);
        levelMng = LevelManager.Instance;
    }
    private void OnEnable()
    {
        _audioSource.Play();
        _continueButton.gameObject.SetActive(false);
        _rankText.text = $"#{levelMng?.Rank.ToString()}";
        _killerText.text = levelMng?.Killer;
        _goldRewardText.text = levelMng?.Gold.ToString();
        if (_crtEnableButton != null)
        {
            StopCoroutine(_crtEnableButton);
        }
        if (_crtEnableButton == null)
        {
            _crtEnableButton = StartCoroutine(EnableContinueButton(2));
        }
    }
    public void Continue()
    {
        Close(0);
        SceneManager.LoadScene("StartScene");
    }
    public IEnumerator EnableContinueButton(float time)
    {
        yield return new WaitForSeconds(time);
        _continueButton.gameObject.SetActive(true);
    }
}

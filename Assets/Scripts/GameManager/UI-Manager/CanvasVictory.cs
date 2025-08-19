using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasVictory : UICanvas
{
    [Header("Button")]
    [SerializeField] private Button _nextZoneButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _goldRewardText;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    Coroutine _crtEnableNextZoneButton;
    LevelManager levelMng;

    private void Awake()
    {
        _nextZoneButton.onClick.AddListener(NextZone);
        levelMng = LevelManager.Instance;
    }
    private void OnEnable()
    {
        _audioSource.Play();
        _nextZoneButton.gameObject.SetActive(false);
        _goldRewardText.text = levelMng?.Gold.ToString();
        if (_crtEnableNextZoneButton != null)
        {
            StopCoroutine(_crtEnableNextZoneButton);
        }
        if (_crtEnableNextZoneButton == null)
        {
            _crtEnableNextZoneButton = StartCoroutine(EnableNextZoneButton(5));
        }
    }
    public void NextZone()
    {
        Close(0);
        SceneManager.LoadScene("StartScene");
    }
    public IEnumerator EnableNextZoneButton(float time)
    {
        yield return new WaitForSeconds(time);
        _nextZoneButton.gameObject.SetActive(true);
    }
}

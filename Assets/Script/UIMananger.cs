﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMananger : MonoBehaviour
{
    private bool _isThrusting = false;
    private Player _player;

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameoverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _quitText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Sprite[] _livesSprite;
    private GameManager _gameManager;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _slider.value = 0;
        _scoreText.text = "Score: " + 0;
        _gameoverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _quitText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    private void Update()
    {
        if(!_isThrusting)
        {
            _slider.value += Time.deltaTime;
        }
        else
        {
            _slider.value -= Time.deltaTime * 3;
            if(_slider.value <= 0)
            {
                _isThrusting = false;
                _player.ThrustEnable(false);
            }
        }
    }

    public void ThrustTrigger()
    {
        if (_slider.value >= 6)
        {
            _isThrusting = true;
            _player.ThrustEnable(true);
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateSprite(int currentLives)
    {
        _livesImg.sprite = _livesSprite[currentLives];

        if(currentLives == 0)
        {
            _gameManager.GameOver();
            _gameoverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            _quitText.gameObject.SetActive(true);
            StartCoroutine(GameoverTextFlicker());
        }
    }

    IEnumerator GameoverTextFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameoverText.gameObject.GetComponent<Text>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            _gameoverText.gameObject.GetComponent<Text>().enabled = true;
        }
    }

    public void UpdateAmmo(int ammoCount, int maxAmmo)
    {
        _ammoText.text = ": " + ammoCount + "/" + maxAmmo;
    }    
}

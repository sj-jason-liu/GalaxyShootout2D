﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMananger : MonoBehaviour
{
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
    private Sprite[] _livesSprite;
    private GameManager _gameManager;

    void Start()
    {
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

    public void UpdateAmmo(int ammoCount)
    {
        if(ammoCount > 0)
        {
            _ammoText.text = ": " + ammoCount;
        }
        else
        {
            _ammoText.text = ": " + 0;
        }
    }    
}

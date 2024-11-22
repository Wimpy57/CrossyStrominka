using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour {

    private void Start() {
        gameObject.SetActive(false);
        Player.Instance.OnCollideVehicle += Player_OnCollideVehicle;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, EventArgs e) {
        gameObject.SetActive(true);
    }

    private void Player_OnCollideVehicle(object sender, EventArgs e) {
        gameObject.SetActive(true);
    }
}

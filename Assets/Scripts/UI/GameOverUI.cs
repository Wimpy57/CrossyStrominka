using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour {

    private void Start() {
        gameObject.SetActive(false);
        Player.Instance.OnCollideVehicle += Player_OnCollideVehicle;
    }

    private void Player_OnCollideVehicle(object sender, EventArgs e) {
        gameObject.SetActive(true);
    }
}

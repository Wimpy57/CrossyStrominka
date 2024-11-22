using TMPro;
using UnityEngine;

public class ScoreCounterUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;
    
    public static ScoreCounterUI Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
    
    private void Start() {
        scoreText.text = "0";
        
    }

    public void RefreshScore() {
        scoreText.text = Player.Instance.transform.position.z.ToString();
    }
}

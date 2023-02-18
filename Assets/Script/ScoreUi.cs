using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUi : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
            
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManger.Instance.currentScore.ToString();

    }
}

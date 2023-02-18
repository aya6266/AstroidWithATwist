using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class highScoreUi : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        highScoreText.text = GameManger.Instance.highScore.ToString();
    }
}

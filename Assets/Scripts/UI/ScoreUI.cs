using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    [SerializeField] IntegerData score;
    private void Update()
    {
        scoreText.text = score.value.ToString();
    }
}

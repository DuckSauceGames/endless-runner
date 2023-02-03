using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressionBehaviour : MonoBehaviour {

    GameObject player;
    LevelBuilder levelBuilder;
    public TMP_Text scoreText;
    BonusScoring bonuses;

    float startPoint;
    float currentPoint = 0f;
    int score = 0;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        bonuses = player.GetComponent<BonusScoring>();
        startPoint = player.transform.position.x;
        levelBuilder = GameObject.Find("Platforms").GetComponent<LevelBuilder>();
    }

    void FixedUpdate() {
        currentPoint = player.transform.position.x;
        levelBuilder.SpawnPlaformsWhenClose(currentPoint);

        UpdateScore();
    }

    void UpdateScore() {
        int baseScore = Mathf.Max(0, Mathf.RoundToInt(currentPoint - startPoint));
        int bonusScore = bonuses.GetTotalBonusScore();
        score = baseScore + bonusScore;

        scoreText.text = "Score: " + score;
    }
}

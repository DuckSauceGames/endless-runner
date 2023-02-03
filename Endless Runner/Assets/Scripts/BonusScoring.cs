using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BonusScoring : MonoBehaviour {

    public GameObject bonusMessagesParent;
    public GameObject bonusMessagePrefab;

    Rigidbody2D target;
    SledBehaviour sled;
    bool inAir = true;
    int bonusScore = 0;

    int flipScore = 100;
    float airRotation;
    float prevFrameRotation;

    // >20 speed for 3 seconds (20 points per half second)
    int speedScore = 50;
    float speedThreshold = 20;
    float timeUntilFirstBonus = 3;
    float timeUntilBonus = 5f;
    float bonusInterval = 5f;


    void Awake() {
        target = GetComponent<Rigidbody2D>();
        sled = GetComponent<SledBehaviour>();
    }

    void FixedUpdate() {
        if (!sled.IsAlive()) return;

        CheckFlip();
        CheckSpeeding();
    }

    public void OnLand() {
        // Score flip
        if (Mathf.Abs(airRotation) > 180) {
            Debug.Log(-airRotation + " flip!");
            int numberHalfFlips = Mathf.RoundToInt(Mathf.Abs(airRotation) / 180);
            AddBonusPoints("Flip", numberHalfFlips * flipScore / 2);
        }
    }

    public void OnTakeoff() {
        // Reset flip
        airRotation = 0;
        prevFrameRotation = target.rotation;
    }


    void CheckFlip() {
        if (inAir) {
            airRotation += target.rotation - prevFrameRotation;
            prevFrameRotation = target.rotation;
        }    
    }

    void CheckSpeeding() {
        if (target.velocity.magnitude >= speedThreshold) {
            timeUntilBonus -= Time.fixedDeltaTime;
            if (timeUntilBonus <= 0) {
                Debug.Log(speedScore + " speed bonus!");
                AddBonusPoints("Speeding", speedScore);
                timeUntilBonus = bonusInterval;
            }
        } else if (!inAir) {
            timeUntilBonus = timeUntilFirstBonus;
        }
    }

    public int GetTotalBonusScore() {
        return bonusScore;
    }

    void AddBonusPoints(string message, int points) {
        bonusScore += points;

        GameObject instance = Instantiate(bonusMessagePrefab, bonusMessagesParent.transform);
        BonusMessage bonusMessage = instance.GetComponent<BonusMessage>();
        bonusMessage.SetMessage(message, points);
    }
}

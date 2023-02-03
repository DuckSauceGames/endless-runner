using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusMessage : MonoBehaviour {

    TMP_Text text;
    float lifetime = 2;
    float fadeTime = 1f;
    float yIncrease = 150;
    float yStart;
    float timeAlive = 0;

    void Awake() {
        text = GetComponent<TMP_Text>();
        yStart = transform.position.y;
    }

    private void Start() {
        Destroy(gameObject, lifetime);
    }

    void Update() {
        timeAlive += Time.deltaTime;
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(yStart, yStart + yIncrease, timeAlive / lifetime);
        if (timeAlive > lifetime - fadeTime) {
            byte opacity = (byte) Mathf.RoundToInt(Mathf.Lerp(255, 0, (timeAlive - (lifetime - fadeTime)) / fadeTime));
            text.faceColor = new Color32(255, 255, 255, opacity);
        }
        transform.position = pos;
    }

    public void SetMessage(string bonusMessage, int points) {
        text.text = bonusMessage + " +" + points;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusMessageManager : MonoBehaviour {

    Queue<Bonus> queue = new Queue<Bonus>();

    public void AddBonus(string message, int points) {
        queue.Enqueue(new Bonus(message, points));
    }

    // Update is called once per frame
    void Update() {
        
    }
}

class Bonus {
    public int points;
    public string message;

    public Bonus(string message, int points) {
        this.points = points;
        this.message = message;
    }
}

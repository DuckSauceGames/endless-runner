using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour {

    PolygonCollider2D coll;
    Vector2 overlap = new Vector2(-0.05f, 0);

    private PolygonCollider2D GetCollider() {
        if (!coll) {
            coll = GetComponent<PolygonCollider2D>();
        }
        return coll;
    }

    public Vector2 FirstPlatformPoint() {
        return (Vector2) transform.position + TopRightPoint();
    }

    public Vector2 PositionPlatform(Vector2 previousPlatformTopRight) {
        Vector2 target = previousPlatformTopRight + overlap;
        PositionSelf(target);
        return target - TopLeftPoint() + TopRightPoint();
    }

    void PositionSelf(Vector2 joinPoint) {
        //StartCoroutine(MoveSlowly(joinPoint - TopLeftPoint()));
        transform.position = joinPoint - TopLeftPoint();
    }

    Vector2 TopLeftPoint() {
        Vector2 current = GetCollider().points[0];
        foreach (Vector2 point in GetCollider().points) {
            if (point.x < current.x || point.x == current.x && point.y > current.y) {
                current = point;
            }
        }
        return current;
    }

    Vector2 TopRightPoint() {
        Vector2 current = GetCollider().points[0];
        foreach (Vector2 point in GetCollider().points) {
            if (point.x > current.x || point.x == current.x && point.y > current.y) {
                current = point;
            }
        }
        return current;
    }

    IEnumerator MoveSlowly(Vector2 targetPosition) {
        float timeSinceStarted = 0f;
        while (true) {
            timeSinceStarted += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, targetPosition, timeSinceStarted / 100);

            // If the object has arrived, stop the coroutine
            if ((Vector2) transform.position == targetPosition) {
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LevelBuilder : MonoBehaviour {
    public GameObject platformPrefab;
    public PhysicsMaterial2D groundMaterial;
    int pixelsPerUnit = 128;

    Vector2 nextJoinPoint = Vector2.zero;
    float playerSpawnRadius = 50f;
    Dictionary<string, Sprite> platformSprites = new Dictionary<string, Sprite>();
    List<string> spriteNames = new List<string>();

    ObjectPool<GameObject> platformPool = new ObjectPool<GameObject>();
    int poolSize = 5;

    void Start() {
        LoadAllSprites();
        FillObjectPool();

        CreatePlatform("Flat");
        CreatePlatform("Flat");
        CreatePlatform("Flat");
    }

    void LoadAllSprites() {
        string dir = "Assets/Sprites/platforms/";
        foreach (string file in Directory.GetFiles(dir)) {
            if (file.EndsWith(".png")) {
                string fileName = file.Split("/")[3];
                string spriteName = fileName.Substring(0, fileName.Length - 4);
                platformSprites.Add(spriteName, LoadPNG(file));
                spriteNames.Add(spriteName);
            }
        }
        Debug.Log("Loaded " + platformSprites.Count + " sprites");
    }

    public void SpawnPlaformsWhenClose(float playerX) {
        if (Mathf.Abs(nextJoinPoint.x - playerX) < playerSpawnRadius) {
            CreateRandomPlatform();
        }
    }

    private Sprite LoadPNG(string fileName) {
        string filePath = fileName;
        Sprite sprite = null;

        if (File.Exists(filePath)) {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(pixelsPerUnit, pixelsPerUnit);
            tex.LoadImage(fileData);
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, pixelsPerUnit);
        } else {
            Debug.LogError(string.Format("Path {0} does not exist", filePath));
        }
        return sprite;
    }

    private void FillObjectPool() {
        foreach (string spriteName in spriteNames) {
            for (int i = 0; i < poolSize; i++) {
                Sprite sprite = platformSprites[spriteName];

                GameObject instance = Instantiate(platformPrefab, new Vector2(-999, -999), Quaternion.identity, transform);
                instance.name = spriteName;
                SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
                sr.sprite = sprite;
                PolygonCollider2D coll = instance.AddComponent<PolygonCollider2D>();
                coll.sharedMaterial = groundMaterial;

                instance.SetActive(false);

                platformPool.Add(spriteName, instance);
            }
        }
    }

    private void CreateRandomPlatform() {
        CreatePlatform(spriteNames[Random.Range(0, spriteNames.Count)]);
    }

    private void CreatePlatform(string spriteName) {
        GameObject instance = platformPool.NextObject(spriteName);
        instance.transform.position = nextJoinPoint + new Vector2(1 * Random.Range(5f, 15f), Random.Range(-10f, 10f));
        instance.SetActive(true);
        nextJoinPoint = instance.GetComponent<PlatformBehaviour>().PositionPlatform(nextJoinPoint);
    }
}

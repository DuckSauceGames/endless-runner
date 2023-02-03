using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> {

    Dictionary<string, Queue<T>> pool = new Dictionary<string, Queue<T>>();

    public void Add(string objectId, T instance) {
        Queue<T> queue = GetOrCreateQueue(objectId);
        queue.Enqueue(instance);
    }

    public void AddAll(string objectId, List<T> instances) {
        Queue<T> queue = GetOrCreateQueue(objectId);
        foreach (T instance in instances) {
            queue.Enqueue(instance);
        }
    }

    public T NextObject(string objectId) {
        Queue<T> queue = pool.GetValueOrDefault(objectId);
        if (queue == null) return default(T);

        T result = queue.Dequeue();
        queue.Enqueue(result);
        return result;
    }

    public T RandomObject() {
        List<string> keys = new List<string>(pool.Keys);
        return NextObject(keys[Random.Range(0, keys.Count)]);
    }

    public int Size(string objectId) {
        Queue<T> queue = pool.GetValueOrDefault(objectId);
        if (queue == null) return 0;
        return queue.Count;
    }

    private Queue<T> GetOrCreateQueue(string objectId) {
        if (!pool.ContainsKey(objectId)) {
            pool.Add(objectId, new Queue<T>());
        }
        return pool.GetValueOrDefault(objectId);
    }
}

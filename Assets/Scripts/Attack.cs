using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AttackEntry
{
    public float timeToSpawn = 0;

    public AttackEntry()
    {
    }

    public virtual void Process()
    {
        // To be overridden by subclasses
        Debug.Log("Processing Attack Entry at time: " + timeToSpawn);
    }
}

public class BulletEntry : AttackEntry
{
    public Bullet bullet;

    public BulletEntry(Bullet bullet) : base()
    {
        this.bullet = bullet;
    }

    public override void Process()
    {
        BulletSpawner.Instance.SpawnBullet(bullet);
    }
}

public class HeartEntry : AttackEntry
{
    public Vector3 position;

    public HeartEntry(Vector3 position) : base()
    {
        this.position = position;
    }

    public override void Process()
    {
        GameManager.Instance.SpawnHeart(position);
        Debug.Log("Spawning Heart at position: " + position);
    }
}

public class BoxUpdateEntry : AttackEntry
{
    public Vector3 position;
    public float rotation;
    public Vector2 scale;
    public int duration;

    public BoxUpdateEntry(Vector3 position, float rotation, Vector2 scale, int duration) : base()
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.duration = duration;
    }

    public override void Process()
    {
        // Spawn box update at position
        Debug.Log("Box Update... ");
    }
}

public class EndEntry : AttackEntry
{
    public EndEntry() : base()
    {
    }

    public override void Process()
    {
        GameManager.Instance.currentEvent.EndEvent();
    }
}

public class Attack {

    // attack class -> essentially a spawner that spawns things on in interval

    // -> list of "bullets" to spawn (as prefabs probably?) and a time of when to spawn

    public List<AttackEntry> entries = new List<AttackEntry>();
    int nextEntryIndex = 0;

    float timer = 0.0f;

    public void Init() {
        timer = 0.0f;
        nextEntryIndex = 0;

        // if last entry isn't end, append an end entry
        if (entries.Count == 0 || !(entries[entries.Count - 1] is EndEntry)) {
            entries.Add(new EndEntry());
            entries[entries.Count - 1].timeToSpawn = entries[entries.Count - 2].timeToSpawn + 2.0f;
        }

        Debug.Log("Attack Initialized with " + entries.Count + " entries. First entry time: " + entries[0].timeToSpawn);
    }

    public void Update() {

        timer += Time.deltaTime;
        
        for (int i = nextEntryIndex; i < entries.Count; i++) {
            AttackEntry entry = entries[i];
            if (timer >= entry.timeToSpawn) {
                entry.Process();
                nextEntryIndex = i + 1;
            }
        }
    }

    public void addEntry(float time, AttackEntry entry) {
        entry.timeToSpawn = time;
        entries.Add(entry);
    }
}
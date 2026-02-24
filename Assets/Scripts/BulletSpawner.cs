using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet {
    public float speed;
    public string sprite_name;
    public Color color;
    public float hitboxRadius;
    public string tagName;

    public Bullet(
        string spr,
        Color col,
        float hitboxRad,
        string tag
    )
    {
        sprite_name = spr;
        color = col;
        hitboxRadius = hitboxRad;
        tagName = tag;
    }
}

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public static BulletSpawner Instance;

    public int poolSize = 50;

    // for debugging
    public int QueueBulletCount = 0;
    public int ActiveBulletCount = 0;

    Queue<GameObject> pool = new Queue<GameObject>();
    List<GameObject> activeBullets = new List<GameObject>();

    // instancing
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < poolSize; i++)
        {
            var b = Instantiate(bulletPrefab, transform);
            b.SetActive(false);
            pool.Enqueue(b);
            QueueBulletCount++;
        }
    }

    public GameObject PoolGet()
    {
        GameObject b;

        if (pool.Count > 0)
        {
            b = pool.Dequeue();
            QueueBulletCount--;
        }
        else
        {
            b = Instantiate(bulletPrefab, transform);
        }

        b.SetActive(true);
        activeBullets.Add(b);
        ActiveBulletCount++;

        return b;
    }    

    public void PoolReturn(GameObject b)
    {
        if (b == null) return;

        if (activeBullets.Contains(b))
        {
            activeBullets.Remove(b);
            b.SetActive(false);
            pool.Enqueue(b);

            ActiveBulletCount--;
            QueueBulletCount++;
        }
        else
        {
            Debug.LogWarning("Attempted to return a bullet that's not active!");
        }
    }

    static Dictionary<string, Sprite> spriteCache = new();

    Sprite GetSprite(string spriteName)
    {
        if (!spriteCache.TryGetValue(spriteName, out Sprite sprite))
        {
            sprite = Resources.Load<Sprite>(spriteName);
            spriteCache[spriteName] = sprite;
        }
        return sprite;
    }

    public BulletMovement SpawnBullet(
        Vector3 position,
        float angle,
        Bullet bullet,
        AnimationCurve speedCurve = null
    )
    {
        GameObject bulletObj = PoolGet();

        Sprite sprite = GetSprite(bullet.sprite_name);

        position.z = this.transform.position.z; // ensure bullet is on the same plane as spawner

        // play tan1 or tan2 random
        //float rand = Random.value;
        SoundManager.Instance.Play("tan2", 0.2f, 1.2f, 0.1f);

        BulletMovement bm = bulletObj.GetComponent<BulletMovement>();
        bm.Initialize(
            this,
            position,
            angle,
            speedCurve,
            sprite,
            bullet.color,
            bullet.hitboxRadius,
            bullet.tagName
        );

        return bm;
    }

    public BulletMovement SpawnBullet(
        Vector3 position,
        float angle,
        Bullet bullet,
        float speed
    )
    {
        AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, speed));
        return SpawnBullet(position, angle, bullet, speedCurve);
    }

    public void ResetBullets()
    {
        foreach (GameObject o in activeBullets)
        {
            BulletMovement bm = o.GetComponent<BulletMovement>();
            if (bm != null)
            {
                bm.ClearBullet();
            }
        }
    }   

}

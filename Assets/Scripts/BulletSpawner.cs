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
        }
    }

    public GameObject PoolGet()
    {
        if (pool.Count <= 0) {
            var b2 = Instantiate(bulletPrefab, transform);
            b2.SetActive(false);
            pool.Enqueue(b2);
        }

        var b = pool.Dequeue();
        b.SetActive(true);
        activeBullets.Add(b);
        return b;
    }          

    public void PoolReturn(GameObject b)        
    {
        b.SetActive(false);
        activeBullets.Remove(b);
        pool.Enqueue(b);
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
        float rand = Random.value;
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
        List<GameObject> bulletsToClear = new List<GameObject>(activeBullets);
        foreach (GameObject o in activeBullets)
        {
            bulletsToClear.Add(o);
        }
        foreach (GameObject o in bulletsToClear)
        {
            PoolReturn(o);
        }
    }   

}

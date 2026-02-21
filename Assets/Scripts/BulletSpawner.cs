using UnityEngine;

public class Bullet {
    public Vector3 direction;
    public float speed;
    public string sprite_name;
    public Color color;
    public float hitboxRadius;
    public string tagName;

    public Bullet(
        Vector3 dir,
        float spd,
        string spr,
        Color col,
        float hitboxRad,
        string tag
    )
    {
        direction = dir.normalized;
        speed = spd;
        sprite_name = spr;
        color = col;
        hitboxRadius = hitboxRad;
        tagName = tag;
    }
}

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject heartPrefab;
    public static BulletSpawner Instance;

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
    }

    public void SpawnBullet(
        Bullet bullet
    )
    {
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);

        Sprite sprite = Resources.Load<Sprite>(bullet.sprite_name + ".png");

        BulletMovement bm = bulletObj.GetComponent<BulletMovement>();
        bm.Initialize(
            bullet.direction,
            bullet.speed,
            sprite,
            bullet.color,
            bullet.hitboxRadius,
            bullet.tagName
        );
    }

    public void ClearBullets()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }   

    public void SpawnHeart(Vector3 position)
    {
        
        GameObject heartObj = Instantiate(heartPrefab, position, Quaternion.identity, transform);

        SnapToTarget s = heartObj.GetComponent<SnapToTarget>();
        s.snapTarget = GameManager.Instance.bossHeartSnapTarget.transform;
    }
}
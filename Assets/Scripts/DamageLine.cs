using UnityEngine;

public class DamageLine : MonoBehaviour
{
    public float ChargeTime = 2f;
    public float ActiveTime = 2f;
    public float FlashTime = 0.5f;

    public GameObject particleEffectPrefab;
    private float timer = 0;
    private int state = 0;
    private float lastTimestamp = 0;
    private bool yellowColor = true;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;

    private Sprite boxSprite;
    private Sprite swordSprite;


    float ogY;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<BoxCollider2D>();
        col.enabled = false;
        spriteRenderer.color = new Color(1f, 1f, 0f, 0f);
        swordSprite = Resources.Load<Sprite>("Bullet_big_sword");

        ogY = transform.position.y;
        ogY += 0.7f;
        transform.position = new Vector3(transform.position.x, ogY + 2.6f, transform.position.z);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (state == 0)
        {
            float alpha = Mathf.Clamp01(timer / ChargeTime);
            if (timer - lastTimestamp >= FlashTime)
            {
                lastTimestamp = timer;

                yellowColor = !yellowColor;
            }
            Color baseColor = yellowColor ? Color.yellow : new Color(1f, 0.5f, 0f);
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            
            transform.position = new Vector3(transform.position.x, ogY + 2.6f * (1.0f - alpha), transform.position.z);

            if (timer >= ChargeTime)
            {
                state++;
                timer = 0;
                spriteRenderer.color = Color.red;
                spriteRenderer.sprite = swordSprite;
                col.enabled = true;

                SoundManager.Instance.Play("tan0", 0.5f, 0.7f, 0.2f, true);

                particleEffectPrefab.SetActive(true);
            }
        } 
        if (state == 1)
        {
            if (timer >= ActiveTime)
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class DamageLine : MonoBehaviour
{
    public float ChargeTime = 2f;
    public float ActiveTime = 2f;
    public float FlashTime = 0.5f;

    private float timer = 0;
    private int state = 0;
    private float lastTimestamp = 0;
    private bool yellowColor = true;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;

    private Sprite boxSprite;
    private Sprite swordSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = gameObject.GetComponent<BoxCollider2D>();
        col.enabled = false;
        spriteRenderer.color = new Color(1f, 1f, 0f, 0f);
        swordSprite = Resources.Load<Sprite>("Bullet_big_sword");
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

            if (timer >= ChargeTime)
            {
                state++;
                timer = 0;
                spriteRenderer.color = Color.red;
                spriteRenderer.sprite = swordSprite;
                col.enabled = true;
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

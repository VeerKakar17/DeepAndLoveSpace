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
        spriteRenderer.color = Color.yellow;
        swordSprite = Resources.Load<Sprite>("bullet_big_sword");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (state == 0)
        {
            if (timer - lastTimestamp >= FlashTime)
            {
                lastTimestamp = timer;

                yellowColor = !yellowColor;
                spriteRenderer.color = yellowColor ? Color.yellow : Color.orange;
            }
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

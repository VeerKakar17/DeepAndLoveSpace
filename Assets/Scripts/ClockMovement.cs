using UnityEngine;

public class ClockMovement : MonoBehaviour
{
    public Transform MainClock;
    public Transform hourHand;
    public Transform minuteHand;

    public float mainSpeed = 5f;
    public float hourSpeed = 30f;
    public float minuteSpeed = 180f;

    void Update()
    {
        float dt = Time.deltaTime;

        MainClock.Rotate(0f, 0f, -mainSpeed * dt);
        hourHand.Rotate(0f, 0f, -hourSpeed * dt);
        minuteHand.Rotate(0f, 0f, -minuteSpeed * dt);
    }
}
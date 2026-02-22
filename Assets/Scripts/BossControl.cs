using UnityEngine;

public class BossControl : MonoBehaviour
{
    public Transform HeartParent;

    void Awake()
    {
        GameManager.Instance.bossControl = this;
    }

}

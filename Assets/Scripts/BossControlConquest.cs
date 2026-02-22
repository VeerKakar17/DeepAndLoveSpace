using UnityEngine;

public class BossControlConquest : BossControl
{

    public GameObject bowObject;

    public void HideBow()
    {
        if (bowObject != null)
        {
            bowObject.SetActive(false);
        }
    }

    public void ShowBow()
    {
        if (bowObject != null)
        {
            bowObject.SetActive(true);
        }
    }
}

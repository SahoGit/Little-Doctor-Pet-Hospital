using UnityEngine;

[DisallowMultipleComponent]
class AvoidAppOpen : MonoBehaviour
{
    static bool flag = true;

    void Start()
    {
        gameObject.SetActive(flag);
    }

    float timer = 0;
    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer > 1)
        {
            if (flag && Input.touchCount > 0)
            {
                flag = false;
                gameObject.SetActive(false);
                AdsManager.Instance.SetAppOpenAutoShow(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : MonoBehaviour
{
    private Transform visual;

    // Start is called before the first frame update
    void Start()
    {
        visual = transform.gameObject.transform;
        StartCoroutine(LootFloatAni());
        visual.eulerAngles = new Vector3(visual.eulerAngles.x + 30, visual.eulerAngles.y + 45, visual.eulerAngles.z);

    }

    private IEnumerator LootFloatAni()
    {
        while (true)
        {
            // visual.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);

            visual.position = new Vector3(transform.position.x, 0.4f + Mathf.Sin(Time.time) * 0.2f, transform.position.z);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerArmature")
        {
            Destroy(gameObject);
        }
    }
}

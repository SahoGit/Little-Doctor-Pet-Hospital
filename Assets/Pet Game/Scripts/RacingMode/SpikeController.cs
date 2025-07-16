using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public float spikeSpeed;
    public int score;
    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.left * spikeSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        //if (collision.gameObject.CompareTag("meat"))
        //{
        //    Destroy(collision.gameObject);
        //}



        //if (collision.gameObject.CompareTag("eatingItem"))
        //{
        //    Destroy(this.gameObject);
        //}
    }
}

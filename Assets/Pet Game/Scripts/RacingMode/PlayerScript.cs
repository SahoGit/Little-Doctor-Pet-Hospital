using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerScript : MonoBehaviour
{
    public float JumpForce;

    public bool isGrounded = false;
    public bool isGameStart = false;
    public GameObject EndGamePanel;
    public GameObject GamePlayManager;
    public Text score;

    //public Button JumpButton;

    public Image FilledBar;

    private Animator anim;

    private Rigidbody2D RB;

    public GameObject SecondChanceBtn;
    public Image FilledImg;
    // Start is called before the first frame update
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isGameStart)
        //{
        //    if (Input.touchCount > 0)
        //    {
        //        if (isGrounded == true)
        //        {
        //            RB.AddForce(Vector2.up * JumpForce);
        //            anim.SetBool("Jump", true);
        //            isGrounded = false;
        //        }
        //        //Debug.Log(isGrounded);
        //    }
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isGrounded == false)
            {
                isGrounded = true;
                anim.SetBool("Jump", false);
            }
        }


        if (collision.gameObject.CompareTag("eatingItem"))
        {
            SoundManager.instance.PlayEatSound();
            anim.SetBool("Eat", true);
            Invoke("EatAnimation", 0.01f);
            score.text = (int.Parse(score.text) + collision.gameObject.GetComponent<SpikeController>().score).ToString();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("CrossHurdle"))
        {
            score.text = (int.Parse(score.text) + 5).ToString();
        }

        if (collision.gameObject.CompareTag("Hurdle"))
        {
            SoundManager.instance.PlayHitHurdleSound();
            if (FilledBar.fillAmount > 0.4)
            {
                FilledBar.fillAmount = FilledBar.fillAmount - 0.2f;
                Destroy(collision.gameObject);
            }
            else 
            {
                FilledBar.fillAmount = FilledBar.fillAmount - 0.2f;
                EndGamePanel.SetActive(true);
                Destroy(collision.gameObject);
                SecondChance();
                GamePlayManager.GetComponent<GamePlayManager>().StopGamePlay();
                //Destroy(this.gameObject);
            }
            //anim.SetBool("Eat", true);
            //Invoke("EatAnimation", 0.01f);
        }
    }
    private void EatAnimation()
    {
        anim.SetBool("Eat", false);
    }

    public void SecondChance()
    {
        Debug.Log("SecondChance");
        StartCoroutine(FillAction(FilledImg));
        Invoke("SecondChanceOff", 6.0f);
    }

    IEnumerator FillAction(Image img)
    {
        if (img.fillAmount < 1)
        {
            img.fillAmount = img.fillAmount + 0.004f;
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(FillAction(img));
        }
        else if (img.color.a >= 1f)
        {
            StopCoroutine(FillAction(img));
        }
    }

    void SecondChanceOff()
    {
        SecondChanceBtn.SetActive(false);
    }
}

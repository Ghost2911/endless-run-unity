using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]

public class PlayerInput : MonoBehaviour
{
    public const float gravity = 20.0f;
    public const float jumpHeight = 2.5f;
    public GameObject menu;

    public const float MAX_SWIPE_TIME = 0.5f;
    public const float MIN_SWIPE_DISTANCE = 0.17f;

    public Animation anim;
    public Text txtCoins;

    public bool debugWithArrowKeys = true;
    bool move = false;

    Vector2 startPos;
    float startTime;

    Rigidbody r;
    bool grounded = false;
    bool gameOver = false;
    Vector3 defaultScale;

    int coinNow = 0;

    void Start()
    {
        anim = this.GetComponent<Animation>();
        r = GetComponent<Rigidbody>();

        coinNow = PlayerPrefs.GetInt("coins");
        txtCoins.text = coinNow.ToString();
        string skinName = PlayerPrefs.GetString("skin", "default");
        GameObject obj = Resources.Load("skin/" + skinName) as GameObject;
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = obj.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                    startTime = Time.time;
                }

                if (t.phase == TouchPhase.Ended)
                {
                    if (Time.time - startTime > MAX_SWIPE_TIME)
                        return;

                    Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                    Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                    if (swipe.magnitude < MIN_SWIPE_DISTANCE)
                        return;

                    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    { 
                        // Horizontal swipe
                        if (swipe.x > 0)
                        {
                            if (grounded)
                            {
                                r.AddRelativeForce(new Vector2(3, 2), ForceMode.Impulse);
                                anim.Play("Right");
                            }
                        }
                        else
                        {
                            if (grounded)
                            {
                                r.AddRelativeForce(new Vector2(-3, 2), ForceMode.Impulse);
                                anim.Play("Left");
                            }
                        }
                    }
                    else
                    { 
                        // Vertical swipe
                        if (swipe.y > 0)
                        {
                            if (grounded)
                            {
                                r.AddForce(new Vector2(0, 5), ForceMode.Impulse);
                                anim.Play("Jump");
                            }
                        }
                        else
                        {
                            r.AddForce(new Vector2(0, -5), ForceMode.Impulse);
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));
        grounded = false;
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            gameOver = true;
            UIDraw.instance.gameOver = true;
            GroundGenerator.instance.gameOver = true;
            if (PlayerPrefs.GetInt("bestScore", 0) < GroundGenerator.instance.score)
                PlayerPrefs.SetInt("bestScore", GroundGenerator.instance.score);
            anim.Play("Skin");
            menu.SetActive(true);
            PlayerPrefs.SetInt("coins", coinNow);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            txtCoins.text = (++coinNow).ToString();
        }

        if (other.gameObject.tag == "Jumper")
        {
            other.enabled = false;
            r.AddForce(new Vector2(0, 15), ForceMode.Impulse);
            anim.Play("Jump");
        }
    }
}
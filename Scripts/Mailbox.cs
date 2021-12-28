using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour
{

    GameObject lettershown;
    SpriteRenderer lettersr;
    [SerializeField] Sprite[] letters;

    SpriteRenderer sr;
    [SerializeField] Sprite[] box;

    bool newMail = true;

    float readTime = 0.5f;
    float lastOpened = 0f;

    float nextMail = 60.0f * 3;
    
    private AudioSource plop;

    // Start is called before the first frame update
    void Start()
    {
        lettershown = GameObject.Find("Letter");
        lettersr = lettershown.GetComponent<SpriteRenderer>();
        lettershown.SetActive(false);

        sr = GetComponent<SpriteRenderer>();
        plop = GetComponent<AudioSource>();
        plop.volume = 0.30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (lettershown.activeSelf && Input.GetMouseButtonDown(0) && Time.time - lastOpened >= readTime) {
            lettershown.SetActive(false);
        }
        if (!newMail && Time.time - lastOpened >= nextMail) {
            newMail = true;
        }
        if (!newMail) {
            sr.sprite = box[0];
        }
        else {
            sr.sprite = box[1];
        }
    }

    void OnMouseDown() 
    {
        if (!lettershown.activeSelf && newMail) {
            plop.Play();
            lettershown.SetActive(true);
            lettersr.sprite = letters[Random.Range(0, letters.Length)];
            lastOpened = Time.time;
            newMail = false;
        }
    }
}

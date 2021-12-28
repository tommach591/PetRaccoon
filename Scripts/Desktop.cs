using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desktop : Building
{
    public bool occupied = false;
    private GameObject chair;
    private Collider2D chairCol;
    private GameObject chairOverlap;

    private GameObject boy;
    private GameObject girl;
    private Collider2D boyCol;
    private Collider2D girlCol;

    private int raccoonID = -1;

    [SerializeField] public Sprite[] screens;
    SpriteRenderer monitor;
    GameObject cursor;
    float lastChangeScreen;
    float changeScreenCD = 4.0f;

    private AudioSource click;

    // Start is called before the first frame update
    void Start()
    {
        chair = GameObject.Find("PC Chair");
        chairCol = chair.GetComponent<Collider2D>();
        chairOverlap = GameObject.Find("PC Chair Overlap");

        boy = GameObject.Find("Boy");
        girl = GameObject.Find("Girl");

        boyCol = boy.GetComponent<Collider2D>();
        girlCol = girl.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(boyCol, chairCol, true);
        Physics2D.IgnoreCollision(girlCol, chairCol, true);

        monitor = GameObject.Find("Screen").GetComponent<SpriteRenderer>();
        monitor.sprite = screens[0];
        cursor = GameObject.Find("Cursor");

        chairOverlap.SetActive(false);
        cursor.SetActive(false);

        click = GetComponent<AudioSource>();
        click.volume = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        if (occupied && !click.isPlaying) {
            click.Play();
        }
        if (!occupied) {
            click.Stop();
            monitor.sprite = screens[0];
        }
        if (occupied && Time.time - lastChangeScreen >= changeScreenCD) {
            monitor.sprite = screens[Random.Range(1, screens.Length - 1)];
            if (Random.Range(0, 100) == 0) {
                monitor.sprite = screens[screens.Length - 1];
            }
            lastChangeScreen = Time.time;
        }
    }

    override public void OnTriggerStay2D(Collider2D collider) {
        GameObject collided = collider.gameObject;
        if (collided.tag == "Raccoon") {
            Raccoon r = collided.GetComponent<Raccoon>();
            if (r.onTopBuilding && !r.pickedup && inUse[r.id] == 0 && !r.occupied && !occupied) {
                if (r.beingFlicked) {
                    r.onTopBuilding = false;
                }
                else {
                    lastRelaxing = StartCoroutine(relaxing(r));
                }
            }
            if (inUse[r.id] == 0 && lastRelaxing != null && raccoonID == r.id) {
                StopCoroutine(lastRelaxing);
                occupied = false;
                chairOverlap.SetActive(false);
                cursor.SetActive(false);
                if (r.id == 0) {
                    Physics2D.IgnoreCollision(boyCol, chairCol, true);
                }
                else {
                    Physics2D.IgnoreCollision(girlCol, chairCol, true);
                }
                raccoonID = -1;
            }
            if (occupied) {
                r.onTopBuilding = false;
            }
        }
    }

    override public IEnumerator relaxing(Raccoon r) {
        raccoonID = r.id;
        inUse[r.id] = 1;
        occupied = true;
        r.occupied = true;
        r.onTopBuilding = false;
        chairOverlap.SetActive(true);
        cursor.SetActive(true);
        if (r.id == 0) {
            Physics2D.IgnoreCollision(boyCol, chairCol, false);
        }
        else {
            Physics2D.IgnoreCollision(girlCol, chairCol, false);
        }
        if (!r.facingLeft) {
            r.flip();
        }
        monitor.sprite = screens[Random.Range(1, screens.Length - 1)];
        if (Random.Range(0, 100) == 0) {
            monitor.sprite = screens[screens.Length - 1];
        }
        lastChangeScreen = Time.time;
        r.transform.position = transform.position + offset;
        r.current = this;
        yield return new WaitForSeconds(relaxTime);
        raccoonID = -1;
        inUse[r.id] = 0;
        if (r.id == 0) {
            Physics2D.IgnoreCollision(boyCol, chairCol, true);
        }
        else {
            Physics2D.IgnoreCollision(girlCol, chairCol, true);
        }
        occupied = false;
        r.occupied = false;
        chairOverlap.SetActive(false);
        cursor.SetActive(false);
    }
}

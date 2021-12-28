using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeShop : Building
{
    public Vector3 leftSeatPos;
    public Vector3 rightSeatPos;

    private GameObject leftChair;
    private GameObject rightChair;
    private Collider2D leftChairCol;
    private Collider2D rightChairCol;

    private GameObject boy;
    private GameObject girl;
    private Collider2D boyCol;
    private Collider2D girlCol;

    public Coroutine boyRelaxing;
    public Coroutine girlRelaxing;

    private GameObject leftCoffee;
    private GameObject rightCoffee;
    float lastLeftCoffeeDrank;
    float lastRightCoffeeDrank;
    float makeCoffeeTimer = 60.0f * 7;

    AudioSource slurp;

    // Start is called before the first frame update
    void Start()
    {
        leftChair = GameObject.Find("Chair 1");
        rightChair = GameObject.Find("Chair 2");

        leftChairCol = leftChair.GetComponent<Collider2D>();
        rightChairCol = rightChair.GetComponent<Collider2D>();

        boy = GameObject.Find("Boy");
        girl = GameObject.Find("Girl");

        boyCol = boy.GetComponent<Collider2D>();
        girlCol = girl.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(boyCol, leftChairCol, true);
        Physics2D.IgnoreCollision(boyCol, rightChairCol, true);
        Physics2D.IgnoreCollision(girlCol, leftChairCol, true);
        Physics2D.IgnoreCollision(girlCol, rightChairCol, true);

        leftCoffee = GameObject.Find("Coffee 1");
        rightCoffee = GameObject.Find("Coffee 2");

        slurp = GetComponent<AudioSource>();
        slurp.volume = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftCoffee.activeSelf && Time.time - lastLeftCoffeeDrank >= makeCoffeeTimer) {
            leftCoffee.SetActive(true);
        }
        if (!rightCoffee.activeSelf && Time.time - lastRightCoffeeDrank >= makeCoffeeTimer) {
            rightCoffee.SetActive(true);
        }
    }

    override public void OnTriggerStay2D(Collider2D collider) {
        GameObject collided = collider.gameObject;
        if (collided.tag == "Raccoon") {
            Raccoon r = collided.GetComponent<Raccoon>();
            if (r.onTopBuilding && !r.pickedup && inUse[r.id] == 0 && !r.occupied) {
                if (r.beingFlicked) {
                    r.onTopBuilding = false;
                }
                else {
                    if (r.id == 0)
                        boyRelaxing = StartCoroutine(relaxing(r));
                    if (r.id == 1)
                        girlRelaxing = StartCoroutine(relaxing(r));
                }
            }
            if (inUse[r.id] == 0 && boyRelaxing != null && r.id == 0) {
                StopCoroutine(boyRelaxing);
                Physics2D.IgnoreCollision(boyCol, leftChairCol, true);
            }
            if (inUse[r.id] == 0 && girlRelaxing != null && r.id == 1) {
                StopCoroutine(girlRelaxing);
                Physics2D.IgnoreCollision(girlCol, rightChairCol, true);
            }
        }
    }

    override public IEnumerator relaxing(Raccoon r) {
        inUse[r.id] = 1;
        r.occupied = true;
        r.onTopBuilding = false;
        if (r.id == 0) {
            Physics2D.IgnoreCollision(boyCol, leftChairCol, false);
            r.transform.position = leftSeatPos + offset;
            if (leftCoffee.activeSelf) {
                leftCoffee.SetActive(false);
                slurp.Play();
                lastLeftCoffeeDrank = Time.time;
            }
        }
        else {
            Physics2D.IgnoreCollision(girlCol, rightChairCol, false);
            r.transform.position = rightSeatPos + offset;
            if (rightCoffee.activeSelf) {
                rightCoffee.SetActive(false);
                slurp.Play();
                lastRightCoffeeDrank = Time.time;
            }
        }
        r.current = this;
        yield return new WaitForSeconds(relaxTime);
        inUse[r.id] = 0;
        if (r.id == 0) {
            Physics2D.IgnoreCollision(boyCol, leftChairCol, true);
        }
        else {
            Physics2D.IgnoreCollision(girlCol, rightChairCol, true);
        }
        r.occupied = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int[] inUse = {0, 0};
    public Vector3 offset;
    public Coroutine lastRelaxing;
    public float relaxTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void OnTriggerStay2D(Collider2D collider) {
        GameObject collided = collider.gameObject;
        if (collided.tag == "Raccoon") {
            Raccoon r = collided.GetComponent<Raccoon>();
            if (r.onTopBuilding && !r.pickedup && inUse[r.id] == 0 && !r.occupied && !r.beingFlicked) {
                lastRelaxing = StartCoroutine(relaxing(r));
            }
            if (inUse[r.id] == 0 && lastRelaxing != null) {
                StopCoroutine(lastRelaxing);
            }
        }
    }

    virtual public IEnumerator relaxing(Raccoon r) {
        inUse[r.id] = 1;
        r.occupied = true;
        r.onTopBuilding = false;
        r.transform.position = transform.position + offset;
        r.current = this;
        yield return new WaitForSeconds(relaxTime);
        inUse[r.id] = 0;
        r.occupied = false;
    }

}

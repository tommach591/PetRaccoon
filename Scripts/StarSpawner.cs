using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] ShootingStar starPrefab;
    [SerializeField] Sprite[] starTypes;
    Sun sun;

    float spawnChance = 15;

    float spawnAttemptCD = 5f;
    float lastSpawnChance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.Find("Sun").GetComponent<Sun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!sun.daytime && Time.time - lastSpawnChance >= spawnAttemptCD) {
            if (Random.Range(0, 100) <= spawnChance) {
                StartCoroutine(spawnStars());
            }
            lastSpawnChance = Time.time;
        }
    }

    IEnumerator spawnStars() {
        for (int i = 0; i < 3; i++) {
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y + Random.Range(-1.0f, 1.0f), 0);
            ShootingStar star = Instantiate(starPrefab, newPos, transform.rotation);
            star.starType = starTypes[i];
            yield return new WaitForSeconds(0.25f);
        }
    }
}

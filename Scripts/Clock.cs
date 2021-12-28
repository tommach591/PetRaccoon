using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update

    Text time;
    Text date;

    void Start()
    {
        time = transform.GetChild(0).GetComponent<Text>();
        date = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string datetime = System.DateTime.Now.ToString();
        string[] datetimesplit = datetime.Split(' ');
        string[] timesplit = datetimesplit[1].Split(':');

        date.text = datetimesplit[0];
        time.text = timesplit[0] + ":" + timesplit[1] + " " + datetimesplit[2];
    }
}

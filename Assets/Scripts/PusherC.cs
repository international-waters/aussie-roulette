using UnityEngine;
using System.Collections;

public class PusherC : MonoBehaviour
{

    //  private int longspindemo = 0;
    GameObject thepusherGO;
    Vector3 rot;
    float ballpushermovementvariable = 100;
    // Use this for initialization
    void Start()
    {
        thepusherGO = GameObject.Find("pushertomoveball");
        initialiserandompusher();
    }

    // Update is called once per frame
    void Update()
    {
        randommovementpusher();
    }

    void initialiserandompusher()
    {
        ballpushermovementvariable = 100;
        rot.y = 360 * Random.value;
        thepusherGO.transform.eulerAngles = rot;
    }

    void randommovementpusher()
    {
        float lengthtimetoroll = .998f;
        if (ballpushermovementvariable >= 1)
        {
            ballpushermovementvariable = ballpushermovementvariable * lengthtimetoroll + (0.0001f * Random.value);/////change the 0.995 value to something like 0.999 or bigger for the ball to roll around longer-near zero means less rolling around
            rot.y = rot.y + ballpushermovementvariable * Time.deltaTime;
            if (rot.y > 360) { rot.y -= 360; }
            if (rot.y < 360) { rot.y += 360; }
            thepusherGO.transform.eulerAngles = rot;
        }
    }
}
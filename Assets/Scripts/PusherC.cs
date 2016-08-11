using UnityEngine;
using System.Collections;

public class PusherC : MonoBehaviour {

    private int longspindemo = 0;
    GameObject thepusherGO;
    Vector3 rot;
    int doonce = 1;
    int findobjonce = 1;
    int dorandomspin = 1;
    float ballpushermovementvariable = 100;
                                         
 


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (findobjonce == 1)
        {
            thepusherGO = GameObject.Find("pushertomoveball");
            findobjonce = 2;
        }

        if (doonce == 1)
        {
            if (dorandomspin == 1) { initialiserandompusher(); }
   
        }

        if (dorandomspin == 1) { randommovementpusher(); }
   

   
    }


    void initialiserandompusher()
    {
        ballpushermovementvariable = 100;
        rot.y = 360 * Random.value;
        thepusherGO.transform.eulerAngles = rot;
        doonce = 2;
    }
 
    ////////////////////--------------------------------------------------------------------------------------------------------------------
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
    /////////////---------------------------------------------------------------------------------------------------------------------------------
 
   

    }

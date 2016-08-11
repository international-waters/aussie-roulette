using UnityEngine;
using System.Collections;

public class PusherC : MonoBehaviour {

    private int longspindemo = 0;
    GameObject thepusherGO;
    Vector3 rot;
    int doonce = 1;
    int findobjonce = 1;/////used to find game pusher object once
    int dorandomspin = 1;//// set to 1 for random set to 0 for predetermined number at start o fupdate loop
    float ballpushermovementvariable = 100;//////the function initialiserandompusher,randommovementpusher of this below determines how the pusher moves
                                           /////replace above with your own function if you want to setting dorandomspin=0 uses initialisenonrandompusher,nonrandommovementpusher instead of above
                                           ///////-----the follwoing three example are for non-random spin
    float counter = 0;////used in determinsitic non-random spin
    int increasepusherangle = 1;////this is either 0 or 1


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
   

    //    if ((RotateWheelC.spinstoppedglobalcopy) == 1)
   //     {//////this global variable set in other sctript
     //       if (Input.GetKeyDown("space"))  //////need to add code to use spinstopped var here so when it is 1 space is allowed when its 0 space press is not allow -similar to roatate hell script
    //        { doonce = 1; }////

      //  }
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

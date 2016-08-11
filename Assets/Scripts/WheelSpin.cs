
/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* This class contains the event handlers for spinning the wheel... 
* TODO not sure if this with 
****************************************************************************/


using UnityEngine;
using System.Collections;

public class WheelSpin : MonoBehaviour {
	public Vector3 rot;
	float wheelSpinTime = 121;
    /// <summary>
    /// ////
    /// 

    /// </summary>
    public int RealisticSpin = 0; // 0 quick game play, 1 realistic spin time.
    public int winningNumber;
    private float proximity;
    private Vector3 ballinitial; //ball intial position
    private Vector3 ballfinal;
   
    private bool doneOnce = false; //private hasspinstopped: GUIText;/////if still spinnning message-
    private int numbersinwheel = 37;////// simple the 37 number is a roulette wheel a constant
                                    //	private int spinagain = 0;/////this is set to true after the  spin is finished
    private static float infinity = 999999;/////a large number will serve as infinity
    private Vector3[] posofnumbers ;////array for 3d positions of each collision spehre for each number (0 to 36) is in the wheel
                                   //////////////////////////
    private int getfinalpositionofpusherandballonce = 1;////used to get final postion of ball once every game
    private Quaternion finalpusherorientation;/////use to stop the pusher (it spinns around at fixed point (0,0,0) at the end of every game
                                              //var numberofspins: int =1;////// numnber of spins 
    private float[] distancefromnums;///// array of floats used to find distance of ball from each number
    private float minimaldistance = infinity;/////used to find which number the ball is near , infinity means not near any anumber
    private float minimaldistancetocheckifnearanumber = infinity;////// used to find number which ball is near while the wheel is still spinning
    private float timerstart = 0; ////timer value start for every game get reset back to 0
    private float ballnearsomenumber = infinity;////////means intially not near any number because its at infinity
    private float timecounter = 0;////the timer code to determine the time of spin is not actually used
    private float timeelapsed = 0;////timeelspsed is the time elapsed since the spin 
    private static int mintimeforeachspin = 35; ////e.g. setting minimaltimeforeachspin (this var makes the ball move up to 40 seconds)  40 seconds makes the ball roll longer near end of spin maybe to add some excitement, setting to 0 is same as ignoring the ccariable
    private float timetakenforthespin = 0;////the time taken for the spin from when to ball and whell stop and start
    private Vector3 notnearvector;
    ///// used for doing initlisation once
    private int loop = 0;  ///// used in loops
    private int numberstop = 0; //////the actual number from 0 to 36 then ball stopped on
    private bool spinstopped = false;/////1 if stopped spinning ,0 if still spinning
    public static int spinstoppedglobalcopy = 0;
    //private float minimaldistancetocheckifnearanumber= infinity;
    private int cameraposition = 2;////1 is defined as top view , is defines as near wheel view
    private float ballnearnesstonnumberdistancetostop = 0.235F;/////detemines how near the ball is to some number to stop. Nearer to zero means stop centre of the number
    private Vector3 camerarot;
    private Vector3 ballPos;
    private Vector3 ballFinalPos;


     GameObject number0;///game object this hold the collsion sphere for the zero number
	 GameObject number1;/////similar to above but for the number 1
    GameObject number2;
     GameObject number3;
    GameObject number4;
    private GameObject number5;
    private GameObject number6;
    private GameObject number7;
    private GameObject number8;
    private GameObject number9;
    private GameObject number10;
    private GameObject number11;
    private GameObject number12;
    private GameObject number13;
    private GameObject number14;
    private GameObject number15;
    private GameObject number16;
    private GameObject number17;
    private GameObject number18;
    private GameObject number19;
    private GameObject number20;
    private GameObject number21;
    private GameObject number22;
    private GameObject number23;
    private GameObject number24;
    private GameObject number25;
    private GameObject number26;
    private GameObject number27;
    private GameObject number28;
    private GameObject number29;
    private GameObject number30;
    private GameObject number31;
    private GameObject number32;
    private GameObject number33;
    private GameObject number34;
    private GameObject number35;
    private GameObject number36;
    //////////////now declare game object for ball,pusher, camera and wheel
    public GameObject ball;
    public GameObject pusher;
    public GameObject wheel;
    public GameObject GOcamera;
    // public GameObject TextResult;

 //  public TextMesh textobj;

    // Use this for initialization
    void Start () {
        initialiseGameObj();
        resetcollsiononnumbervalues();
       
    }

    // Update is called once per frame
    void Update () {
       // if (doneOnce == true)
      //  {
      //      resetcollsiononnumbervalues();
      //      doneOnce = false;
     //   }

        if (spinstopped == false)
            {
            dowheelmovementfunction();
            Debug.Log(spinstopped); }
        else
        ////use the sphereX for collison detect-this is the destination collision point
        //  computedistancefromanumber(ball.transform.position);
              {
            // ballandnumbercollisiondetection();

            // ballFinalPos = ball.transform.position;
           // ballstopped();
Debug.Log("test");
          
          //  Vector3 v = number0.transform.position;

            Debug.Log("num transform6");
         //   Debug.Log(v.ToString());
           // for (var loop = 0; loop <= numbersinwheel; loop++)
          //  {
  //


          //  }


          


            ballandnumbercollisiondetection();
          
        }

     


    }

    /// controls Wheel Movement
    void dowheelmovementfunction()
	{
		float timetospin = .05f;
		
		{
			
			wheelSpinTime=wheelSpinTime-timetospin;
		
			rot.y = rot.y + wheelSpinTime * Time.deltaTime;
			if (rot.y > 360) {rot.y -= 360;}
			if (rot.y < 360) {rot.y += 360;}
			transform.eulerAngles = -1*rot;
          
           
        }
	

		if (wheelSpinTime<-0.1)

        {
            wheelSpinTime = 0;
            spinstopped = true; ////this controls the wheel 
        };
       
Debug.Log(spinstopped);
    }

    void initialiseGameObj ()
    {
        ball = GameObject.Find("Ball");
        wheel = GameObject.Find("roulettewheel");
        GOcamera = GameObject.Find("thecamera");
        pusher = GameObject.Find("pushertomoveball");
        number0 = GameObject.Find("Sphere0");
        number1 = GameObject.Find("Sphere1");
        number2 = GameObject.Find("Sphere2");
        number3 = GameObject.Find("Sphere3");
        number4 = GameObject.Find("Sphere4");
        number5 = GameObject.Find("Sphere5");
        number6 = GameObject.Find("Sphere6");
        number7 = GameObject.Find("Sphere7");
        number8 = GameObject.Find("Sphere8");
        number9 = GameObject.Find("Sphere9");
        number10 = GameObject.Find("Sphere10");
        number11 = GameObject.Find("Sphere11");
        number12 = GameObject.Find("Sphere12");
        number13 = GameObject.Find("Sphere13");
        number14 = GameObject.Find("Sphere14");
        number15 = GameObject.Find("Sphere15");
        number16 = GameObject.Find("Sphere16");
        number17 = GameObject.Find("Sphere17");
        number18 = GameObject.Find("Sphere18");
        number19 = GameObject.Find("Sphere19");
        number20 = GameObject.Find("Sphere20");
        number21 = GameObject.Find("Sphere21");
        number22 = GameObject.Find("Sphere22");
        number23 = GameObject.Find("Sphere23");
        number24 = GameObject.Find("Sphere24");
        number25 = GameObject.Find("Sphere25");
        number26 = GameObject.Find("Sphere26");
        number27 = GameObject.Find("Sphere27");
        number28 = GameObject.Find("Sphere28");
        number29 = GameObject.Find("Sphere29");
        number30 = GameObject.Find("Sphere30");
        number31 = GameObject.Find("Sphere31");
        number32 = GameObject.Find("Sphere32");
        number33 = GameObject.Find("Sphere33");
        number34 = GameObject.Find("Sphere34");
        number35 = GameObject.Find("Sphere35");
        number36 = GameObject.Find("Sphere36");

        
    }

    void finddistfromsomenumber()
    {
        ///// This determies the distance of the ball from some number and
        /////this function is used to decide when to stop the wheel which is when
        /////it is near  to some number and the nearness to a number is determined by this function
        for (var loop = 0; loop <= numbersinwheel; loop++)
        {
            if (minimaldistancetocheckifnearanumber > (distancefromnums[loop]))
            {
                minimaldistancetocheckifnearanumber = distancefromnums[loop];
            }
        }
    }


    void resetcollsiononnumbervalues()
    {//////this function resets the vlaues for collosion detectiom
     /////////works by storing all the collsion spheres for each number and
     //////// comparing the ditance from ball
        notnearvector.x = infinity;
        notnearvector.y = infinity;
        notnearvector.z = infinity;
        posofnumbers = new Vector3[numbersinwheel + 2];///// array of 3d vectors to hold ball psotion numbers
        distancefromnums = new float[numbersinwheel + 2];////39 values to decide which the ball is nearest to
        for (var loop = 0; loop <= (numbersinwheel + 1); loop++)
        {
            distancefromnums[loop] = infinity;///// means not near
            posofnumbers[loop] = notnearvector;
        }
    }
    ////////////////////////////////////////////

    void ballstopped()////obsolite
    {
        /////the ball is moved by the pusher so stop the pusher first then stop the ball using ths fucntion
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.GetComponent<ConstantForce>().force = Vector3.zero;
        ball.GetComponent<ConstantForce>().relativeForce = Vector3.zero;
        ball.GetComponent<ConstantForce>().torque = Vector3.zero;
        ball.transform.position = ballfinal;
        //	ball.transform.position.x=ballfinal.x;
        //	ball.transform.position.y=ballfinal.y;
        //	ball.transform.position.z=ballfinal.z;
    }

    void getnumberpositions()
    {
        
        /////this get the collision spheres 3D positions for each number so they can be plugged into
        /////the collision detection function
        ////////can use array to make smaller but this is easier to understand
      posofnumbers[0] = number0.transform.position;
        posofnumbers[1] = number1.transform.position;
        posofnumbers[2] = number2.transform.position;
        posofnumbers[3] = number3.transform.position;
        posofnumbers[4] = number4.transform.position;
        posofnumbers[5] = number5.transform.position;
        posofnumbers[6] = number6.transform.position;
        posofnumbers[7] = number7.transform.position;
        posofnumbers[8] = number8.transform.position;
        posofnumbers[9] = number9.transform.position;
        posofnumbers[10] = number10.transform.position;
        posofnumbers[11] = number11.transform.position;
        posofnumbers[12] = number12.transform.position;
        posofnumbers[13] = number13.transform.position;
        posofnumbers[14] = number14.transform.position;
        posofnumbers[15] = number15.transform.position;
        posofnumbers[16] = number16.transform.position;
        posofnumbers[17] = number17.transform.position;
        posofnumbers[18] = number18.transform.position;
        posofnumbers[19] = number19.transform.position;
        posofnumbers[20] = number20.transform.position;
        posofnumbers[21] = number21.transform.position;
        posofnumbers[22] = number22.transform.position;
        posofnumbers[23] = number23.transform.position;
        posofnumbers[24] = number24.transform.position;
        posofnumbers[25] = number25.transform.position;
        posofnumbers[26] = number26.transform.position;
        posofnumbers[27] = number27.transform.position;
        posofnumbers[28] = number28.transform.position;
        posofnumbers[29] = number29.transform.position;
        posofnumbers[30] = number30.transform.position;
        posofnumbers[31] = number31.transform.position;
        posofnumbers[32] = number32.transform.position;
        posofnumbers[33] = number33.transform.position;
        posofnumbers[34] = number34.transform.position;
        posofnumbers[35] = number35.transform.position;
        posofnumbers[36] = number36.transform.position;
        ballFinalPos = ball.transform.position;
        
        /////put postion of roulette wheel number in the array
    }
    /////////////////-----------------
    void computedistancefromanumber(Vector3 ballPos)
    {
        for (int loop = 0; loop <= numbersinwheel; loop++)
        {//////////////computes the distance of ball to each number using the norm of the vector
            distancefromnums[loop] = (posofnumbers[loop] - ballPos).magnitude;
          //  Debug.Log("3");
          //  Debug.Log(distancefromnums[loop]);
        }
    }
    /////////
   // void decideifspinstoppedandprocessthespin()
  //  {
  //      if ((spinstopped == 1) && (ballnearsomenumber < ballnearnesstonnumberdistancetostop) && (timeelapsed > mintimeforeachspin))
   //     {
   //         /////the above is read as if the function controlling the wheel then spin is stopped, the ball has collided near enough
   //         ////to some number and the minimal time required for eash spin has passed
    //        if (getfinalpositionofpusherandballonce == 1)
  //          {
  //              ////this freezes the ball after the elapsed time and other stopping constions are met
                //// because of the frictional forces between 
                //// the ball and table will keep ot drifting and this will be hard to see anyway
  //              ballFinalPos = ball.transform.position;
  //              finalpusherorientation = pusher.transform.rotation;////store the final pusher orientation -not the postion of pusher is fixed around (0,0,0)
  //              getfinalpositionofpusherandballonce = 2;
  //              timetakenforthespin = timeelapsed;
  //          }
   //         ballstopped();////stop the ball
   //         pusher.transform.rotation = finalpusherorientation;/////after the spin stop keep the pusher stopped at final orientation
   //         ballandnumbercollisiondetection();////while the collsion can be done with unity's biult in collision detection
                                              //////system using the custom coded collision detection system here in the code is a good way to do it too
  //      }
  //  }
    ////
    void ballandnumbercollisiondetection()
    {
        //////////////////////begin collision detection routine for ball near numbers 0 to 36

    
     
        getnumberpositions();////use the spheres for collison detect-this is the destination collision point
        computedistancefromanumber(ballFinalPos);
        findnumberstoppedon();

        ////////////end collision detection routine for ball near numbers 0 to 36
    }

    //////
    void findnumberstoppedon()
    {////////////////determines the number stopped upon by finding the nearest ditance to the ball
     //////////not that the disctance can never be equal since the ball keeps moving until
     //////////it converges to some number on the wheel
     //////////this function is used after the wheel has stopped
        for (int loop = 0; loop <= numbersinwheel; loop++)
        {
            if (minimaldistance > (distancefromnums[loop]))
            {
                numberstop = loop;
                minimaldistance = distancefromnums[loop];
           
            }
        }
        Debug.Log(numberstop);
      
    }

    

}


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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WheelSpin : MonoBehaviour {

    public Vector3 rot;
	float wheelSpinTime = 121;
    public int winningNumber;
    private float proximity;
   // private Vector3 ballinitial; //ball intial position
    private Vector3 ballfinal;
    private int numbersinwheel = 37;////// simple the 37 number is a roulette wheel a constant
    private static float infinity = 999999;
    private Vector3[] posofnumbers ;////array for 3d positions of each collision spehre for each number (0 to 36) is in the wheel
    private float[] distancefromnums;///// array of floats used to find distance of ball from each number
    private float minimaldistance = infinity;/////used to find which number the ball is near , infinity means not near any anumber
    private float ballnearsomenumber = infinity;////////means intially not near any number because its at infinity
   // private Vector3 notnearvector;
    private int numberstop = 0; //////the actual number from 0 to 36 then ball stopped on
    private bool spinstopped = false;/////1 if stopped spinning ,0 if still spinning
  //  private int cameraposition = 2;////1 is defined as top view , is defines as near wheel view
  //  private float ballnearnesstonnumberdistancetostop = 0.235F;/////detemines how near the ball is to some number to stop. Nearer to zero means stop centre of the number
  //  private Vector3 camerarot;
    private Vector3 ballPos;
    private Vector3 ballFinalPos;
    private GameObject number0;///game object this hold the collsion sphere for the zero number
	private GameObject number1;/////similar to above but for the number 1
    private GameObject number2;
    private GameObject number3;
    private GameObject number4;
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
    private GameObject ball;
    private GameObject pusher;
    private GameObject wheel;
  //  private GameObject movecamera;
  
	private GameManager game; // holds a reference to main game manager object
	private GameObject winDisplayPanel;
	private bool isReturning = false;
	private Text winnerLabel;
	private int[] blackNumbers = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20,
		22, 24, 26, 28, 29, 31, 33, 35 };


  
    // Use this for initialization
    void Start () {
        initialiseGameObj();
        resetcollsiononnumbervalues();
		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		winnerLabel = GameObject.Find ("WinnerLabel").GetComponent<Text> ();
		winDisplayPanel = GameObject.Find ("WinDisplayPanel");
		winDisplayPanel.SetActive (false);
      }

    // Update is called once per frame
    void Update () {
          if (spinstopped == false)
            {
            dowheelmovementfunction();
            //  Debug.Log(spinstopped); 
        }
        else
          {
			//just want to call this once
			if(!isReturning){
				ballandnumbercollisiondetection ();
				game.winNumberFlag = this.winningNumber;
				DisplayWinPanel ();
				StartCoroutine(returnAfterDelay ()); // pause and then return to game screen
				isReturning = true;
			}      
          }
        }
	IEnumerator returnAfterDelay(){
	yield return new WaitForSeconds (2.0f);
	SceneManager.LoadScene ("GamePlayScreen");
	}

	void DisplayWinPanel(){
		if (this.winningNumber == 0)
			this.winnerLabel.text = "0 Green";
		else {
			bool isBlack = false;
			foreach (int number in blackNumbers) {
				if (this.winningNumber == number) {
					isBlack = true;
					break;
				}
			}
			string colour = (isBlack) ? " Black" : " Red";
			this.winnerLabel.text = winningNumber + colour;
		}
		this.winDisplayPanel.SetActive (true);
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
            spinstopped = true; ////this controls the wheel exit to calc number
        };
       
//Debug.Log(spinstopped);
    }

    void initialiseGameObj ()
    {
        ball = GameObject.Find("Ball");
        wheel = GameObject.Find("roulettewheel");
      //  GOcamera = GameObject.Find("thecamera");
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
        /// Wheel Spin random generator...
        wheelSpinTime = Random.Range(100, 170);
    }

     void resetcollsiononnumbervalues()
    {//////this function resets the vlaues for collosion detectiom
     /////////works by storing all the collsion spheres for each number and
     //////// comparing the ditance from ball
      //  notnearvector.x = infinity;
     //   notnearvector.y = infinity;
      //  notnearvector.z = infinity;
        posofnumbers = new Vector3[numbersinwheel + 2];///// array of 3d vectors to hold ball psotion numbers
        distancefromnums = new float[numbersinwheel + 2];////39 values to decide which the ball is nearest to
        for (var loop = 0; loop <= (numbersinwheel + 1); loop++)
        {
            distancefromnums[loop] = infinity;///// means not near
       //     posofnumbers[loop] = notnearvector;
        }
    }
    ////////////////////////////////////////////

    void getnumberpositions()
    {   /////this get the collision spheres 3D positions for each number so they can be plugged into
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
    /////

    void computedistancefromanumber(Vector3 ballPos)
    {
        for (int loop = 0; loop <= numbersinwheel; loop++)
        {//////////////computes the distance of ball to each number using the norm of the vector
            distancefromnums[loop] = (posofnumbers[loop] - ballPos).magnitude;
        }
    }
   
    void ballandnumbercollisiondetection()
    {
        //////////////////////begin collision detection routine for ball near numbers 0 to 36
        getnumberpositions();////use the spheres for collison detect-this is the destination collision point
        computedistancefromanumber(ballFinalPos);
        findnumberstoppedon();
        ////////////end collision detection routine for ball near numbers 0 to 36
    }
    
    void findnumberstoppedon()
    {////////////////determines the number stopped upon by finding the nearest ditance to the ball
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
        winningNumber = numberstop;

        /////Exit scene with winnningNumber
    }

    

}

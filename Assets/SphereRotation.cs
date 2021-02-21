using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SphereRotation : MonoBehaviour
{
    
    /* Variables for network connection */
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;

    bool condition;
    bool running;


    /*Variables of GameObject */
    GameObject sphere;
    GameObject rightPlane;
    GameObject leftPLane;

    /*GameObject unity names*/
    const string SPHERE = "/Sphere";
    const string RIGHT_PLANE = "/Right Plane";
    const string LEFT_PLANE = "/Left Plane";

 

    // Start is called before the first frame update
    void Awake()
    {
        /* Collect Data from server */
        ThreadStart ts = new ThreadStart(GetInfo);                      // Construct a Start Thread Class with GetInfo()
        mThread = new Thread(ts);                                       // Construct mTHread with 'ts' to use Start()
        mThread.Start();                                              // Start thread


        //Get component of of Render and Animator in the Gameplay
        sphere = GameObject.Find(SPHERE);
        rightPlane = GameObject.Find(RIGHT_PLANE);
        leftPLane = GameObject.Find(LEFT_PLANE);

    }

    // Update is called once per frame
    void Update()
    {

        if (condition == true)
        {

            rightPlane.transform.Rotate(10.0f, 0.0f, 0.0f);
            leftPLane.transform.Rotate(10.0f, 0.0f, 0.0f);
            sphere.transform.Rotate(0, Time.deltaTime*500, 0);
            

            print("Data recieved and ball is being animated");
        }

        condition = false; 
        
    }


    
    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);                       //make the string 'connectionIP' an IP add
        listener = new TcpListener(IPAddress.Any, connectionPort);      //Create Listener for any data
        listener.Start();                                               //Start Listener

        client = listener.AcceptTcpClient();

        running = true;
        while(running)
        {
            RecieveData();
        }
        listener.Stop();
    }

    void RecieveData()
    {
        NetworkStream nwStream = client.GetStream();                    // Initialize a Network Strean obj with a TCPClient obj
        byte[] buffer = new byte[client.ReceiveBufferSize];              //variables to collect byte data in arrays.

        /* To recieve Data from Host */
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
        string dataRecieved = Encoding.UTF8.GetString(buffer, 0, bytesRead);


        if (dataRecieved != null)
        {
            condition = true;
           
        }
        else
        {
            condition = false;

        }
    }
}

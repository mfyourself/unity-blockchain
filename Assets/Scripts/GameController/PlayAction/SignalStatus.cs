using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

public class Signal : MonoBehaviour
{
    // Start is called before the first frame update

    public string serverAddress = ""; // Google's public DNS server
    public float pingInterval = 1.0f; // Ping interval in seconds
    public float timeout = 2.0f; // Timeout for ping in seconds
    public float goodThreshold = 100.0f; // Threshold for considering ping as good
    public float badThreshold = 500.0f; // Threshold for considering ping as bad
    public GameObject High;
    public GameObject Middle;
    public GameObject Low;
    public GameObject Warning;
    private float lastPingTime;
    private Ping ping;
    void Start()
    {
        lastPingTime = Time.time;
        serverAddress = Constant.ServerAddress;
        StartCoroutine(MonitorNetwork());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     IEnumerator MonitorNetwork()
    {
        while (true)
        {
            // Check if it's time to ping
            if (Time.time - lastPingTime >= pingInterval)
            {
                // Start a new ping
                ping = new Ping(serverAddress);
                lastPingTime = Time.time;

                // Wait for the ping to complete
                yield return new WaitForSecondsRealtime(timeout);

                // Check if the ping has completed
                if (ping.isDone)
                {
                    float pingTime = ping.time;

                    // Determine network condition based on ping time
                    if (pingTime <= goodThreshold)
                    {
                         Debug.Log("Network is good. Ping time: " + pingTime + " ms");
                        High.SetActive(true);
                        Middle.SetActive(false);
                        Low.SetActive(false);
                    }
                    else if (pingTime > goodThreshold && pingTime <= badThreshold)
                    {
                         Debug.Log("Network is okay. Ping time: " + pingTime + " ms");
                        High.SetActive(false);
                        Middle.SetActive(true);
                        Low.SetActive(false);
                    }
                    else
                    {
                         Debug.Log("Network is bad. Ping time: " + pingTime + " ms");
                        High.SetActive(false);
                        Middle.SetActive(false);
                        Low.SetActive(true);
                        Warning.SetActive(false);
                        Warning.SetActive(true);
                    }
                }
                else
                {
                   // Debug.Log("Ping timeout. Network may be unreachable.");
                }
            }

            yield return null;
        }
    }
}

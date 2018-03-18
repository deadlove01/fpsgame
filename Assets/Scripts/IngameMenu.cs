using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class IngameMenu : NetworkBehaviour
{

    public GameObject IngameMenuGO;
    public GameObject MenuGO;

    public GameObject pauseGO;


    void Start()
    {
        pauseGO.SetActive(true);
      
        var btnDisconnect = GameObject.Find("BtnDisconnect").GetComponent<Button>();
        btnDisconnect.onClick.RemoveAllListeners();
        btnDisconnect.onClick.AddListener(NetworkManager.singleton.StopHost);
        pauseGO.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseGO.activeInHierarchy == false)
            {
                pauseGO.SetActive(true);
                var localPlayerGo = GameObject.FindGameObjectsWithTag("Player");
                foreach (var o in localPlayerGo)
                {
                    var child = o.GetComponent<FPSController>();
                    if (child != null)
                    {
                        child.CursorLock(false);
                        child.enabled = false;
                        break;
                    }
                }
                // GetComponent<FPSController>().enabled = false;
            }
            else
            {
                ClientResume();
                //GetComponent<FPSController>().enabled = true;
            }
        }
    }

    public void StartOrJoinHost()
    {
        MenuGO.SetActive(false);
        IngameMenuGO.SetActive(true);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void ClientDisconnect()
    {
        MenuGO.SetActive(true);
        IngameMenuGO.SetActive(false);

        NetworkManager.singleton.StopClient();
        //  if (isServer)
        {
            //NetworkManager.singleton.StopHost();
            //NetworkManager.Shutdown();
            //NetworkServer.Shutdown();
            //   NetworkManager.Shutdown();
        }
       // else
        {
            
        }
        //        var localPlayerGo = GameObject.FindGameObjectsWithTag("Player");
        //        foreach (var o in localPlayerGo)
        //        {
        //            var child = o.GetComponent<NetworkIdentity>();
        //            if (child != null)
        //            {
        //                NetworkManager.singleton.
        //                break;
        //            }
        //        }

    }

    public void ClientResume()
    {
        pauseGO.SetActive(false);
        var localPlayerGo = GameObject.FindGameObjectsWithTag("Player");
        foreach (var o in localPlayerGo)
        {
            var child = o.GetComponent<FPSController>();
            if (child != null)
            {
                child.CursorLock(true);
                child.enabled = true;
                break;
            }
        }
    }


   

}

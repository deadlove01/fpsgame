using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        List<Transform> positions = NetworkManager.singleton.startPositions;
        var playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerObj.transform.position = positions[0].position;
        NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);
        //        base.OnServerAddPlayer(conn, playerControllerId);
        print("add player: "+playerControllerId);
       
    }

  
    public void StartupHost()
    {
        print("start host");
        SetupPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        SetupIpAddress();
        SetupPort();
        NetworkManager.singleton.StartClient();
    }


    void SetupIpAddress()
    {
        var ipAddress = GameObject.Find("HostIpAddress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetupPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }


    void OnLevelWasLoaded(int level)
    {
        print("level: " + level);
        if (level == 1)
        {
            SetupMenuScene();
        }
        else if (level == 2)
        {
            SetupIngameScene();
        }
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }


    void SetupMenuScene()
    {
        var btnStartHost = GameObject.Find("BtnStartHost").GetComponent<Button>();
        btnStartHost.onClick.RemoveAllListeners();
        btnStartHost.onClick.AddListener(StartupHost);

        var btnJoinGame = GameObject.Find("BtnJoinGame").GetComponent<Button>();
        btnJoinGame.onClick.RemoveAllListeners();
        btnJoinGame.onClick.AddListener(JoinGame);

        var btnBack = GameObject.Find("BackToMainMenu").GetComponent<Button>();
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(BackToMainMenu);
    }


    void SetupIngameScene()
    {
        var menuGO = GameObject.Find("IngameMenu");
        foreach (var componentsInChild in menuGO.GetComponentsInChildren<Transform>())
        {
            Console.WriteLine(componentsInChild.gameObject.name);
        }
        var btnDisconnect = GameObject.Find("BtnDisconnect").GetComponent<Button>();
        btnDisconnect.onClick.RemoveAllListeners();
        btnDisconnect.onClick.AddListener(NetworkManager.singleton.StopHost);
    }

}

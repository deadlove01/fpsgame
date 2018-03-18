using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerHealth : NetworkBehaviour
{

    [SyncVar] public float health = 100f;
    private float maxHealth = 100f;
    private NetworkStartPosition[] respawnPositions;

    public Text healthText;

    void Start()
    {
        if (isLocalPlayer)
        {
            respawnPositions = GameObject.FindObjectsOfType<NetworkStartPosition>();
        }
        healthText = GameObject.Find("Health").GetComponent<Text>();
    }
    public void TakeDamage(float damage)
    {
        if (!isServer)
        {
            return;
        }
        health -= damage;
        healthText.text = ((int)health) + "";
        print("take damage! health remains: "+health);

        if (health <= 0)
        {
            print("You are death!");
        }
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            health = maxHealth;
            healthText.text = ((int)health) + "";
            var spawnPoint = Vector3.zero;
            if (respawnPositions != null && respawnPositions.Length > 0)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                spawnPoint = respawnPositions[Random.Range(0, respawnPositions.Length)].transform.position;
            }
            transform.position = spawnPoint;
        }
    }
}

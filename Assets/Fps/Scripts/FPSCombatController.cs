using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FPSCombatController : NetworkBehaviour
{

    public GameObject bloodImpact, concreteImpact;

    public void CheckHit(Camera mainCamera)
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            if (hit.transform.tag == "Enemy")
            {
                CmdDealDamage(hit.transform.gameObject, hit.point, hit.normal);
            }
            else
            {
                Instantiate(concreteImpact, hit.point, Quaternion.LookRotation(hit.normal));
            }
           
        }
    }

    [Command]
    public void CmdDealDamage(GameObject obj, Vector3 pos, Vector3 rotation)
    {
        obj.GetComponent<PlayerHealth>().TakeDamage(5);

        Instantiate(bloodImpact, pos, Quaternion.LookRotation(rotation));
    }

}

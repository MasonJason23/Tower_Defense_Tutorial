using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void purchaseStandardTurret()
    {
        Debug.Log("Standard Turret Purchased!");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }
    
    // public void purchaseAnotherTurret()
    // {
    //     Debug.Log("Another Turret Purchased!");
    // }
}

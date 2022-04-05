using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    
    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Purchased!");
        buildManager.SelectTurretToBuild(standardTurret);
    }
    
    // public void purchaseAnotherTurret()
    // {
    //     Debug.Log("Another Turret Purchased!");
    // }
}

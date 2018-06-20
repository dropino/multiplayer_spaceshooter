﻿using UnityEngine;
using System.Collections;

public class PowerupHandler : MonoBehaviour
{
	[SerializeField]
    private GameObject _shieldPrefab;

	//bools for powerup activation tracking
    private bool _speedBoost = false;
    private bool _shields = false;
    private bool _enhancedShot = false;

	private ShootHandler _shootHandler;
	private Ship ship;
	private string shipName;

	public bool SpeedBoostStatus
    {
        get { return this._speedBoost; }
        set { this._speedBoost = value; }
    }

    public bool ShieldsStatus
    {
        get { return this._shields; }
        set { this._shields = value; }
    }

    public bool EnhancedShotStatus
    {
        get { return this._enhancedShot; }
        set { this._enhancedShot = value; }
    }

    // Use this for initialization
    void Start()
    {
		_shootHandler = GetComponentInParent<ShootHandler>();
		ship = GetComponentInParent<Ship>();
		shipName = ship.name;
    }



	// ****   PowerUps handlers   **** \\
    //these public classes will be directly called by the respective PoweUps once they collide with a ship
    public void EnhancedShotOn(float shotDamage, float shotSpeed)
	{
		if (!EnhancedShotStatus)
        {
            EnhancedShotStatus = true;
            //changes the attributes of Shooting. 
            //Power down handled by Shooting after one EnhancedShot is shot setting false EnhancedShotStatus.
			_shootHandler.EnhancedShotDamage = shotDamage;
			_shootHandler.EnhancedShotSpeed = shotSpeed;
            
        }
	}

    public void ShieldsOn()
	{
		if (!ShieldsStatus) ActivateShield();
	}

	public void SpeedBoostOn(float speedMultiplyer)
	{
		if (!SpeedBoostStatus)
        {
            SpeedBoostStatus = true;
            // Changes the speed at which the ship moves linearly
			ship.MaxSpeed *= speedMultiplyer;
			ship.RotSpeed *= speedMultiplyer;

            //start the automatic power down coroutine
            StartCoroutine(this.SpeedBoostPowerDown());
        }
	}

    //haven't reset maxspeed in ship yet!!
	protected IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(3.0f);
        SpeedBoostStatus = false;

		switch (name)
		{
			case "StrongShip_Prefab":
				ship.MaxSpeed = StrongShip.MAX_SPEED;
				ship.RotSpeed = StrongShip.ROT_SPEED;
				break;
			case "FastShip_Prefab":
				ship.MaxSpeed = FastShip.MAX_SPEED;
                ship.RotSpeed = FastShip.ROT_SPEED;
                break;
			case "ResistantShip_Prefab":
                ship.MaxSpeed = ResistantShip.MAX_SPEED;
                ship.RotSpeed = ResistantShip.ROT_SPEED;
                break;
			default:
				Debug.Log("Shouldn't get here");
				break;
		}
        
    }
    
	private void ActivateShield()
    {
        _shieldPrefab.SetActive(true);
    }

    private void DeactivateShield()
    {
        _shieldPrefab.SetActive(false);
    }
}


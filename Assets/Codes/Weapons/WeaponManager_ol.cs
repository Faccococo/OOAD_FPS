using Codes.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponManager_ol : NetworkBehaviour
{
    public Gun_ol Main_Weapon;
    public Gun_ol Secaondary_Weapon;
    
    private Gun_ol carriedWeapon;

    public override void OnStartLocalPlayer()
    {
        carriedWeapon = Main_Weapon;
    }

    private void Update()
    {
        carriedWeapon.updateWeaponState();
        SwapWeapon();
    }

    public Gun_ol getCarriedWeapon()
    {
        return carriedWeapon;
    }
    private void SwapWeapon()
    {
        if (carriedWeapon.isReloading())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DeActivateCarriedWeapon();
            carriedWeapon = Main_Weapon;
            ActivateCarriedWeapon();
            carriedWeapon.playStartSound();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DeActivateCarriedWeapon();
            carriedWeapon = Secaondary_Weapon;
            ActivateCarriedWeapon();
            carriedWeapon.playStartSound();
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            DeActivateCarriedWeapon();
            changeWeapon();
            ActivateCarriedWeapon();
            carriedWeapon.playStartSound();
        }
    }

    private void changeWeapon()
    {
        if (carriedWeapon == Main_Weapon) carriedWeapon = Secaondary_Weapon;
        else carriedWeapon = Main_Weapon;
    }

    private void DeActivateCarriedWeapon()
    {
        carriedWeapon.gameObject.SetActive(false);
        carriedWeapon.GunIcon.SetActive(false);
    }
    private void ActivateCarriedWeapon()
    {
        carriedWeapon.gameObject.SetActive(true);
        carriedWeapon.GunIcon.SetActive(true);
    }
}

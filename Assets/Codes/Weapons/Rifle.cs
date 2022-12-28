using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Codes.Weapon
{
    public class Rifle : Gun
    {

        public Text Current_Bullet_In_Mag_UI;
        public Text Current_Max_Bullet_UI;
        public Text Weapon_Name;



        protected override void Start()
        {
            base.Start();
            OriginCameraField = Gun_Camera.fieldOfView;
        }

        public override void attack()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Fire();
            }
        }


        protected override void PlayFireAnimation()
        {
            if (Aiming)
            {
                GunAnimator.Play("Aim Fire", 0, 0);
            }
            else
            {
                GunAnimator.Play("Fire", 0, 0);
            }
        }



        protected override bool isReloading()
        {
            //两种换子弹动画
            if (info.IsName("Reload Out Of Ammo") || info.IsName("Reload Ammo Left"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void DoReloadAnimation()
        {
            if (Current_Bullet_In_Mag > 0)
            {
                GunAnimator.Play("Reload Ammo Left", 0, 0);
            }
            if (Current_Bullet_In_Mag == 0)
            {
                GunAnimator.Play("Reload Out Of Ammo", 0, 0);
            }
        }

        protected override void CheckWeapon()
        {
            GunAnimator.Play("Inspect", 0, 0);
        }

        protected override bool isChecking()
        {
            if (info.IsName("Inspect")) return true;
            else return false;
        }

        protected override void UpdateUI()
        {
            Current_Bullet_In_Mag_UI.text = Current_Bullet_In_Mag.ToString();
            Current_Max_Bullet_UI.text = Current_Max_Bullet.ToString();
            Weapon_Name.text = "Rifle";
        }

        protected override void UpdateAimState()
        {
            Gun_Camera.fieldOfView = Aiming ? 20 : OriginCameraField;
            GunAnimator.SetBool("Aim", Aiming);
        }
    }
}
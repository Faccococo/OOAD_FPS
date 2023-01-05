using System.Diagnostics;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

namespace Codes.Weapon
{
    public class HandGun : Gun
    {

        public Text Current_Bullet_In_Mag_UI;
        public Text Current_Max_Bullet_UI;
        public Text Weapon_Name;

        public override void attack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
            }
            if (MuzzleParticle.time > 0.1f)
            {
                MuzzleParticle.Stop();
            }
        }

        protected override void UpdateUI()
        {
            Current_Bullet_In_Mag_UI.text = Current_Bullet_In_Mag.ToString();
            Current_Max_Bullet_UI.text = Current_Max_Bullet.ToString();
            Weapon_Name.text = "Hand Gun";
        }
    }
}
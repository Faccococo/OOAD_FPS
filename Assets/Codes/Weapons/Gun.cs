using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace Codes.Weapon
{
    public abstract class Gun : MonoBehaviour, Weapon
    {
        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public GameObject Hit_Particial;
        public GameObject Bullet_Hole;

        public int Mag_Capacity;
        public int Total_Bullet;
        public int Fire_Range;
        public float Fire_Rate;
        public PlayerMovement PM;
        public Camera Gun_Camera;
        public MouseLookAt mouseLook;
        public AnimationCurve Spread_Curve;//子弹散射曲线
        public float Spread;// 用于散射效果的减弱
        public GameObject GunIcon;
        public GameObject CrossUI;


        protected float currentSpreadTime; // 当前后坐力曲线曲线时间
        protected float SpreadAngel; //当前散射大小
        protected float Last_Fire_Time;
        protected int Current_Bullet_In_Mag;
        protected int Current_Max_Bullet;
        protected bool Aiming;
        protected float OriginCameraField;

        protected Animator GunAnimator;
        protected AnimatorStateInfo info;


        protected virtual void Start()
        {
            SpreadAngel = 0f;
            Current_Bullet_In_Mag = Mag_Capacity;
            Current_Max_Bullet = Total_Bullet;
            GunAnimator = GetComponent<Animator>();
            info = GunAnimator.GetCurrentAnimatorStateInfo(0);
            UpdateUI();
        }

        protected bool isAllowedShooting()
        {
            return Time.time - Last_Fire_Time > 1 / Fire_Rate && !PM.IsRuning && !isReloading();
        }

        protected bool isAllowedAiming()
        {
            return !PM.IsRuning && !isReloading() && !isChecking();
        }

        protected void hitOnObjects(RaycastHit hit)
        {
            GameObject hitParticleEffect = Instantiate(Hit_Particial, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //创造火光特效
            GameObject bulletHoleEffect = Instantiate(Bullet_Hole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(bulletHoleEffect, 3f);
            Destroy(hitParticleEffect, 1f);
        }

        protected void doShoot()
        {
            Vector3 spread = Random.insideUnitCircle * SpreadAngel * Gun_Camera.fieldOfView * 0.005f;
            if (isAiming()) spread = spread * 0.1f;
            RaycastHit hit;
            Vector3 shootDirection = MuzzlePoint.forward + spread;
            UnityEngine.Debug.Log(spread.x + " " + spread.y);
            if (Physics.Raycast(MuzzlePoint.position, shootDirection, out hit, Fire_Range))
            {
                hitOnObjects(hit);
            }
        }

        protected void Fire()
        {
            if (!isAllowedShooting() || Current_Bullet_In_Mag <= 0)
            {
                MuzzleParticle.Stop();
                currentSpreadTime = 0;
                return;
            }
            Current_Bullet_In_Mag -= 1;
            PlayFireAnimation();
            Last_Fire_Time = Time.time;
            MuzzleParticle.Play();
            doShoot();
            mouseLook.FiringForTest();
            updateRecoilVal();
        }

        protected void Reload()
        {
            if (Current_Max_Bullet <= 0) return;
            DoReloadAnimation();
            Current_Bullet_In_Mag = Mag_Capacity;
            Current_Max_Bullet -= Mag_Capacity;
        }

        protected void updateRecoilVal()
        {
            currentSpreadTime += Time.deltaTime;
            float recoilFraction = currentSpreadTime * Spread * 0.1f;
            SpreadAngel = Spread_Curve.Evaluate(recoilFraction);
        }

        public bool isAiming()
        {
            return Aiming;
        }

        public void updateWeaponState()
        {
            Aiming = false;
            info = GunAnimator.GetCurrentAnimatorStateInfo(0);
            if (Input.GetKey(KeyCode.Mouse1) && isAllowedAiming())
            {
                Aiming = true;
            }
            UpdateAimState();
            attack();
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                CheckWeapon();
            }
            UpdateUI();
            if (isAiming())
            {
                CrossUI.SetActive(false);
            }
            else
            {
                CrossUI.SetActive(true);    
            }
        }
        public abstract void attack();
        protected abstract bool isReloading();
        protected abstract void PlayFireAnimation();
        protected abstract void DoReloadAnimation();
        protected abstract void UpdateAimState();
        protected abstract void CheckWeapon();
        protected abstract bool isChecking();
        protected abstract void UpdateUI();
    }
}


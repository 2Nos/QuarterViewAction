//====================250302
//�����丵

//==========================

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using static DUS.WeaponInfo;

namespace DUS
{
    public class WeaponInfo : MonoBehaviour
    {
        public enum WeaponType { Melee, Range };
        public WeaponType m_weaponType;
        public BulletPoolManager m_BulletPoolManager;
        public int m_damage;
        public float m_rate;

        [Header("[Melee]")]
        public MeleeWeaponInfo m_meleeWeaponInfo;

        public RangeWeaponInfo m_rangeWeaponInfo;

        UIManager m_UIManager;
        private void Awake()
        {
            m_UIManager = FindObjectOfType<UIManager>(); ;
        }
        public bool IsCalculatePickUpAmmo(int filedAmmo)
        {
            if (this.m_rangeWeaponInfo.currentAmmo >= this.m_rangeWeaponInfo.maxMagazineAmmo) return false;
            
            int totalAmmo = this.m_rangeWeaponInfo.magazineAmmo + filedAmmo;

            if (totalAmmo < this.m_rangeWeaponInfo.maxMagazineAmmo)
            {
                this.m_rangeWeaponInfo.magazineAmmo += filedAmmo;
            }
            else if (totalAmmo >= this.m_rangeWeaponInfo.maxMagazineAmmo)
            {
                this.m_rangeWeaponInfo.magazineAmmo = this.m_rangeWeaponInfo.maxMagazineAmmo;
            }

            //�������� ���� ���� ���� TMP��ȭ
            if(this.gameObject.activeSelf) m_UIManager.ChangeAmmoTMP(this.m_rangeWeaponInfo.currentAmmo, this.m_rangeWeaponInfo.maxLoadAmmo, this.m_rangeWeaponInfo.magazineAmmo, this.m_rangeWeaponInfo.maxMagazineAmmo);
            return true;
        }

        public bool ReloadAmmo()
        {
            //1. ������ �����
            if (this.m_rangeWeaponInfo.magazineAmmo == 0) return false;
            if (this.m_rangeWeaponInfo.currentAmmo >= this.m_rangeWeaponInfo.maxLoadAmmo) return false;

            //2. ������ �Ѿ� ���� Ȯ��
            int needAmmo = this.m_rangeWeaponInfo.maxLoadAmmo - this.m_rangeWeaponInfo.currentAmmo;

            //3. ���� źâ�� �Ѿ� ���� Ȯ��
            int remainderMagazine = 0;
            if (this.m_rangeWeaponInfo.magazineAmmo <= needAmmo)
            {
                this.m_rangeWeaponInfo.currentAmmo += this.m_rangeWeaponInfo.magazineAmmo;
                remainderMagazine = 0;
            }
            else if(this.m_rangeWeaponInfo.magazineAmmo > needAmmo)
            {
                //
                this.m_rangeWeaponInfo.currentAmmo += needAmmo;
                remainderMagazine = this.m_rangeWeaponInfo.magazineAmmo - needAmmo;
            }
            this.m_rangeWeaponInfo.magazineAmmo = remainderMagazine;
            return true;
        }

        public void MeleeAttack()
        {

        }

        public void Shoot()
        {
            if (this.m_rangeWeaponInfo.currentAmmo <= 0) return;
            --this.m_rangeWeaponInfo.currentAmmo;
            m_UIManager.ChangeAmmoTMP(this.m_rangeWeaponInfo.currentAmmo,this.m_rangeWeaponInfo.maxLoadAmmo, this.m_rangeWeaponInfo.magazineAmmo, this.m_rangeWeaponInfo.maxMagazineAmmo);
            GameObject bullet = m_BulletPoolManager.GetBullet();
            bullet.transform.position = m_rangeWeaponInfo.bulletPos.position;
            bullet.transform.rotation = m_rangeWeaponInfo.bulletPos.rotation;
        }
    }
}
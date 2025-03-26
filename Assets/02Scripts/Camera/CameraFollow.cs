//====================240811
//ù ����
//ī�޶� �÷��̾� ���󰡵���

//==================================================

//********** Ÿ�� ������ ��� Ŭ���� **********
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DUS
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        Vector3 offsetDis;
        [SerializeField]
        Vector3 offsetRot;
        private void LateUpdate()
        {
            transform.position = target.position + offsetDis;
            transform.rotation = Quaternion.Euler(offsetRot);
        }
    }
}
//====================250303
//�׷쿡�� ó���� �����ϰ� �ش� ��� ���x

//====================240824
// �÷��̾ ���� ȸ��
// �÷��̾ �ְ� �׳� ȸ�� �� �÷��̾��� ȸ���� ���� �� ���� ȸ���� �ع����⿡ �̻�����
// ȸ���� �����ս��� �� �ֱ����� �÷��̾��� ȸ���� ������� ���۵ǵ���

//==================================================

//********** ����ź ȸ�� �˵� **********
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUS
{
    public class Orbit : MonoBehaviour
    {
        public Transform target;
        public float orbitSpeed;
        Vector3 m_offset;

        void Start()
        {
            m_offset = transform.position - target.position; //�÷��̾���� �Ÿ�
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = target.position + m_offset;
            transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
            m_offset = transform.position - target.position;
        }
    }
}
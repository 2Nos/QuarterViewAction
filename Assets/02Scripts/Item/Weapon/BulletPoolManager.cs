// �Ѿ� �߻縦 ���� �� �ѹ߸�x ���� �����
// �Ѿ��� ��򰡿� �ε����ų� �ð��������� ������ �ؾ��� ��Ȳ�� ���� ��
// C#������ �ʿ���� �޸� �˾Ƽ� ������ �÷��Ͱ� ������
// ������ �÷��Ͱ� ����� ��δ�
// Destory �� �Լ� ����� �߻�
// ������Ʈ Ǯ���� �̹� �� �Ѿ��� ��Ȱ���ϴ� ���

using UnityEngine;
using System.Collections.Generic;

namespace DUS
{
    public class BulletPoolManager : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform bullePoolParent;
        public int poolSize;

        private Queue<GameObject> bulletPool = new Queue<GameObject>();

        private void Awake()
        {
            // �̸� �Ѿ� ����
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(this.bulletPrefab, bullePoolParent);
                bullet.SetActive(false);
                bulletPool.Enqueue(bullet);
            }
        }

        public GameObject GetBullet()
        {
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool.Dequeue(); //��� ������ ���� �� ť������ ����
                bullet.SetActive(true);
                return bullet;
            }
            else
            {
                // Ǯ�� ���� �Ѿ��� ������ �߰� ���� (�ʿ� ��)
                GameObject bullet = Instantiate(this.bulletPrefab, bullePoolParent);
                bullet.SetActive(false);
                bulletPool.Enqueue(bullet); // ���� ť
                return GetBullet(); //����?���?
            }
        }

        public void ReturnBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }
}

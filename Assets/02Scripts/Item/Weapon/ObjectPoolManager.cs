using UnityEngine;
using UnityEngine.Pool;


namespace DUS
{
    public class ObjectPoolManager : MonoBehaviour
    {
        ObjectPool<GameObject> pool;
        const int maxSize = 100;
        const int initSize = 20;

        //CreateFunc: ������Ʈ ���� �Լ�(Func), actionOnGet: Ǯ���� ������Ʈ�� �������� �Լ�(Action)
        //actionOnRelease: ������Ʈ�� ��Ȱ��ȭ�� �� ȣ��(Action), actionOnDestory: ������Ʈ �ı��Լ�(Action)
        //collectionCheck: �ߺ� ��ȯ üũ(bool), defaultCapacity: ó���� �̸� �����ϴ� ������Ʈ ����(int)
        //maxSize: ������ ������Ʈ�� �ִ� ����(int)
        /*private void Awake()
        {
            pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize); ;
        }
        private GameObject CreateObject() // ������Ʈ ����
        {
            return Instantiate(prefab);
        }

        private void ActivatePoolObject(GameObject obj) // ������Ʈ Ȱ��ȭ
        {
            obj.SetActive(true);
        }

        private void DisablePoolObject(GameObject obj) // ������Ʈ ��Ȱ��ȭ
        {
            obj.SetActive(false);
        }

        private void DestroyPoolObject(GameObject obj) // ������Ʈ ����
        {
            Destroy(obj);
        }

        public GameObject GetObject()
        {
            GameObject sel = null;

            if (pool.CountActive >= maxSize) // maxSize�� �Ѵ´ٸ� �ӽ� ��ü ���� �� ��ȯ
            {
                sel = CreateObject();
                sel.tag = "PoolOverObj";
            }
            else
            {
                sel = pool.Get();
            }

            return sel;
        }

        public void ReleaseObject(GameObject obj)
        {
            if (obj.CompareTag("PoolOverObj"))
            {
                Destroy(obj);
            }
            else
            {
                pool.Release(obj);
            }
        }*/
    }
}
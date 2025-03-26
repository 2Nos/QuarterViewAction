//====================250316
//���� ���� ��� �߰�

//====================250301
//���� ������ ���� ����

//====================250218
// �����丵 �۾�
// ���� ��Ƽ ��� �ֱ�

//====================240928 
// ���� ��� ����
// ����ź ��ô

//====================240824
// ����� ������ �з�
// ����ź ����

//====================240816 ~ 17
// ������ ����
// �ʵ� ������ �浹 ó�� �� ȹ��
// ���� ������ ���� (Equip ����)

//====================240811
// ù ����
// Movement���� ��� ����

//==================================================
// ���� �۾��ѳ��� ~�� ����


//********** �÷��̾� ���� ���� Ŭ���� **********
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS
{
    public enum PlayerState
    {
        Move,
        Jump,
        Dodge,
        PickUpWeapon,
        SwapWeapon,
        Attack,
        Reload,
        Grenade,
        Die,
        Shopping
    }
    public class PlayerLocomotion : MonoBehaviour
    {
        /*#region Singleton
        private static PlayerLocomotion instance;
        public static PlayerLocomotion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PlayerLocomotion>();
                    if (instance == null)
                    {
                        //GameObject singletonObj = new GameObject(typeof(PlayerLocomotion).Name);
                        GameObject singletonObj = new GameObject("Player");
                        instance = singletonObj.AddComponent<PlayerLocomotion>();
                    }
                }
                return instance;
            }
        }
        #endregion*/

        //===== RefClass
        public PlayerAnimationManager m_playerAnimationManager { get; private set; }
        public PlayerHasWeapon m_playerHasWeapon { get; private set; }
        public UIManager m_UIManager;
        //public PlayerAudioManager m_PlayerAudioManager;
        public GameManager m_GameManager;

        public PlayerInputHandler m_playerInputHandler;
        

        //===== State
        [HideInInspector]
        public PlayerState m_playerState { get; private set; }

        //===== Setting
        //[FormerlySerializedAs("runSpeed")] //������ ���� �� �ش� ���� ������� �ʵ����ϴ� ���
        [SerializeField]
        Camera m_followCamera;

        [Header("[ Player Movement Value]")]
        [SerializeField]
        float m_runSpeed;
        [SerializeField]
        float m_walkSpeed;
        [Tooltip("���� ȸ��"), SerializeField]
        float m_jumpUpForce;
        [SerializeField]
        float m_jumpForwardForce;

        [Tooltip("���� ȸ��"), SerializeField]
        float m_dodgeForwardForce;
        float m_moveSpeed;

        //===== Item ����
        [Header("[Grenade]")]
        public EffectGrenadeManager m_effectGrenadeManager;
        public GameObject m_GrenadePrefab;

        [Header("[Coin]")]
        public int m_Coin;

        [Header(" [Status] ")]
        public int m_CurrentHealth;
        public int m_MaxHealth;
        public MeshRenderer[] m_MeshRenders;

        //===== Movement ����
        Vector3 m_moveDir;
        Rigidbody m_playerRigid;

        bool m_isGrounded; //������ Ʈ�����Լ� ������ m_isStatusProgressing ��ſ� ���� �� üũ
        bool m_isStatusProgressing;
        bool m_isDamage;
        Coroutine m_dialogCoroutine;
        /*private void SingletonInitialized()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }*/

        private void Awake()
        {
            //SingletonInitialized();

            //m_playerInputHandler = PlayerInputHandler.Instance;

            m_playerRigid = GetComponent<Rigidbody>();
            m_playerAnimationManager = GetComponentInChildren<PlayerAnimationManager>();
            m_playerHasWeapon = GetComponent<PlayerHasWeapon>();
            
            ChangeState(PlayerState.Move);

            m_MeshRenders = GetComponentsInChildren<MeshRenderer>();

            PlayerPrefs.SetInt("MaxScore", 1025);
        }

        private void Start()
        {
            m_CurrentHealth = m_MaxHealth;
            m_UIManager.ChangeHeart(m_CurrentHealth, m_MaxHealth);
            m_Coin = 4000;
            m_UIManager.ChangeCoinTMP(m_Coin);
        }

        void StopToWall()
        {
            //Debug.DrawRay(transform.position, transform.forward * 5);
            //m_isBoard = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
        }

        private void FixedUpdate()
        {
            //StopToWall();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                ChangeState(PlayerState.Die);
            }
            UpdateState();
        }
        #region ============================================================================================================================== StatusMagement
        private void UpdateState()
        {
            if (m_isStatusProgressing) return;
            //FSM(���ѻ��¸ӽ�) ������� ���� ����(���¸ӽ� ����� ������ �ֱ�)
            switch (m_playerState)
            {
                case PlayerState.Move:
                    Move();
                    Rotate();
                    break;
                case PlayerState.Jump:
                    Jump();
                    break;
                case PlayerState.Dodge:
                    Dodge();
                    break;
                case PlayerState.PickUpWeapon:
                    PickUpWeapon();
                    break;
                case PlayerState.SwapWeapon:
                    break;
                case PlayerState.Attack:
                    Attack();
                    break;
                case PlayerState.Reload:
                    Reload();
                    break;
                case PlayerState.Grenade:
                    Grenade();
                    break;
                case PlayerState.Die:
                    Die();
                    break;
                case PlayerState.Shopping:

                    break;
            }
        }
        private void ChangeState(PlayerState playerState)
        {
            m_playerState = playerState;
        }

        private void FSMManagement()
        {
            if(m_isStatusProgressing) return;

            //m_isStatusProgressing�� false ������ ��, �� �ٸ� ������ ���� �� ��ü
            else if(m_playerInputHandler.m_IsJumpKey) ChangeState(PlayerState.Jump);
            else if(m_playerInputHandler.m_IsDodgeKey) ChangeState(PlayerState.Dodge);
            else if(m_playerInputHandler.m_IsPickUpWeaponKey) ChangeState(PlayerState.PickUpWeapon);
            else if(m_playerInputHandler.m_IsAttackKey) ChangeState(PlayerState.Attack);
            else if(m_playerInputHandler.m_IsSwapKey) ChangeState(PlayerState.SwapWeapon);
            else if(m_playerInputHandler.m_isReloadKey) ChangeState(PlayerState.Reload);
            else if (m_playerInputHandler.m_isGrenadeKey) ChangeState(PlayerState.Grenade);
        }
        #endregion ==============================================================================================================================/StatusMagement

        #region ============================================================================================================================== Movement
        /// <summary>
        /// �׻� ��� ������ ��������� �� �������̰� �̷�������� ����
        /// </summary>
        private void Move()
        {
            m_moveDir = new Vector3(m_playerInputHandler.m_InputMoveVec.x, 0, m_playerInputHandler.m_InputMoveVec.y);
            if (m_moveDir.sqrMagnitude > 1) m_moveDir.Normalize(); //Ȯ�� ��� ��ֶ�����Ǽ� ���µ� (�밢���� ���ؼ�)

            m_moveSpeed = m_playerInputHandler.m_IsWalkKey ? m_walkSpeed : m_runSpeed;
            transform.Translate(m_moveDir * m_moveSpeed * Time.deltaTime, Space.World);

            m_playerAnimationManager.MoveAni(m_moveDir == Vector3.zero ? false : true, m_playerInputHandler.m_IsWalkKey);

            FSMManagement();

            //====================�ֿ� �м�====================
            //position�� �̵��� Translate�� �̵��� ���̿� ���� ����(��ġ�� �̵�, ����� �̵�)
            //rigid �̵��������� ���� ������ FixedUpdate �� ���� ������ CPU �������⿡ ����ȭ�� ���� ����
            //�ٸ� ��ĵ� ������ �ϴ��� �����ϰԸ�
            //transform.Translate(m_moveDir * m_moveSpeed * Time.deltaTime, Space.World);
            //transform.position += moveDir * moveSpeed * Time.deltaTime; //���� ���
            //rigid.MovePosition(transform.position + (moveDir * moveSpeed * Time.deltaTime));*/
            //=================================================
        }

        private void Rotate()
        {
            // ���콺 ȸ���� ���� ĳ���� ȸ���� �ƴ� Ű�θ� ȸ�� �����ϰ�
            // ������ ��ȹ�浹�� ����
            transform.LookAt(transform.position + m_moveDir);

            #region=============================================250219 ���콺 ȸ���� ���� ĳ���� ȸ�� ��ȹ�� ���� �ʴ� �� �;� �ּ�ó��
            //if (m_isAttackking) return;
            // 1. Ű���忡 ���� ȸ��

            /*if (m_equipWeaponInfo.weaponType == WeaponInfo.WeaponType.Melee && m_isAttackking) return;
            transform.LookAt(transform.position + m_moveDir);
            
            // 2. ���콺�� ���� ȸ��
            Ray ray = m_followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (m_equipWeaponInfo.weaponType == WeaponInfo.WeaponType.Melee) return;
            if (!m_isAttackKey) return;

            //out �����Լ���� �����ϸ�� Hit�� �Ǿ��� �� rayHit�� Hit������ ��ڴ�
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }*/
            #endregion=============================================/250219 ���콺 ȸ���� ���� ĳ���� ȸ�� �ּ�ó��
        }

        private void Jump()
        {
            if (m_isGrounded) return;
            //m_PlayerAudioManager.PlayAudio(1);
            m_isGrounded = true;
            m_isStatusProgressing = true;

            m_playerRigid.AddForce(Vector3.up * m_jumpUpForce, ForceMode.Impulse);
            m_playerRigid.AddForce(transform.forward * m_jumpForwardForce, ForceMode.Impulse);

            m_playerAnimationManager.JumpAni(m_isStatusProgressing);
        }

        private void Dodge()
        {
            m_playerRigid.linearVelocity = transform.forward * m_dodgeForwardForce;
            m_playerAnimationManager.DodgeAni(m_isStatusProgressing);
            InvokeStatus("OutDodge", 1.0f);
        }

        private void OutDodge()
        {
            m_playerRigid.linearVelocity = Vector3.zero;
            OutStatus();
        }
        //������ ���ͷ���
        private void Die()
        {
            m_GameManager.GameOver();
            m_playerAnimationManager.Die();
            m_isStatusProgressing = true;
            StopAllCoroutines();
        }
        IEnumerator OnDamage(int damage, Bullet bullet)
        {
            m_isDamage = true;
            foreach (MeshRenderer mesh in m_MeshRenders)
            {
                mesh.material.color = Color.yellow;
            }
            m_CurrentHealth -= damage;

            if (m_CurrentHealth <= 0)
            {
                m_CurrentHealth = 0;
                ChangeState(PlayerState.Die);
            }
            if (bullet != null && bullet.attacker == AttacType.BossTaunt) m_playerRigid.AddForce(transform.forward * -25, ForceMode.Impulse);
            yield return new WaitForSeconds(0.3f);
            if (bullet != null && bullet.attacker == AttacType.BossTaunt) m_playerRigid.linearVelocity = Vector3.zero;
            foreach (MeshRenderer mesh in m_MeshRenders)
            {
                mesh.material.color = Color.white;
            }
            m_UIManager.ChangeHeart(m_CurrentHealth, m_MaxHealth);
            m_isDamage = false;
        }
        #endregion =========================================================================================================================== /Movement
        #region =========================== PublicStatusFunction
        private bool CheckStatusAndReturnToMoveStatus(bool isCheck)
        {
            if (isCheck == false)
            {
                ChangeState(PlayerState.Move);
                return true;
            }
            return false;
        }
        private void InvokeStatus(string invokeName, float time)
        {
            m_isStatusProgressing = true;
            Invoke(invokeName, time);
        }
        public void OutStatus()
        {
            m_isStatusProgressing = false;
            ChangeState(PlayerState.Move);
        }
        #endregion =========================== /PublicStatusFunction

        #region ============================================================================================================================== Item Interaction
        #region ============================== FieldItem
        // PlayerHasWepon���� ó��
        private void PickUpWeapon()
        {
            if(CheckStatusAndReturnToMoveStatus(m_playerHasWeapon.PickUpWeapon())) return;

            InvokeStatus("OutStatus", 0.3f);
        }

        /// <summary>
        /// ������ �ڵ����� �Ծ����� �����۵�
        /// </summary>
        /// <param name="other"></param>
        private void PickUpFieldItem(GameObject other)
        {
            Item item = other.GetComponent<Item>();
            switch (item.itemInfo.itemType)
            {
                //źâ �Ծ��� ���
                case ItemType.Ammo:
                    if (PickUpAmmo(item.itemInfo.itemNum) == false) return;
                    break;
                case ItemType.Coin:
                    m_Coin += item.itemInfo.itemNum;
                    m_UIManager.ChangeCoinTMP(m_Coin);
                    break;
                case ItemType.Grenade:
                    if (!m_effectGrenadeManager.OnEffectGrenade())
                    {
                        m_UIManager.HasWeaponUI(this);
                        return;
                    }
                    break;
                case ItemType.Heart:
                    m_CurrentHealth += item.itemInfo.itemNum;
                    if (m_CurrentHealth > m_MaxHealth)
                    {
                        m_CurrentHealth = m_MaxHealth;
                    }
                    m_UIManager.ChangeHeart(m_CurrentHealth, m_MaxHealth);
                    break;
            }
            Destroy(other.gameObject);
        }
        private bool PickUpAmmo(int itemNum)
        {
            if (!m_playerHasWeapon.IsSupplyAmmo(itemNum)) return false;
            return true;
        }
        #endregion =========================== /FieldItem

        #region ============================== Weapon

        /// <summary>
        /// PlayerHasWepon���� ó��
        /// </summary>
        /// <param name="swapKeyNum"></param>
        public void SwapWeapon(int swapKeyNum)
        {
            if(CheckStatusAndReturnToMoveStatus(m_playerHasWeapon.SwapWeapon(swapKeyNum))) return;

            InvokeStatus("OutStatus", 0.5f);
        }

        public void Attack()
        {
            //1. ������ �����ΰ��� üũ �� True�̸� IsAttack()���� ���� �̷����
            if(CheckStatusAndReturnToMoveStatus(m_playerHasWeapon.IsAttack())) return;
            

            //2. �ִϸ��̼� ���ð� �� ���󺹱�
            InvokeStatus("OutStatus", m_playerHasWeapon.m_WeaponInfos[m_playerHasWeapon.m_currentIndexNum].m_rate);
        }
        private void Reload()
        {
            if (CheckStatusAndReturnToMoveStatus(m_playerHasWeapon.IsReload())) return;

            m_playerAnimationManager.ReloadAni();
            InvokeStatus("OutStatus", 3);
        }

        //���� ���ݰ� ������ ����
        private void Grenade()
        {
            if (CheckStatusAndReturnToMoveStatus(m_effectGrenadeManager.OffEffectGrenade())) return;
            Ray _ray = m_followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit _rayHit;

            if (Physics.Raycast(_ray, out _rayHit, 100))
            {
                Vector3 _nextVec = _rayHit.point - transform.position;
                _nextVec.y = 0;
                GameObject _instantGrenade = Instantiate(m_GrenadePrefab, transform.position, transform.rotation);
                Rigidbody _rigidGrenade = _instantGrenade.GetComponent<Rigidbody>();
                _rigidGrenade.AddForce(_nextVec, ForceMode.Impulse);
                _rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
            }
            m_playerAnimationManager.ThrowAni();
            InvokeStatus("OutStatus", 0.8f);
        }
        #endregion =========================== /Weapon
        #endregion ==============================================================================================================================/ Item Interaction

        #region ============================================================================================================================== Shop Interaction
        private void EnterShop(GameObject other)
        {
            m_isStatusProgressing = true;
            ChangeState(PlayerState.Shopping);
            Shop shop = other.GetComponentInParent<Shop>();
            shop.Enter(this);
        }
        public void ExitShop(GameObject other)
        {
            Shop shop = other.GetComponentInParent<Shop>();
            shop.Exit();
        }
        #endregion =========================================================================================================================== /Shop Interaction
        #region ============================================================================================================================== Detection
        //Collision �浹, Trigger ���
        //Item �ݶ��̴� �ΰ� ������ �ִ� ����(���� �ݶ��̴� : Floor�� ����, ū �ݶ��̴� : �÷��̾� Ʈ���� ����)
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Floor"))
            {
                if (m_isGrounded)
                {
                    m_isStatusProgressing = false;
                    m_isGrounded = false;
                    ChangeState(PlayerState.Move);
                    m_playerAnimationManager.JumpAni(m_isStatusProgressing);
                }
            }
        }
        IEnumerator PickupDialog(string talk)
        {
            
            m_UIManager.ShowDiaLogTMP(true, talk);
            yield return new WaitForSeconds(2f);
            m_UIManager.ShowDiaLogTMP(false, "");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                PickUpFieldItem(other.gameObject);
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                if (m_isDamage || m_playerState == PlayerState.Die) return;
                Bullet bullet = other.GetComponent<Bullet>();

                /*if (m_dialogCoroutine != null)
                {
                    StopCoroutine(OnDamage(bullet.m_damage, bullet));
                    m_dialogCoroutine = null;
                }
                m_dialogCoroutine = */
                    StartCoroutine(OnDamage(bullet.m_damage, bullet));
            }
            else if(other.CompareTag("EnemyMeleeAttack"))
            {
                if (m_isDamage || m_playerState == PlayerState.Die) return;
                Enemy enemy = other.GetComponentInParent<Enemy>();
                /*if (m_dialogCoroutine != null)
                {
                    StopCoroutine(OnDamage(enemy.m_damage, null));
                    m_dialogCoroutine = null;
                }
                m_dialogCoroutine = */
                    StartCoroutine(OnDamage(enemy.m_damage, null));
            }


            if (other.CompareTag("Weapon"))
            {
                if (m_dialogCoroutine != null)
                {
                    StopCoroutine(m_dialogCoroutine);
                    m_dialogCoroutine = null;
                }
                m_dialogCoroutine = StartCoroutine(PickupDialog("F Ű�� ��������"));
            }

            if (other.CompareTag("Shop"))
            {
                if (m_dialogCoroutine != null)
                {
                    StopCoroutine(m_dialogCoroutine);
                    m_dialogCoroutine = null;
                }
                m_dialogCoroutine = StartCoroutine(PickupDialog("Z Ű�� ��������"));

            }
        }
        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Shop"))
            {
                if (m_playerInputHandler.m_isShoppingKey)
                {
                    EnterShop(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Shop"))
            {
                ExitShop(other.gameObject);
                if (m_dialogCoroutine != null)
                {
                    StopCoroutine(m_dialogCoroutine);
                    m_dialogCoroutine = null;
                }
                m_UIManager.ShowDiaLogTMP(false,"");
            }
            if (other.CompareTag("Weapon"))
            {
                if (m_dialogCoroutine != null)
                {
                    StopCoroutine(m_dialogCoroutine);
                    m_dialogCoroutine = null;
                }
                m_UIManager.ShowDiaLogTMP(false, "");
            }

        }

        #endregion =========================================================================================================================== /Detection
    }
}
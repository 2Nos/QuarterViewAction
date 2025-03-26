//===========================250301
//�÷��̾��� ���¸ӽ� �߻�Ŭ����

//===========================
using DUS;
using UnityEngine;

public abstract class PlayerStateMachine
{
    public abstract void EnterState(PlayerLocomotion playerLocomotion);
    public abstract void UpdateState(PlayerLocomotion playerLocomotion);
    public abstract void ExitState();
}
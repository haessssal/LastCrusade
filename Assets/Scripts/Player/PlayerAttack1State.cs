using UnityEngine;

public class PlayerAttack1State : IState
{
    private Player player;
    private PlayerStateMachine stateMachine;
    private Animator animator;

    private float attackDuration = 0.4f; // 공격 유지 시간
    private float timer;

    public PlayerAttack1State(Player player, PlayerStateMachine stateMachine, Animator animator)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animator = animator;
    }

    public void Enter()
    {
        timer = 0f;
        animator.Play("Attack1"); // 정지된 공격 애니메이션 실행
        player.RB.linearVelocity = Vector2.zero; // 공격 중 이동 멈춤
        Debug.Log("Attack1 enter");
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        // 공격 도중엔 입력 무시
        if (timer >= attackDuration)
        {
            float xInput = player.GetMoveInput().x;

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.idleState);
                Debug.Log("Attack1 End: Back to Idle");
            }

            else stateMachine.ChangeState(player.walkState);
        }
    }

    public void Exit() { }
}
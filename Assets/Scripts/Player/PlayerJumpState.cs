using UnityEngine;

public class PlayerJumpState : IState
{
    private Player player;
    private PlayerStateMachine stateMachine;
    private Animator animator;
    private bool hasJumped;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, Animator animator)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animator = animator;
    }

    public void Enter()
    {
        hasJumped = false;
        animator.Play("JumpStart");
    }

    public void Execute()
    {
        // 점프 힘 주기 (Enter에서 바로 줘도 됨)
        if (!hasJumped)
        {
            player.Jump(8f); // 점프 파워
            hasJumped = true;
        }

        float xInput = player.GetMoveInput().x;
        player.Move(xInput);

        // 상승/하강에 따른 애니메이션
        if (player.RB.linearVelocity.y > 0.1f)
        {
            animator.Play("Jump"); // 상승
        }
        else if (player.RB.linearVelocity.y < -0.1f)
        {
            animator.Play("JumpEnd"); // 하강
        }

        // 착지 판정
        if (player.IsGrounded && player.RB.linearVelocity.y <= 0.1f)
        {
            if (xInput == 0)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.walkState);
        }
    }

    public void Exit() { }
}
using UnityEngine;

public class PlayerIdleState : IState
{
    private Player player;
    private PlayerStateMachine stateMachine;
    private Animator animator;

    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, Animator animator)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animator = animator;
    }

    public void Enter()
    {
        animator.Play("Idle");
    }

    public void Execute()
    {
        float xInput = player.GetMoveInput().x;

        if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (Mathf.Abs(xInput) > 0.1f)
        {
            stateMachine.ChangeState(player.walkState);
            return;
        }

        if (player.IsJumpPressed())
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }
    }

    public void Exit() { }
}
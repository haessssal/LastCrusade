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
        Debug.Log("Idle enter");
    }

    public void Execute()
    {
        if (player.IsAttack1Pressed())
        {
            stateMachine.ChangeState(player.attack1State);
            return;
        }

        float xInput = player.GetMoveInput().x;

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
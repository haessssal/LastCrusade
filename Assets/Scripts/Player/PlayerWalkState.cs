using UnityEngine;

public class PlayerWalkState : IState
{
    private Player player;
    private PlayerStateMachine stateMachine;
    private Animator animator;

    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, Animator animator)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animator = animator;
    }

    public void Enter()
    {
        animator.Play("Walk");
    }

    public void Execute()
    {
        float xInput = player.GetMoveInput().x;
        player.Move(xInput);

        if (Mathf.Abs(xInput) < 0.1f)
        {
            stateMachine.ChangeState(player.idleState);
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
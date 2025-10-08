using UnityEngine;

public class PlayerJumpState : IState
{
    private Player player;
    private PlayerStateMachine stateMachine;
    private Animator animator;
    // private bool hasJumped;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, Animator animator)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animator = animator;
    }

    public void Enter()
    {
        // hasJumped = false;
        animator.Play("JumpStart");
        Debug.Log("Jump enter");

        player.Jump(8f);
        // hasJumped = true;
    }

    public void Execute()
    {
        float xInput = player.GetMoveInput().x;
        player.Move(xInput);
        
        if (player.IsAttack1Pressed())
        {
            stateMachine.ChangeState(player.attack1State);
            return;
        }

        if (player.IsGrounded && player.RB.linearVelocity.y <= 0.1f)
        {
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.idleState);
                Debug.Log("Jump End: Back to Idle");
            }

            else stateMachine.ChangeState(player.walkState);
        }
    }

    public void Exit() { }
}
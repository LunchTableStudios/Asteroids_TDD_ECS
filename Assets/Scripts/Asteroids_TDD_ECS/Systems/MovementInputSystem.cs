namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Jobs;

    public class MovementInputSystem : JobComponentSystem
    {
        private struct MovementInputJob : IJobForEach<MovementInput>
        {
            public bool MovementInputPressed;

            public void Execute( ref MovementInput input )
            {
                input.Value = MovementInputPressed ? 1 : 0;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            return ProcessMovementInputJob( currentKeyboard.wKey.isPressed, inputDependencies );
        }

        public JobHandle ProcessMovementInputJob( bool keyIsPressed, JobHandle inputDependencies = default( JobHandle ) )
        {
            MovementInputJob job = new MovementInputJob
            {
                MovementInputPressed = keyIsPressed
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}
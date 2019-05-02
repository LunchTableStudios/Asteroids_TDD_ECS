namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Mathematics;

    public class MovementInputSystem : JobComponentSystem
    {
        private struct MovementInputJob : IJobForEach<MovementInput, Movement>
        {
            public bool MovementInputPressed;

            public void Execute( [ ReadOnly ] ref MovementInput input, ref Movement movement )
            {
                float value = MovementInputPressed ? 1 : 0;
                movement.Value = new float3( 0, value, 0 );
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
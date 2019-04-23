namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;

    public class MovementInputSystem : JobComponentSystem
    {
        [ BurstCompile ]
        private struct MovementInputJob : IJobForEach<MovementInput>
        {
            public bool wKeyIsPressed;

            public void Execute( ref MovementInput input )
            {
                input.Value = wKeyIsPressed ? 1 : 0;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            MovementInputJob job = new MovementInputJob
            {
                wKeyIsPressed = currentKeyboard.wKey.isPressed
            };
            return job.Schedule( this, inputDependencies );
        }
    }
}
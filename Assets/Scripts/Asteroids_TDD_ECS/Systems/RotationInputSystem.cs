namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;

    public class RotationInputSystem : JobComponentSystem
    {
        [ BurstCompile ]
        private struct RotationInputJob : IJobForEach<RotationInput>
        {
            public bool aKeyIsPressed;
            public bool dKeyIsPressed;

            public void Execute( ref RotationInput input )
            {
                input.Value = dKeyIsPressed ? 1 : aKeyIsPressed ? -1 : 0;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            RotationInputJob job = new RotationInputJob
            {
                aKeyIsPressed = currentKeyboard.aKey.isPressed,
                dKeyIsPressed = currentKeyboard.dKey.isPressed
            };
            return job.Schedule( this, inputDependencies );
        }
    }
}
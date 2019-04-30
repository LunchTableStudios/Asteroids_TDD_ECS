namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;

    public class RotationInputSystem : JobComponentSystem
    {
        private struct RotationInputJob : IJobForEach<RotationInput>
        {
            public bool AKeyIsPressed;
            public bool DKeyIsPressed;

            public void Execute( ref RotationInput input )
            {
                input.Value = AKeyIsPressed ? 1 : DKeyIsPressed ? -1 : 0;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            RotationInputJob job = new RotationInputJob
            {
                AKeyIsPressed = currentKeyboard.aKey.isPressed,
                DKeyIsPressed = currentKeyboard.dKey.isPressed
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}
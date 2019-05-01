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
            public bool RotateLeftKeyIsPressed;
            public bool RotateRightKeyIsPressed;

            public void Execute( ref RotationInput input )
            {
                int direction = 0;

                direction += RotateLeftKeyIsPressed ? 1 : 0;
                direction += RotateRightKeyIsPressed ? -1 : 0;

                input.Value = direction;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            return ProcessRotationInputJob( currentKeyboard.aKey.isPressed, currentKeyboard.dKey.isPressed, inputDependencies );
        }

        public JobHandle ProcessRotationInputJob( bool leftKeyPressed, bool rightKeyPressed, JobHandle inputDependencies = default( JobHandle ) )
        {
            RotationInputJob job = new RotationInputJob
            {
                RotateLeftKeyIsPressed = leftKeyPressed,
                RotateRightKeyIsPressed = rightKeyPressed
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}
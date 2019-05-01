namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Jobs;

    public class ShootInputSystem : JobComponentSystem
    {
        private struct ShootInputJob : IJobForEach<ShootInput>
        {
            public bool ShootInputPressed;

            public void Execute( ref ShootInput input )
            {
                input.IsShooting = ShootInputPressed;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            return ProcessShootInputJob( currentKeyboard.spaceKey.isPressed, inputDependencies );
        }

        public JobHandle ProcessShootInputJob( bool keyPressed, JobHandle inputDependencies = default( JobHandle ) )
        {
            ShootInputJob job = new ShootInputJob
            {
                ShootInputPressed = keyPressed
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}
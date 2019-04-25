namespace Asteroids_TDD_ECS
{
    using UnityEngine.Experimental.Input;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;

    public class ShootInputSystem : JobComponentSystem
    {
        private struct ShootInputJob : IJobForEach<ShootInput>
        {
            public bool spacebarIsPressed;

            public void Execute( ref ShootInput input )
            {
                if( spacebarIsPressed )
                    input.IsShooting = true;
                else
                    input.IsShooting = false;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            Keyboard currentKeyboard = Keyboard.current;

            ShootInputJob job = new ShootInputJob
            {
                spacebarIsPressed = currentKeyboard.spaceKey.isPressed
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}
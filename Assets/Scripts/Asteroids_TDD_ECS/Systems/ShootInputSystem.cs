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

            ShootInputJob job = new ShootInputJob
            {
                ShootInputPressed = currentKeyboard.spaceKey.isPressed
            };

            return job.Schedule( this, inputDependencies );
        }
    }
}
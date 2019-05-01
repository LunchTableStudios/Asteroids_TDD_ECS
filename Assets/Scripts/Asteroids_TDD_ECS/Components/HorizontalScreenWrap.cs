namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct HorizontalScreenWrap : IComponentData
    {
        public float Min;
        public float Max;
    }
}
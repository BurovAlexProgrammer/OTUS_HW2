namespace Listener
{
    public interface IUpdateListener : IGameListener
    {
        void OnUpdate(float deltaTime);
    }
}
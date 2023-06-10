namespace Listener
{
    public interface IGameInitListener : IGameListener
    {
        void OnInit();
    }

    public interface IGameStartListener : IGameListener
    {
        void OnStart();
    }

    public interface IGameOverListener : IGameListener
    {
        void OnGameOver();
    }
    
    public interface IUpdateListener : IGameListener
    {
        void OnUpdate(float deltaTime);
    }
    
    public interface IFixedUpdateListener : IGameListener
    {
        void OnFixedUpdate(float fixedDeltaTime);
    }
    
    public interface IGameListener
    {
    }
}
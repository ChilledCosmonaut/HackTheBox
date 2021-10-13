namespace Interfaces
{
    public interface IController
    { 
        void Action();

        void ActivatePlayer();

        void DeactivatePlayer();

        bool IsPlayerActive();

        bool ActionsLeft();

        void ResetActions();

        bool IsEscaped();
    }
}

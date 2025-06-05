public interface IToggle
{
    void SetApplicationActive(bool toggle);
    
    void RPC_ToggleMovement(bool toggle);
    
    
    void ConnectToApplicationManager();
}

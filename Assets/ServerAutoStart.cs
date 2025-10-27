using Mirror;
using UnityEngine;

public class ServerAutoStart : MonoBehaviour
{
  void Start()
  {
    if (!Application.isEditor && SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
    {
      // auto-start as dedicated server on headless
      NetworkManager.singleton.StartServer();
      Debug.Log("Server started in headless mode!");
    }
  }
}

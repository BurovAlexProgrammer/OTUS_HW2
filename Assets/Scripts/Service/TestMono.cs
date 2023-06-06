using DI;
using UnityEngine;

namespace Service
{
    public class TestMono : MonoBehaviour
    {
        [Inject]
        public void Construct()
        {
            Debug.Log("Injected");
        }
    }
}
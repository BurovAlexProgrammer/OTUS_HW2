using System;
using Service;
using UnityEngine;

namespace DI
{
    public class DependencyResolver : MonoBehaviour
    {
        [SerializeField] private TestMono testMono;
        private void Start()
        {
            DependencyInjector.Inject(testMono);
        }
    }
}
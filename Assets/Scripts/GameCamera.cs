using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class GameCamera : MonoBehaviour
    {
        public Pacman pacman;

        private Transform _transform;
        private Transform _transformPac;

        void Start()
        {
            _transform = this.transform;
            _transformPac = pacman.transform;
        }

        void Update()
        {
            _transform.position = new Vector3(_transformPac.position.x, _transformPac.position.y, _transform.position.z);
        }
    }
}

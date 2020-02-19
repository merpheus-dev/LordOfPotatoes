using Assets.Code.GridSystems;
using UnityEngine;

namespace Code.GridSystems
{
    [RequireComponent(typeof(MeshRenderer))]
    public class GridVisualizer : MonoBehaviour
    {
        public Grid TargetGrid;

        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public void ClearStatus()
        {
            TargetGrid.Clear();
        }

        private void Update()
        {
            if (TargetGrid == null) return;
            switch (TargetGrid.GridStatus)
            {
                case GridStatus.Current:
                    _renderer.material.color = Color.green;
                    break;
                case GridStatus.Selectable:
                    _renderer.material.color = Color.red;
                    break;
                case GridStatus.None:
                    _renderer.material.color = Color.white;
                    break;
                case GridStatus.Target:
                    _renderer.material.color = Color.yellow;
                    break;
            }
        }
    }
}

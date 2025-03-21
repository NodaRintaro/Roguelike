using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private MapGenerator _mapGenerator;

    private Ray _ray;
    private RaycastHit _hitDontWalkTile;

    private void Start()
    {
        _mapGenerator = FindFirstObjectByType<MapGenerator>();
    }

    private void Update()
    {
        _ray = new Ray(this.transform.position, transform.forward);
    }

    public void OnWalk()
    {
        if (!Physics.Raycast(_ray, out _hitDontWalkTile, MapGenerator.GridSize))
        {
            //TODOFˆÚ“®ˆ—
        }
    }
}

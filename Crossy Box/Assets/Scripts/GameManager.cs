using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int minZPos = -5;
    [SerializeField] int maxSameTerrainRepeat = 3; 

    // int maxZpos;
    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    private void Start()
    {
        // Belakang
        for (int z = minZPos; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        // Depan
        for (int z = 1; z < frontDistance; z++)
        {
            // Generate Block dengan Probabilitas
            var prefab = GetNextRandomTerrainPrefab(z);

            // Instantiate Block
            CreateTerrain(prefab, z);
        }

        player.SetUp(minZPos, extent);
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0 , 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if(map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if(tbRef is Grass)
                return road;
            else
                return grass;
        }


        // Penentuan terrain block
        return Random.value > 0.5f ? road : grass;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    private int[,] platform;

    private void Awake() {
        platform = LevelLoader.LoadPlatform(1, 1);

        for(int x = 0;x < 14;x++) {
            for(int y = 0;y < 14;y++) {
                if(platform[x, y] == 0) {
                    continue;
                }

                Instantiate(platformPrefab, new Vector3(x, -y, 1), Quaternion.identity);
            }
        }
    }
}

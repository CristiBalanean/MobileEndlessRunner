using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOrigin : MonoBehaviour
{
    [SerializeField] private float threshold = 100f;

    [SerializeField] private WorldSpawner worldSpawner;

    private void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;

        if (Mathf.Abs(cameraPosition.y) > threshold)
        {
            RecenterObjects(threshold);
        }
    }

    private void RecenterObjects(float yOffset)
    {
        float yOffsetDifference = worldSpawner.nextSegmentSpawnPosition - threshold;
        worldSpawner.nextSegmentSpawnPosition = yOffsetDifference;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            foreach (GameObject g in SceneManager.GetSceneAt(i).GetRootGameObjects())
            {
                Vector3 pos = g.transform.position;
                pos.y -= yOffset;
                g.transform.position = pos;
            }
        }
        worldSpawner.playerPositionLastFrame = 0;
    }
}

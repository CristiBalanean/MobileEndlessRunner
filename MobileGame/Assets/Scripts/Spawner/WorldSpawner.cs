using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    public int initialSegments = 7;
    public float segmentLength = 8f;
    [SerializeField] private float unloadDistanceThreshold = 30f; // Threshold for unloading segments

    public float playerPositionLastFrame;
    public float nextSegmentSpawnPosition;

    private Queue<GameObject> highwaySegments = new Queue<GameObject>();

    private void Start()
    {
        playerPositionLastFrame = player.position.y;
        nextSegmentSpawnPosition = player.position.y;

        // Create initial highway segments
        for (int i = 0; i < initialSegments; i++)
        {
            SpawnHighwaySegment();
        }
    }

    private void Update()
    {
        float playerPosition = player.position.y;

        if (playerPosition > playerPositionLastFrame + segmentLength)
        {
            Debug.Log("Spawn");
            SpawnHighwaySegment();
            playerPositionLastFrame = playerPosition;
        }

        UnloadHighwaySegments(playerPosition);
    }

    private void SpawnHighwaySegment()
    {
        int segmentsToAdd = initialSegments - highwaySegments.Count;

        for (int i = 0; i < segmentsToAdd; i++)
        {
            GameObject newSegment = ObjectPoolHighway.instance.GetPooledObject();

            if (newSegment != null)
            {
                newSegment.transform.position = new Vector3(0f, nextSegmentSpawnPosition, 0f);
                newSegment.SetActive(true);
                highwaySegments.Enqueue(newSegment);

                // Update the next spawn position
                nextSegmentSpawnPosition += segmentLength;
            }
        }
    }

    private void UnloadHighwaySegments(float playerPosition)
    {
        if (highwaySegments.Peek() != null)
        {
            float firstSegmentPosition = highwaySegments.Peek().transform.position.y;

            if (playerPosition - firstSegmentPosition > unloadDistanceThreshold)
            {
                highwaySegments.Peek().SetActive(false);
                highwaySegments.Dequeue();
            }
        }
    }
}

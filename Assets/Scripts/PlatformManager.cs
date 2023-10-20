using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pillarPrefab;
    private GameObject currentPillar, nextPillar;
    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector2 minMaxRange, spawnRange;
    bool start;
    void Start()
    {
        Events.PlatformChange += CreatePlatform;
    }
    private void OnDestroy()
    {
        Events.PlatformChange -= CreatePlatform;
    }
    void CreatePlatform()
    {
        var currentPlatform = Instantiate(pillarPrefab);
        currentPillar = nextPillar == null ? currentPlatform : nextPillar;
        nextPillar = currentPlatform;
        currentPlatform.transform.position = pillarPrefab.transform.position + startPos;
        Vector3 tempDistance = new Vector3(Random.Range(spawnRange.x, spawnRange.y) + currentPillar.transform.localScale.x * 0.5f, 0, 0);
        startPos += tempDistance;

        if (Random.Range(0, 4) == 0)
        {
            Events.CaseChange?.Invoke(currentPlatform.transform.position);
        }
        Events.CreateStartStick?.Invoke(currentPillar, nextPillar);
        if (!start)
        {
            start = true;
            Events.CreateStartObject?.Invoke(currentPillar, nextPillar);
        }
    }
}

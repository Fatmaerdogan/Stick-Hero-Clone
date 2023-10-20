using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManager : MonoBehaviour
{
    private GameObject currentStick,currentPillar, nextPillar;
    [SerializeField]
    private float stickIncreaseSpeed, maxStickSize;
    [SerializeField]
    private Transform rotateTransform, endRotateTransform;
    [SerializeField]
    private Vector2 minMaxRange, spawnRange;
    [SerializeField]
    private GameObject stickPrefab;
    private GameObject player;
    [SerializeField]
    private GameObject currentCamera;
    private float cameraOffsetX;
    void Start()
    {
        Events.StickSizeChange += ScaleStick;
        Events.StickFall += StickFall;
        Events.GameStart += GameStart;
        Events.CreateStartStick += StartCreatStick;
        Events.DeterminingCameraOffset += DeterminingCameraOffset;
       
    }
    private void OnDestroy()
    {
        Events.StickSizeChange -= ScaleStick;
        Events.StickFall -= StickFall;
        Events.GameStart -= GameStart;
        Events.CreateStartStick -= StartCreatStick;
        Events.DeterminingCameraOffset -= DeterminingCameraOffset;
    }
    void StickFall() => StartCoroutine(FallStick());

    void DeterminingCameraOffset(GameObject _player)
    {
        player = _player;
        cameraOffsetX = currentCamera.transform.position.x - player.transform.position.x;
    }
    public void GameStart()
    {
        SetRandomSize(nextPillar);
    }
    void StartCreatStick(GameObject _currentPillar, GameObject _nextPillar)
    {
        currentPillar  = _currentPillar;
        nextPillar = _nextPillar;
        Vector3 stickPos = stickPrefab.transform.position;
        stickPos.x += (currentPillar.transform.localPosition.x * 0.5f - 0.05f);
        currentStick = Instantiate(stickPrefab, stickPos, Quaternion.identity);
    }
    void ScaleStick()
    {
        Vector3 tempScale = currentStick.transform.localScale;
        tempScale.y += Time.deltaTime * stickIncreaseSpeed;
        if (tempScale.y > maxStickSize)
            tempScale.y = maxStickSize;
        currentStick.transform.localScale = tempScale;
    }
    IEnumerator FallStick()
    {
        GameManager.instance.currentState = GameState.NONE;
        var x = Rotate(currentStick.transform, rotateTransform, 0.4f);
        yield return x;

        Vector3 movePosition = currentStick.transform.position + new Vector3(currentStick.transform.localScale.y, 0, 0);
        movePosition.y = player.transform.position.y;
        x = Move(player.transform, movePosition, 0.5f);
        yield return x;

        var results = Physics2D.RaycastAll(player.transform.position, Vector2.down);
        var result = Physics2D.Raycast(player.transform.position, Vector2.down);
        foreach (var temp in results)
        {
            if (temp.collider.CompareTag("Platform"))
            {
                result = temp;
            }
        }

        if (!result || !result.collider.CompareTag("Platform"))
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 1f;
            x = Rotate(currentStick.transform, endRotateTransform, 0.5f);
            yield return x;
            Events.GameOver?.Invoke();
        }
        else
        {
            Events.ScoreUpdate?.Invoke();

            movePosition = player.transform.position;
            movePosition.x = nextPillar.transform.position.x + nextPillar.transform.localScale.x * 0.5f - 0.35f;
            x = Move(player.transform, movePosition, 0.2f);
            yield return x;

            movePosition = currentCamera.transform.position;
            movePosition.x = player.transform.position.x + cameraOffsetX;
            x = Move(currentCamera.transform, movePosition, 0.5f);
            yield return x;

            Events.PlatformChange?.Invoke();
            SetRandomSize(nextPillar);
            GameManager.instance.currentState = GameState.INPUT;
            Vector3 stickPosition = currentPillar.transform.position;
            stickPosition.x += currentPillar.transform.localScale.x * 0.5f - 0.05f;
            stickPosition.y = currentStick.transform.position.y;
            stickPosition.z = currentStick.transform.position.z;
            currentStick = Instantiate(stickPrefab, stickPosition, Quaternion.identity);
        }
    }
    void SetRandomSize(GameObject pillar)
    {
        var newScale = pillar.transform.localScale;
        var allowedScale = nextPillar.transform.position.x - currentPillar.transform.position.x
            - currentPillar.transform.localScale.x * 0.5f - 0.4f;
        newScale.x = Mathf.Max(minMaxRange.x, Random.Range(minMaxRange.x, Mathf.Min(allowedScale, minMaxRange.y)));
        pillar.transform.localScale = newScale;
    }
    IEnumerator Move(Transform currentTransform, Vector3 target, float time)
    {
        var passed = 0f;
        var init = currentTransform.transform.position;
        while (passed < time)
        {
            passed += Time.deltaTime;
            var normalized = passed / time;
            var current = Vector3.Lerp(init, target, normalized);
            currentTransform.position = current;
            yield return null;
        }
    }
    IEnumerator Rotate(Transform currentTransform, Transform target, float time)
    {
        var passed = 0f;
        var init = currentTransform.transform.rotation;
        while (passed < time)
        {
            passed += Time.deltaTime;
            var normalized = passed / time;
            var current = Quaternion.Slerp(init, target.rotation, normalized);
            currentTransform.rotation = current;
            yield return null;
        }
    }

}

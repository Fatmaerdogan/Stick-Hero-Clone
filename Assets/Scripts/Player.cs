using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.currentState == GameState.INPUT)
        {
            if (Input.GetMouseButton(0))
            {
                GameManager.instance.currentState = GameState.GROWING;
                Events.StickSizeChange?.Invoke();
            }
        }

        if (GameManager.instance.currentState == GameState.GROWING)
        {
            if (Input.GetMouseButton(0))
            {
                Events.StickSizeChange?.Invoke();
            }
            else
            {
                Events.StickFall?.Invoke();
            }
        }
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
}

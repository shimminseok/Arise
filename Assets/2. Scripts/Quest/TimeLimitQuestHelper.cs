using UnityEngine;

/// <summary>
/// 시간 제한 퀘스트용 헬퍼. 스테이지 시작 시 StartTimer() 호출, 완료 시 TryClear() 호출.
/// </summary>
public class TimeLimitQuestHelper : MonoBehaviour
{
    private float _startTime;

    [SerializeField] private float _limitSeconds = 60f;
    [SerializeField] private bool _debug = true;

    /// <summary>
    /// 제한 시간 타이머 시작 (스테이지 시작 시 호출)
    /// </summary>
    public void StartTimer()
    {
        _startTime = Time.time;
        if (_debug) Debug.Log("제한 시간 타이머 시작됨");
    }

    /// <summary>
    /// 제한 시간 내 클리어 여부 판정 (스테이지 클리어 시 호출)
    /// </summary>
    public void TryClear()
    {
        float elapsed = Time.time - _startTime;

        if (_debug) Debug.Log($"경과 시간: {elapsed:F2}초");

        if (elapsed <= _limitSeconds)
        {
            // 성공 처리
            QuestManager.Instance.UpdateProgress(QuestType.TimeLimitClear, 1);
            if (_debug) Debug.Log("제한 시간 내 클리어 성공!");
        }
        else
        {
            if (_debug) Debug.Log("제한 시간 초과로 실패");
        }
    }

    /// <summary>
    /// 유틸: 현재 남은 시간 반환
    /// </summary>
    public float GetRemainingTime()
    {
        return Mathf.Max(0f, _limitSeconds - (Time.time - _startTime));
    }
}
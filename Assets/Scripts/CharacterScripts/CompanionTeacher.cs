using UnityEngine;
using DG.Tweening;

public class CompanionTeacher : MonoBehaviour
{
    public Transform lessonPosition;
    public float moveDuration = 1.0f;

    private Vector3 originalOffset;

    void Start()
    {
        originalOffset = transform.localPosition;
    }

    public void MoveToLessonPosition()
    {
        transform.DOLocalMove(lessonPosition.localPosition, moveDuration);
    }

    public void ReturnToOriginalPosition()
    {
        transform.DOLocalMove(originalOffset, moveDuration);
    }
}
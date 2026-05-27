using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TweenSample : MonoBehaviour
{
    [Header("대상")]
    public RectTransform UITarget;
    public GameObject ObjectTarget;
    private Vector3 originalPosition;
    public Image UIImage;

    [Header("텍스트")]
    public TMP_Text countText;
    public int currentValue = 0;
    public int addValue = 100;
    private int targetValue;

    [Header("플래시 효과")]
    public Color flashColor = Color.red;
    private Color originalColor;

    public CanvasGroup fadeTarget;

    public GameObject coinPrefab;

    void Start()
    {
        if (UIImage != null)
        {
            originalColor = UIImage.color;   //초기 색상 저장
        }

        if (ObjectTarget != null)
        {
            originalPosition = ObjectTarget.transform.localPosition;
        }

        if (fadeTarget != null)
        {
            fadeTarget.alpha = 0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayPunchUIScale();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayPunchObjectScale();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayObjectShake();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayCountUP();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayColorFlash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayFade();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Vector3 dropPosition = transform.position + Vector3.up;
            Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        }
    }

    public void PlayPunchUIScale()
    {
        if (UITarget == null) return;
        UITarget.DOKill();      //이전 실행중이던 Tween효과 제거

        UITarget.localScale = Vector3.one;    //스케일 초기화
        UITarget.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f);  //방향 * 크기, 시간, 진동 횟수, 탄성
    }

    public void PlayPunchObjectScale()
    {
        if (ObjectTarget == null) return;
        ObjectTarget.transform.DOKill();      //이전 실행중이던 Tween효과 제거

        ObjectTarget.transform.localScale = Vector3.one;    //스케일 초기화
        ObjectTarget.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f);  //방향 * 크기, 시간, 진동 횟수, 탄성
    }

    public void PlayObjectShake()
    {
        if (ObjectTarget == null) return;
        ObjectTarget.transform.DOKill();      //이전 실행중이던 Tween효과 제거
        //ObjectTarget.transform.DOComplete();

        ObjectTarget.transform.localPosition = originalPosition;    //위치 초기화
        ObjectTarget.transform.DOShakePosition(0.3f, 0.2f, 20, 90f); //시간, 강도, 진동 횟수, 랜덤성
    }

    public void PlayCountUP()
    {
        if (countText == null) return;

        targetValue += addValue;            //목표 숫자
        DOTween.Kill("CountTween", true);   //기존 CountTween연출을 완료 한 후 종료 처리

        DOTween.To(
            () => currentValue,         //현재 값
            value =>                    //중간 값이 바뀔 때마다 실행되는 함수.
            {
                currentValue = value;
                countText.text = currentValue.ToString();
            },
            targetValue,        //목표값
            0.5f                //걸리는 시간
            )
            .SetEase(Ease.OutQuad)
            .SetId("CountTween");
    }

    public void PlayColorFlash()
    {
        if (UIImage == null) return;

        UIImage.DOKill();

        UIImage.color = originalColor;
        UIImage.DOColor(flashColor, 0.1f)
            .OnComplete(() =>
            {
                UIImage.DOColor(originalColor, 0.2f);
            });
    }

    public void PlayFade()
    {
        if (fadeTarget == null) return;
        fadeTarget.DOKill();
        fadeTarget.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Append(fadeTarget.DOFade(1, 0.2f));     //0.2초 동안 나타난다.
        seq.AppendInterval(0.5f);                   //0.5초 유지
        seq.Append(fadeTarget.DOFade(0, 0.3f));     //0.3초 동안 사라진다.
    }
}
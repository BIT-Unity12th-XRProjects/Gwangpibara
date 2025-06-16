using UnityEngine;
using UnityEngine.InputSystem;

public class MainController : MonoBehaviour
{
    /// <summary>
    /// 게임 진행의 메인 화면
    /// </summary>
    StepData _curStepData;

    void Start()
    {
        SetStep(MasterDataManager.Instance.GetMasterStepData(10101)); //1번 주제로 시작
    }

    private void Update()
    {
        //UI 입력을 대신할 테스트 입력 부분
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            ClickNextButton();
            SubmitAnswer("박광호");
        }
    }

    private void ClickNextButton()
    {
        if (_curStepData.ClearType.Equals(ClearType.Click))
        {
            GoNextStep();
        }
    }

    private void GoNextStep()
    {
        int nextStepId = _curStepData.NextStep;
        if(nextStepId == StepData.INVALID_NUMBER)
        {
            Debug.Log("게임 종료");
            return;
        }
        StepData nextStep = MasterDataManager.Instance.GetMasterStepData(nextStepId);
        SetStep(nextStep);
        
    }

    private void SetStep(StepData stepData)
    {
        //현재 단계를 stepData로 맞추기
        _curStepData = stepData;
        Debug.Log(stepData.PrintText); //디버그용
    }

    public void SubmitAnswer(string answer)
    {
        Debug.Log($"{answer} 입력 받음 현재 단계 정답 코드 {_curStepData.SuccessCode} 정답인가 {answer == _curStepData.SuccessCode}");
        if(_curStepData.ClearType == ClearType.Answer)
        {
            bool isCorrect = CheckAnswer(answer);
            if (isCorrect)
            {
                GoNextStep();
            }
        }
    }

    private bool CheckAnswer(string answer)
    {
        return _curStepData.SuccessCode == answer;
    }
}

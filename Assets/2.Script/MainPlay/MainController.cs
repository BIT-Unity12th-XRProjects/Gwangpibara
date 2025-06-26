using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainController : Singleton<MainController>
{
    /// <summary>
    /// 게임 진행의 메인 화면
    /// </summary>

    private int _progressStep; //최대 진행한 단계
    private StepData _curStepData; //현재 세팅된 단계
    private GameUIData _gameUIData;
    [SerializeField] private ItemInventory _itemInventory;
    public Action<GameUIData> onChangeStepData;
    public Action<GameUIData> onShowHint;

    private void Update()
    {
        //테스트 입력 부분 - 추후 UI, 다른 클래스로부터 호출 될 녀석들
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
                 SubmitAnswer("박광호"); //정답 텍스트 입력하기
            AcquireItem(MasterDataManager.Instance.GetMasterItemData(10102)); //10101 아이템 전달받기
        }
    }

    //게임 시작
    public void StartGame(int stepID)
    {
        _itemInventory = new();
        _gameUIData = new();
        SetStep(stepID);
    }

    public void ClickNextButton()
    {
        //이전 단계를 보다가 다음으로 가려는 경우
        if (_curStepData.ID < _progressStep)
        {
            GoNextStep();
            return;
        }

        //진행 단계에서 다음으로 가려는 경우
        if (_curStepData.ClearType.Equals(ClearType.Click))
        {
            GoNextStep();
        }
    }

    public void ClickBeforeButton()
    {
        GoBeforeStep();
    }

    public void SubmitAnswer(string answer)
    {
        Debug.Log($"{answer} 입력 받음 현재 단계 정답 코드 {_curStepData.SuccessCode} 정답인가 {answer == _curStepData.SuccessCode}");
        if (_curStepData.ClearType == ClearType.Answer)
        {
            bool isCorrect = CheckAnswer(answer);
            if (isCorrect)
            {
                FeedbackManager.Instance.PlayEffect(true, SFXType.Correct);
                GoNextStep();
            }
            else
            {
                FeedbackManager.Instance.PlayEffect(false, SFXType.InCorrect);
                PopUpManager.Instance.PopMessege("틀렸쥬");
            }
        }
    }

    public void PleaseHint()
    {
        onShowHint?.Invoke(_gameUIData);
    }

    public bool AcquireItem(ItemData itemData)
    {
        Debug.Log($"ID:{itemData.ID}, 이름:{itemData.Name} 아이템 발견");
        if (_itemInventory.CheckAcquireItemCondition(itemData, _curStepData.ID) == false)
        {
            //습득불가능한 아이템
            // Debug.Log("습득 불가");
            return false;
        }
        //습득가능하면
        _itemInventory.AddItem(itemData);
        // Debug.Log($"{itemData.Name} 습득");
        if (_curStepData.ClearType == ClearType.Collect)
        {
            bool isCorrect = CheckAnswer(itemData.ID.ToString());//수집한 단서 ID를 습득했다고 전달 
            if (isCorrect)
            {
                FeedbackManager.Instance.PlayEffect(true, SFXType.Correct);
                GoNextStep();
            }
        }

        return true;
    }

    public GameUIData GetGameUIData()
    {
        return _gameUIData;
    }

    public ItemInventory GetItemInventory()
    {
        return _itemInventory;
    }

    private void SetStep(int stepID)
    {
        //현재 단계를 stepData로 맞추기
        _curStepData = MasterDataManager.Instance.GetMasterStepData(stepID);
        Debug.Log(_curStepData.PrintText); //디버그용

        _gameUIData.SetData(_curStepData);
        onChangeStepData?.Invoke(_gameUIData);
        RenewProgress();
    }

    private void RenewProgress()
    {
        //현재 단계와 최종 진행된 단계를 비교해서 진행 수치 반영

        //새로 세팅된 단계가 진행단계보다 낮으면 갱신할 것 없음.
        if (_curStepData.ID <= _progressStep)
        {
            return;
        }

        //단계가 진행되었다면
        _progressStep = _curStepData.ID; //진행 수치 조정

        //새로 세팅 된 단계에서 클리어 조건 체크
        if (_curStepData.ClearType == ClearType.Collect)
        {
            CheckInventoryCondition();
        }
    }

    private void GoNextStep()
    {
        int nextStepId = _curStepData.NextStep;
        if (nextStepId == StepData.INVALID_NUMBER)
        {
            Debug.Log("게임 종료");
            return;
        }

        SetStep(nextStepId);

    }

    private void GoBeforeStep()
    {
        int beforeStepId = _curStepData.PreStep;
        if (beforeStepId == StepData.INVALID_NUMBER)
        {
            Debug.Log("없는 단계");
            return;
        }
        SetStep(beforeStepId);
    }

    private void CheckInventoryCondition()
    {
        //단계가 바뀌었을 때 현재 단계의 클리어 조건이 아이템 보유고, 이미 해당 아이템을 갖고 있는지 체크
        int needItemID = int.Parse(_curStepData.SuccessCode);
        int haveAmount = _itemInventory.GetItemAmount(needItemID);
        if (haveAmount >= 1)
        {
            Debug.Log("이미 보유한 아이템 조건 다음 스텝으로");
            FeedbackManager.Instance.PlayEffect(true, SFXType.Correct);
            GoNextStep();
        }
    }

    private bool CheckAnswer(string answer)
    {
        return _curStepData.SuccessCode == answer;
    }
}

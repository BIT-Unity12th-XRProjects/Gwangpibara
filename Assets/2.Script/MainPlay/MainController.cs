using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainController : MonoBehaviour
{
    /// <summary>
    /// 게임 진행의 메인 화면
    /// </summary>
    private StepData _curStepData;
    private ItemInventory _itemInventory;

    void Start()
    {
        _itemInventory = new();
        SetStep(MasterDataManager.Instance.GetMasterStepData(10101)); //1번 주제로 시작
    }

    private void Update()
    {
        //테스트 입력 부분 - 추후 UI, 다른 클래스로부터 호출 될 녀석들
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            ClickNextButton(); //다음 단계 버튼 누르기
            SubmitAnswer("박광호"); //정답 텍스트 입력하기
            AcquireItem(MasterDataManager.Instance.GetMasterItemData(10101)); //10101 아이템 전달받기
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
        StepData nextStep = MasterDataManager.Instance.GetMasterStepData(nextStepId);
        SetStep(nextStep);

    }

    private void SetStep(StepData stepData)
    {
        //현재 단계를 stepData로 맞추기
        _curStepData = stepData;
        Debug.Log(stepData.PrintText); //디버그용

        //세팅된 단계의 클리어 조건이 아이템 수집인경우, 이미 수집했는지 체크
        if(stepData.ClearType == ClearType.Collect)
        {
            CheckInventoryCondition();
        }
        
    }

    private void ClickNextButton()
    {
        if (_curStepData.ClearType.Equals(ClearType.Click))
        {
            GoNextStep();
        }
    }


    public void SubmitAnswer(string answer)
    {
        Debug.Log($"{answer} 입력 받음 현재 단계 정답 코드 {_curStepData.SuccessCode} 정답인가 {answer == _curStepData.SuccessCode}");
        if (_curStepData.ClearType == ClearType.Answer)
        {
            bool isCorrect = CheckAnswer(answer);
            if (isCorrect)
            {
                GoNextStep();
            }
        }
    }

    public bool AcquireItem(ItemData itemData)
    {
        Debug.Log($"ID:{itemData.ID}, 이름:{itemData.Name} 아이템 발견");
        if (CheckAcquireItemCondition(itemData) == false)
        {
            //습득불가능한 아이템
            Debug.Log("습득 불가");
            return false;
        }
        //습득가능하면
        AddItem(itemData);
        Debug.Log($"{itemData.Name} 습득");
        if (_curStepData.ClearType == ClearType.Collect)
        {
            bool isCorrect = CheckAnswer(itemData.ID.ToString());//수집한 단서 ID를 습득했다고 전달 
            if (isCorrect)
            {
                GoNextStep();
            }
        }

        return true;
    }

    private bool CheckAcquireItemCondition(ItemData itemData)
    {
        //습득하려는 아이템이 습득가능한 상태인지
        if ( _curStepData.ID < itemData.AquireStep)
        {
            //진행한 단계가 습득단계 아래면 습득 불가
            return false;
        }

        //중복 아이템인지
        if(_itemInventory.GetItemAmount(itemData) != 0)
        {
            return false;
        }

        return true;
    }

    private void AddItem(ItemData itemData)
    {
       _itemInventory.AddItem(itemData);
    }

    private void CheckInventoryCondition()
    {
        //단계가 바뀌었을 때 현재 단계의 클리어 조건이 아이템 보유고, 이미 해당 아이템을 갖고 있는지 체크
        int needItemID = int.Parse(_curStepData.SuccessCode);
        int haveAmount = _itemInventory.GetItemAmount(needItemID);
        if(haveAmount >= 1)
        {
            Debug.Log("이미 보유한 아이템 조건 다음 스텝으로");
            GoNextStep();
        }
    }

    private bool CheckAnswer(string answer)
    {
        return _curStepData.SuccessCode == answer;
    }
}

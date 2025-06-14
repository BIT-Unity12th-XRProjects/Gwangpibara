using UnityEngine;
using UnityEngine.InputSystem;

public class MainController : MonoBehaviour
{
    /// <summary>
    /// ���� ������ ���� ȭ��
    /// </summary>
    StepData _curStepData;

    void Start()
    {
        SetStep(MasterDataManager.Instance.GetMasterStepData(10101)); //1�� ������ ����
    }

    private void Update()
    {
        //UI �Է��� ����� �׽�Ʈ �Է� �κ�
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            ClickNextButton();
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
            Debug.Log("���� ����");
            return;
        }
        StepData nextStep = MasterDataManager.Instance.GetMasterStepData(nextStepId);
        SetStep(nextStep);
        
    }

    private void SetStep(StepData stepData)
    {
        //���� �ܰ踦 stepData�� ���߱�
        Debug.Log(stepData.PrintText); //����׿�
    }
}

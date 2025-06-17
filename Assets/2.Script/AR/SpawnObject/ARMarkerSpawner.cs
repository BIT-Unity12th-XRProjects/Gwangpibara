using System;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.UI;
using UnityEngine;

public class ARMarkerSpawner : MonoBehaviour
{
    [SerializeField] private Image _objectImage;
    public static Action<Vector3, Quaternion, string, string> OnPositionDebug; 
    public enum SpawnTriggerType
   {
       SelectAttempt,
       InputAction,
   }
       
   [SerializeField]
   [Tooltip("The AR ray interactor that determines where to spawn the object.")]
   XRRayInteractor m_ARInteractor;

   public XRRayInteractor arInteractor
   {
       get => m_ARInteractor;
       set => m_ARInteractor = value;
   }

   [SerializeField]
   [Tooltip("The behavior to use to spawn objects.")]
   MarkerSpawner _markerSpawner;

   public MarkerSpawner markerSpawner
   {
       get => _markerSpawner;
       set => _markerSpawner = value;
   }

   [SerializeField]
   [Tooltip("Whether to require that the AR Interactor hits an AR Plane with a horizontal up alignment in order to spawn anything.")]
   bool m_RequireHorizontalUpSurface;

   public bool requireHorizontalUpSurface
   {
       get => m_RequireHorizontalUpSurface;
       set => m_RequireHorizontalUpSurface = value;
   }

   [SerializeField]
   [Tooltip("The type of trigger to use to spawn an object, either when the Interactor's select action occurs or " +
       "when a button input is performed.")]
   SpawnTriggerType m_SpawnTriggerType;

   public SpawnTriggerType spawnTriggerType
   {
       get => m_SpawnTriggerType;
       set => m_SpawnTriggerType = value;
   }

   [SerializeField]
   XRInputButtonReader m_SpawnObjectInput = new XRInputButtonReader("Spawn Object");

   public XRInputButtonReader spawnObjectInput
   {
       get => m_SpawnObjectInput;
       set => XRInputReaderUtility.SetInputProperty(ref m_SpawnObjectInput, value, this);
   }

   [SerializeField]
   [Tooltip("When enabled, spawn will not be triggered if an object is currently selected.")]
   bool m_BlockSpawnWhenInteractorHasSelection = true;
   public bool blockSpawnWhenInteractorHasSelection
   {
       get => m_BlockSpawnWhenInteractorHasSelection;
       set => m_BlockSpawnWhenInteractorHasSelection = value;
   }

   bool m_AttemptSpawn;
   bool m_EverHadSelection;
   public bool isButtonClick;

   void OnEnable()
   {
       m_SpawnObjectInput.EnableDirectActionIfModeUsed();
   }

   void OnDisable()
   {
       m_SpawnObjectInput.DisableDirectActionIfModeUsed();
   }

   void Start()
   {
        if (_markerSpawner == null)
#if UNITY_2023_1_OR_NEWER
        _markerSpawner = FindAnyObjectByType<MarkerSpawner>();
#else
        _markerSpawner = FindObjectOfType<MarkerSpawner>();
#endif

        if (m_ARInteractor == null)
        {
            Debug.LogError("Missing AR Interactor reference, disabling component.", this);
            enabled = false;
        }
   }

    void Update()
    {
        if (m_AttemptSpawn)
        {
            m_AttemptSpawn = false;

            if (m_ARInteractor.hasSelection)
                return;

            var isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1);
            if (!isPointerOverUI && m_ARInteractor.TryGetCurrentARRaycastHit(out var arRaycastHit))
            {
                if (!(arRaycastHit.trackable is ARPlane arPlane))
                    return;

                if (m_RequireHorizontalUpSurface && arPlane.alignment != PlaneAlignment.HorizontalUp)
                    return;

                if (!isButtonClick)
                {
                    return;
                }
                
                string spawnedObjectName;
                string objectID;
                
                GameObject spawnedObject = _markerSpawner.TrySpawnObject(arRaycastHit.pose.position, arPlane.normal, out spawnedObjectName, out objectID);
                
                OnPositionDebug?.Invoke(arRaycastHit.pose.position,spawnedObject.transform.rotation, spawnedObjectName, objectID);
            }

            return;
        }

        var selectState = m_ARInteractor.logicalSelectState;

        if (m_BlockSpawnWhenInteractorHasSelection)
        {
            if (selectState.wasPerformedThisFrame)
                m_EverHadSelection = m_ARInteractor.hasSelection;
            else if (selectState.active)
                m_EverHadSelection |= m_ARInteractor.hasSelection;
        }

        m_AttemptSpawn = false;
        switch (m_SpawnTriggerType)
        {
            case SpawnTriggerType.SelectAttempt:
                if (selectState.wasCompletedThisFrame)
                    m_AttemptSpawn = !m_ARInteractor.hasSelection && !m_EverHadSelection;
                break;

            case SpawnTriggerType.InputAction:
                if (m_SpawnObjectInput.ReadWasPerformedThisFrame())
                    m_AttemptSpawn = !m_ARInteractor.hasSelection && !m_EverHadSelection;
                break;
        }
    }

    public void OnButtonClick()
    {
        isButtonClick = true;
        _objectImage.gameObject.SetActive(true);
    }

    public void OffButtonClick()
    {
        isButtonClick = false;
        _objectImage.gameObject.SetActive(false);
    }
}
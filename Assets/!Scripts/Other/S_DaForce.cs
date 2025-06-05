using System.Collections;
using UnityEngine;
using Fusion;
using Input;

public class S_DaForce : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject toolPrefab;
    
    [Header("Settings")]
    [SerializeField] private float minPullDistanceCalculation = 1f;
    [SerializeField] private float maxPullDistanceCalculation = 10f;
    [SerializeField] private float deSpawnDistance = 15f;
    [SerializeField] private float speedMultiplier = 8f;
    
    private float _returnSpeed = 8f;
    
    private NetworkObject _tool;
    private Rigidbody _rigidbody;
    

    private bool _moving;

    private bool IsLocal => Object && Object.HasStateAuthority;
    
    // Bool to see if the grab button is pressed
    private bool _grabButtonPressed;
    // Bool to see if the specific tool button is pressed
    private bool _toolButtonPressed;

    private bool _canForcePull;
    
    

    // private void GrabTool()
    // {
    //     if (!_interactor) return;
    //     if (!_tool) return;
    //     if (_tool.TryGetComponent(out NetworkObject tool) && _tool.TryGetComponent(out IXRSelectInteractable interactable))
    //     {
    //         _interactor.TrySelect(tool, interactable);
    //     }
    // }
    
    public void ChangeGrabButtonState(InputInfo info)
    {
        if (info.context.started)
        {
            _grabButtonPressed = true;
            UpdatePull();
        }
        else if (info.context.canceled)
        {
            _grabButtonPressed = false;
            UpdatePull();
        }
    }
    
    public void ChangeToolButtonState(InputInfo info)
    {
        if (info.context.performed)
        {
            _toolButtonPressed = true;
            UpdatePull();
        }
        else if (info.context.canceled)
        {
            _toolButtonPressed = false;
            UpdatePull();
        }
    }
    
    private void UpdatePull()
    {
        if (!_grabButtonPressed || !_toolButtonPressed || transform.childCount > 1)
        {
            if (!_tool) return;
            _moving = false;
            _rigidbody.isKinematic = false;
            _canForcePull = false;
            return;
        }
        if (!_tool)
        {
            SpawnTool();
            return;
        }
        UseForce();
    }
    
    private void UseForce()
    {
        CalculateDistance();
        _canForcePull = true;
        // if (_tool == null) return;
        // 
        // if (transform.childCount > 1) return;
        // 
        // float distance = Vector3.Distance(_tool.transform.position, transform.position);
        // float time = distance / returnSpeed;
        // float timeElapsed = 0f;
        // 
        // var toolPos = _tool.transform.position; // Get the starting position of the tool in Vector3
        // var toolRot = _tool.transform.rotation; // Get the starting rotation of the tool in Quaternion
        // 
        // while (_grabButtonPressed && _toolButtonPressed && !_canForcePull)
        // {
        //     if (transform.childCount > 1) break;
        //     
        //     _tool.transform.position = Vector3.Lerp(toolPos, transform.position, timeElapsed / time);
        //     _tool.transform.rotation = Quaternion.Lerp(toolRot, transform.rotation, timeElapsed / time);
        //     
        //     timeElapsed += Time.deltaTime;
        //     
        //     if (!(timeElapsed >= time)) continue;
        //     
        //     // If the tool is close enough to the player, stop the lerp and add the tool to the player's hand
        //     // Needs to activate the regular grab function
        //     // TODO: Activate the regular grab function
        //     _canForcePull = true;
        //     StartCoroutine(SetPull());
        //     // _tool.transform.SetParent(transform);
        //     // _tool.transform.localPosition = Vector3.zero;
        //     // _tool.transform.localRotation = Quaternion.identity;
        //     
        //     break;
        // }
    }

    private void CalculateDistance()
    {
        var distance = Vector3.Distance(_tool.transform.position, transform.position);
        if (distance <= minPullDistanceCalculation)
        {
            distance = minPullDistanceCalculation;
        }
        else if (distance >= maxPullDistanceCalculation)
        { 
            distance = maxPullDistanceCalculation;
        }

        _returnSpeed = distance * speedMultiplier;
    }
    
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        // Check if the tool is null or not
        if (!_tool) return;
        
        // See if the player has something in their hand already
        if (transform.childCount > 1) return;
        
        if (!_canForcePull) return;
        
        if (_grabButtonPressed && _toolButtonPressed)
        {
            _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, transform.position, Time.deltaTime * _returnSpeed);
            _tool.transform.rotation = Quaternion.RotateTowards(_tool.transform.rotation, transform.rotation, Time.deltaTime * _returnSpeed * 17);
            if (_moving) return;
            _moving = true;
            _rigidbody.isKinematic = true;
        }
        else
        {
            if (!_moving) return;
            _moving = false;
            _rigidbody.isKinematic = false;
            _canForcePull = false;
        }
    }
    
    private void SpawnTool()
    {
        if(_tool) return;
        if (!IsLocal) return;
        _tool = Runner.Spawn(toolPrefab, transform.position, Quaternion.identity, Runner.LocalPlayer);
        _rigidbody = _tool.GetComponent<Rigidbody>();
        
        StartCoroutine(LookForToolDistance());
        // await Shared.GainStateAuthority(_tool);
        // _tool.transform.SetParent(transform);
        // _tool.transform.localPosition = Vector3.zero;
        // _tool.transform.localRotation = Quaternion.identity;
    }

    private IEnumerator LookForToolDistance()
    {
        if (!IsLocal) yield break;
        while (_tool)
        {
            yield return new WaitForSeconds(1f);
            var distance = Vector3.Distance(_tool.transform.position, transform.position);
            if (distance < deSpawnDistance) continue;
            Runner.Despawn(_tool);
            _tool = null;
        }
    }

    // IEnumerator SetPull()
    // {
    //     yield return new WaitForSeconds(3f);
    //     _canForcePull = false;
    // }
}

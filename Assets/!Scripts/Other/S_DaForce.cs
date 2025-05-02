using System.Collections;
using System.Threading.Tasks;
using Extentions.Networking;
using UnityEngine;
using Fusion;
using Input;

public class S_DaForce : NetworkBehaviour
{
    
    [SerializeField] private GameObject toolPrefab;
    [SerializeField] private float returnSpeed = 8f;
    
    private NetworkObject _tool;
    private Rigidbody _rigidbody;

    private bool _moving;
    
    // Bool to see if the grab button is pressed
    private bool _grabButtonPressed;
    // Bool to see if the specific tool button is pressed
    private bool _toolButtonPressed;

    private bool _canForcePull;
    
    
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
        if (!_grabButtonPressed || !_toolButtonPressed || transform.childCount > 1){ return; }
        if (_tool == null)
        {
            SpawnTool();
            return;
        }
        UseForce();
    }
    
    private void UseForce()
    {
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

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (_tool == null) return;
        
        if (transform.childCount > 1) return;
        
        if (!_canForcePull) return;
        
        if (_grabButtonPressed && _toolButtonPressed)
        {
            _tool.transform.position = Vector3.MoveTowards(_tool.transform.position, transform.position, Time.deltaTime * returnSpeed);
            _tool.transform.rotation = Quaternion.RotateTowards(_tool.transform.rotation, transform.rotation, Time.deltaTime * returnSpeed * 17);
            if (_moving) return;
            _moving = true;
            _rigidbody.useGravity = false;
        }
        else
        {
            if (!_moving) return;
            _moving = false;
            _rigidbody.useGravity = true;
        }
    }
    
    private async void SpawnTool()
    {
        if(_tool != null) return;
        _tool = Runner.Spawn(toolPrefab, transform.position, Quaternion.identity);
        _rigidbody = _tool.GetComponent<Rigidbody>();
        await Shared.GainStateAuthority(_tool);
        // _tool.transform.SetParent(transform);
        // _tool.transform.localPosition = Vector3.zero;
        // _tool.transform.localRotation = Quaternion.identity;
    }

    IEnumerator SetPull()
    {
        yield return new WaitForSeconds(3f);
        _canForcePull = false;
    }
}

using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerTriggerFSMEvent: MonoBehaviour
{
    [SerializeField] private PlayMakerFSM fsm = default;
        
    [UsedImplicitly]
    [SerializeField] 
    private bool shouldTriggerEnterEvent = default;
    [SerializeField]
    private string enterEvent = default;
    [UsedImplicitly]
    [SerializeField] 
    private bool shouldTriggerExitEvent = default;
    [SerializeField] 
    private string exitEvent = default;

    private void OnValidate()
    {
        this.GetComponent<Collider2D>().isTrigger = true;
    }
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PlayerTag))
            this.PlayerEntered();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PlayerTag))
            this.PlayerExited();
    }

    private void PlayerEntered()
    {
        if(this.shouldTriggerEnterEvent)
            this.fsm.SendEvent(this.enterEvent);
    }

    private void PlayerExited()
    {
        if(this.shouldTriggerExitEvent)
            this.fsm.SendEvent(this.exitEvent);
    }
}
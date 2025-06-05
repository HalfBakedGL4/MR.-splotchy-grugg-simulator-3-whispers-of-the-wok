using TMPro;
using UnityEngine;

public class S_SetTextOffline : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private S_Ticket ticket;

    private bool _isSet;

    private void Update()
    {
        if (_isSet) return;
        if (!ticket) return;
        if (ticket.GetTicketNumber() <= 0) return;
        numberText.text = $"#{ticket.GetTicketNumber()}";
        _isSet = true;
    }
}

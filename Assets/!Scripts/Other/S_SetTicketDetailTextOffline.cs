using TMPro;
using UnityEngine;

public class S_SetTicketDetailTextOffline : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private S_TicketDetailGiver ticketDetail;

    private bool _isSet;

    private void Update()
    {
        if (_isSet) return;
        if (!ticketDetail) return;
        if (ticketDetail.GetTicketID() <= 0) return;
        numberText.text = $"#{ticketDetail.GetTicketID()}";
        _isSet = true;
    }
}

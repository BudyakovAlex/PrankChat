namespace PrankChat.Mobile.Core.Models.Enums
{
    public enum OrderStatusType
    {
        New,
        Rejected,
        Cancelled,
        Active,
        InWork,
        InArbitration,
        ProcessCloseArbitration,
        ClosedAfterArbitrationCustomerWin,
        ClosedAfterArbitrationExecutorWin,
        Finished,
    }
}

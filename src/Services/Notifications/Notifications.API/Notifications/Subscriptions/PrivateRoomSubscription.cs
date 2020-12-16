namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class PrivateRoomSubscription : BaseSubscription
    {
        public PrivateRoomSubscription(int roomId)
            : base($":privateroom:{roomId}")
        { }
    }
}

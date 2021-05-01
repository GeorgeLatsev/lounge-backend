namespace Lounge.Services.Notifications.API.Notifications.Subscriptions
{
    public class RoomSubscription : BaseSubscription
    {
        public RoomSubscription(int roomId)
            : base($":room:{roomId}")
        { }
    }
}

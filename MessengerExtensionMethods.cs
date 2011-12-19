using IHI.Server.Habbos;
using IHI.Server.Libraries.Cecer1.Messenger;

namespace IHI.Server.Plugins.Cecer1.MessengerManager
{
    public static class MessengerExtensionMethods
    {
        public static MessengerObject GetMessenger(this IBefriendable friend)
        {
            return friend.GetInstanceVariable("Messenger.Instance") as MessengerObject;
        }
    }
}
using System;
using IHI.Server.Habbos;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public delegate void MessengerFriendEventHandler(object source, MessengerFriendEventArgs e);
    public class MessengerFriendEventArgs : EventArgs
    {
        #region Properties
        public Category Category
        {
            get;
            set;
        }
        public IBefriendable Friend
        {
            get;
            set;
        }
        public FriendUpdateType Type
        {
            get;
            set;
        }
        #endregion
    }
}
using System;
using IHI.Server.Habbos;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public delegate void MessengerFriendRequestEventHandler(object source, MessengerFriendRequestEventArgs e);
    public class MessengerFriendRequestEventArgs : EventArgs
    {
        private readonly IBefriendable _from;
        private readonly IBefriendable _to;

        public MessengerFriendRequestEventArgs(IBefriendable from, IBefriendable to)
        {
            _from = from;
            _to = to;
        }

        public IBefriendable GetFrom()
        {
            return _from;
        }

        public IBefriendable GetTo()
        {
            return _to;
        }
    }
}
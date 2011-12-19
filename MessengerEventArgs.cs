using System;
using IHI.Server.Habbos;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public delegate void MessengerEventHandler(object source, MessengerEventArgs e);
    public class MessengerEventArgs : EventArgs
    {
        private readonly Habbo _habbo;

        public MessengerEventArgs(Habbo habbo)
        {
            _habbo = habbo;
        }

        public Habbo GetHabbo()
        {
            return _habbo;
        }
    }
}
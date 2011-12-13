using System;
using IHI.Server.Habbos;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public delegate void MessengerEventHandler(object source, MessengerEventArgs e);
    
    public delegate void MessengerCategoryEventHandler(object source, MessengerCategoryEventArgs e);

    public delegate void MessengerFriendRequestEventHandler(object source, MessengerFriendRequestEventArgs e);

    public delegate void MessengerFriendEventHandler(object source, MessengerFriendEventArgs e);


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

    public class MessengerCategoryEventArgs : EventArgs
    {
        private readonly int _categoryID;
        private readonly string _newName;
        private readonly string _oldName;

        public MessengerCategoryEventArgs(int categoryID, string oldName, string newName)
        {
            _categoryID = categoryID;
            _oldName = oldName;
            _newName = newName;
        }


        /// <summary>
        /// Returns the ID of the Category that was effected by this event.
        /// </summary>
        public int GetCategoryID()
        {
            return _categoryID;
        }

        /// <summary>
        /// Returns the state of the Category before this event.
        /// This returns returns null is the category is new.
        /// </summary>
        public string GetOldName()
        {
            return _oldName;
        }

        /// <summary>
        /// Returns the name of the Category after this event.
        /// This returns null if the category was removed.
        /// </summary>
        public string GetNewName()
        {
            return _newName;
        }
    }

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

    public class MessengerFriendEventArgs : EventArgs
    {
        private readonly Category _category;
        private readonly IBefriendable _friend;
        private readonly FriendUpdateType _type;

        public MessengerFriendEventArgs(IBefriendable friend, FriendUpdateType type, Category category)
        {
            _friend = friend;
            _type = type;
            _category = category;
        }

        public IBefriendable GetFriend()
        {
            return _friend;
        }

        public FriendUpdateType GetUpdateType()
        {
            return _type;
        }

        public Category GetCategory()
        {
            return _category;
        }
    }

    public enum FriendUpdateType
    {
        Removed = -1,
        NoChange = 0,
        Added = 1
    }
}
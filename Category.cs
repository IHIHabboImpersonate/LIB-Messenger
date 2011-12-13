using System.Collections.Generic;
using System.Linq;
using IHI.Server.Habbos;
using IHIDB = IHI.Database;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class Category
    {
        private readonly int _categoryID;
        private readonly List<Friend> _friends;
        private readonly MessengerObject _messenger;

        private readonly Habbo _owner;
        private string _name;

        public Category(int categoryID, string name, MessengerObject messenger)
        {
            _categoryID = categoryID;
            _owner = messenger.GetOwner();
            _friends = new List<Friend>();
            _name = name;
            _messenger = messenger;
        }

        public int GetID()
        {
            return _categoryID;
        }

        public string GetName()
        {
            return _name;
        }

        public Habbo GetOwner()
        {
            return _owner;
        }

        public Category SetName(string name)
        {
            _name = name;
            return this;
        }

        public Category AddFriend(Friend friend)
        {
            if (!_friends.Contains(friend))
            {
                _messenger.RemoveFriendRequest(friend);
                _friends.Add(friend);

                var args = new MessengerFriendEventArgs(friend, FriendUpdateType.Added, this);
                _messenger.InvokeFriendStateChangedEvent(args);
            }
            return this;
        }

        public Category RemoveFriend(Friend friend)
        {
            if (_friends.Contains(friend))
            {
                _friends.Remove(friend);

                var args = new MessengerFriendEventArgs(friend, FriendUpdateType.Removed, this);
                _messenger.InvokeFriendStateChangedEvent(args);
            }
            return this;
        }

        public IEnumerable<Friend> GetFriends()
        {
            return _friends;
        }

        /// <summary>
        /// Checks this Category for an instance of IBefriendable EXACTLY matching Friend.
        /// </summary>
        /// <param name="friend">The The IBefriendable to search for.</param>
        /// <returns>True if a match was found, false otherwise.</returns>
        public bool IsFriend(IBefriendable friend)
        {
            return _friends.Any(f => f.Equals(friend));
        }

        /// <summary>
        /// Checks this Category for an IBefriendable with the ID matching FriendID.
        /// </summary>
        /// <param name="friendID">The ID to search for.</param>
        /// <returns>True if a matching ID was found, false otherwise.</returns>
        public bool IsFriend(int friendID)
        {
            return _friends.Any(f => f.GetID() == friendID);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using IHI.Database;
using IHI.Server.Habbos;
using IHIDB = IHI.Database;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class Category
    {
        private readonly int _categoryID;
        private readonly Dictionary<int, Friend> _friends;
        private readonly MessengerObject _messenger;

        private readonly Habbo _owner;
        private string _name;

        public Category(int categoryID, string name, MessengerObject messenger)
        {
            _categoryID = categoryID;
            _owner = messenger.GetOwner();
            _friends = new Dictionary<int, Friend>();
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
            if (!_friends.ContainsKey(friend.GetID()))
            {
                _messenger.RemoveFriendRequest(friend);
                _friends.Add(friend.GetID(), friend);

                var args = new MessengerFriendEventArgs
                               {
                                   Friend = friend,
                                   Category = this,
                                   Type = FriendUpdateType.Added
                               };
                _messenger.InvokeFriendStateChangedEvent(args);
            }
            return this;
        }
        public Category RemoveFriend(Friend friend)
        {
            if (_friends.ContainsKey(friend.GetID()))
            {
                _friends.Remove(friend.GetID());

                var args = new MessengerFriendEventArgs
                               {
                                   Category = this,
                                   Friend = friend,
                                   Type = FriendUpdateType.Removed
                               };
                _messenger.InvokeFriendStateChangedEvent(args);
            }
            return this;
        }
        public Category RemoveFriend(int friendID)
        {
            if (_friends.ContainsKey(friendID))
            {
                Friend removedFriend = _friends[friendID];
                _friends.Remove(friendID);

                var args = new MessengerFriendEventArgs
                               {
                                   Category = this,
                                   Friend = removedFriend,
                                   Type = FriendUpdateType.Removed
                               };

                _messenger.InvokeFriendStateChangedEvent(args);
                using (var db = CoreManager.ServerCore.GetDatabaseSession())
                {
                    db.Delete(db.Get<Database.MessengerFriendship>(friendID));
                }
            }
            return this;
        }

        public IEnumerable<Friend> GetFriends()
        {
            return _friends.Values;
        }
        public Friend GetFriend(int friendID)
        {
            if(_friends.ContainsKey(friendID))
                return _friends[friendID];
            return null;
        }
        public Friend GetFriend(IBefriendable friend)
        {
            if (_friends.ContainsKey(friend.GetID()))
                return _friends[friend.GetID()];
            return null;
        }

        /// <summary>
        /// Checks this Category for an instance of IBefriendable EXACTLY matching Friend.
        /// </summary>
        /// <param name="friend">The The IBefriendable to search for.</param>
        /// <returns>True if a match was found, false otherwise.</returns>
        public bool IsFriend(IBefriendable friend)
        {
            return _friends.ContainsKey(friend.GetID());
        }

        /// <summary>
        /// Checks this Category for an IBefriendable with the ID matching FriendID.
        /// </summary>
        /// <param name="friendID">The ID to search for.</param>
        /// <returns>True if a matching ID was found, false otherwise.</returns>
        public bool IsFriend(int friendID)
        {
            return _friends.ContainsKey(friendID);
        }
    }
}
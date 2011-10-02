using System.Collections.Generic;
using System.Linq;
using IHI.Server.Habbos;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class MessengerObject
    {
        #region Events

        public event MessengerBlockFlagEventHandler OnMessengerBlockFlagChanged;
        public event MessengerCategoryEventHandler OnMessengerCategoryChanged;
        public event MessengerFriendRequestEventHandler OnMessengerFriendRequestReceived;
        public event MessengerFriendRequestEventHandler OnMessengerFriendRequestSent;
        public event MessengerFriendEventHandler OnMessengerFriendStateChanged;

        #endregion

        #region Fields

        private readonly IDictionary<int, Category> _categories;
        private readonly ICollection<IBefriendable> _friendRequests;
        private readonly Habbo _owner;
        private BlockFlag _blockFlags;

        #endregion

        #region Constructors

        public MessengerObject(Habbo owner)
        {
            _owner = owner;
            _blockFlags = BlockFlag.None;
            _categories = new Dictionary<int, Category>();
            _friendRequests = new List<IBefriendable>();

            if (owner.GetPersistantVariable("Messenger.StalkBlock") == null)
                _blockFlags |= BlockFlag.Stalk;
            if (owner.GetPersistantVariable("Messenger.RequestBlock") == null)
                _blockFlags |= BlockFlag.Request;
            if (owner.GetPersistantVariable("Messenger.InviteBlock") == null)
                _blockFlags |= BlockFlag.Invite;
        }

        public MessengerObject(Habbo owner, bool isStalkable, bool isRequestable, bool isInviteable)
        {
            _owner = owner;
            _blockFlags = BlockFlag.None;
            _categories = new Dictionary<int, Category>();
            _friendRequests = new List<IBefriendable>();

            if (!isStalkable)
                _blockFlags |= BlockFlag.Stalk;
            if (!isRequestable)
                _blockFlags |= BlockFlag.Request;
            if (!isInviteable)
                _blockFlags |= BlockFlag.Invite;
        }

        #endregion

        #region Methods

        public Habbo GetOwner()
        {
            return _owner;
        }

        public ICollection<Friend> GetAllFriends()
        {
            var friends = new HashSet<Friend>();

            foreach (var category in _categories.Values)
            {
                friends.UnionWith(category.GetFriends());
            }
            return friends;
        }

        /// <summary>
        /// Checks all Categories for an instance of IBefriendable EXACTLY matching Friend.
        /// </summary>
        /// <param name="friend">The The IBefriendable to search for.</param>
        /// <returns>True if a match was found, false otherwise.</returns>
        public bool IsFriend(IBefriendable friend)
        {
            return _categories.Values.Any(c => c.IsFriend(friend));
        }

        /// <summary>
        /// Checks all Categories for an IBefriendable with the ID matching FriendID.
        /// </summary>
        /// <param name="friendID">The ID to search for.</param>
        /// <returns>True if a matching ID was found, false otherwise.</returns>
        public bool IsFriend(int friendID)
        {
            return _categories.Values.Any(c => c.IsFriend(friendID));
        }

        internal void InvokeFriendStateChangedEvent(MessengerFriendEventArgs e)
        {
            if (OnMessengerFriendStateChanged != null)
                OnMessengerFriendStateChanged.Invoke(this, e);
        }

        #region Block Flags

        public bool IsStalkable()
        {
            return (_blockFlags & BlockFlag.Stalk) == BlockFlag.Stalk;
        }

        public bool IsRequestable()
        {
            return (_blockFlags & BlockFlag.Request) == BlockFlag.Request;
        }

        public bool IsInviteable()
        {
            return (_blockFlags & BlockFlag.Invite) == BlockFlag.Invite;
        }

        public MessengerObject SetStalkable(bool value)
        {
            if (OnMessengerBlockFlagChanged != null)
            {
                var args = new MessengerBlockFlagEventArgs(BlockFlag.Stalk, IsStalkable(), value);
                OnMessengerBlockFlagChanged.Invoke(this, args);
            }


            if (value)
                _blockFlags |= BlockFlag.Stalk;
            else
                _blockFlags &= ~BlockFlag.Stalk;

            return this;
        }

        public MessengerObject SetRequestable(bool value)
        {
            if (OnMessengerBlockFlagChanged != null)
            {
                var args = new MessengerBlockFlagEventArgs(BlockFlag.Request, IsRequestable(), value);
                OnMessengerBlockFlagChanged.Invoke(this, args);
            }


            if (value)
                _blockFlags |= BlockFlag.Request;
            else
                _blockFlags &= ~BlockFlag.Request;
            return this;
        }

        public MessengerObject SetInviteable(bool value)
        {
            if (OnMessengerBlockFlagChanged != null)
            {
                var args = new MessengerBlockFlagEventArgs(BlockFlag.Invite, IsInviteable(), value);
                OnMessengerBlockFlagChanged.Invoke(this, args);
            }


            if (value)
                _blockFlags |= BlockFlag.Invite;
            else
                _blockFlags &= ~BlockFlag.Invite;
            return this;
        }

        #endregion

        #region Categories

        public MessengerObject SetCategory(int id, Category category)
        {
            string oldName = null;
            string newName = null;

            if (category != null)
            {
                newName = category.GetName();
                if (!_categories.ContainsKey(id))
                    _categories.Add(id, category);
                else
                {
                    oldName = _categories[id].GetName();
                    _categories[id] = category;
                }
            }
            else if (_categories.ContainsKey(id))
                _categories.Remove(id);


            if (OnMessengerCategoryChanged != null)
            {
                var args = new MessengerCategoryEventArgs(id, oldName, newName);
                OnMessengerCategoryChanged.Invoke(this, args);
            }

            return this;
        }

        public Category GetCategory(int? id)
        {
            if (id == null)
                return _categories[0];

            if (_categories.ContainsKey((int) id))
                return _categories[(int) id];
            return null;
        }

        public ICollection<Category> GetAllCategories()
        {
            return _categories.Values;
        }

        #endregion

        #region Requests

        public bool IsRequestedBy(IBefriendable sender)
        {
            return _friendRequests.Contains(sender);
        }

        public MessengerObject AddFriendRequest(IBefriendable from)
        {
            if (_friendRequests.Contains(from))
                return this;

            if (OnMessengerFriendRequestReceived != null)
            {
                var args = new MessengerFriendRequestEventArgs(from);
                OnMessengerFriendRequestReceived.Invoke(this, args);
            }
            _friendRequests.Add(from);
            return this;
        }

        #endregion

        #endregion
    }
}
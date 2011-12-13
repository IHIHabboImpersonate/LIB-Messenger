using System.Collections.Generic;
using System.Linq;
using IHI.Server.Habbos;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class MessengerObject
    {
        #region Events
        public event MessengerCategoryEventHandler OnCategoryChanged;
        public event MessengerFriendRequestEventHandler OnFriendRequestReceived;
        public event MessengerFriendRequestEventHandler OnFriendRequestRemoved;
        public event MessengerFriendEventHandler OnFriendStateChanged;

        #endregion

        #region Fields

        private readonly IDictionary<int, Category> _categories;
        private readonly ICollection<IBefriendable> _friendRequests;
        private readonly Habbo _owner;

        #endregion

        #region Constructors

        public MessengerObject(Habbo owner)
        {
            _owner = owner;
            _categories = new Dictionary<int, Category>();
            _friendRequests = new List<IBefriendable>();

            if (owner.GetPersistantVariable("Messenger.StalkBlock") != null)
                owner.BlockStalking = true;
            if (owner.GetPersistantVariable("Messenger.RequestBlock") != null)
                owner.BlockRequests = true;
            if (owner.GetPersistantVariable("Messenger.InviteBlock") != null)
                owner.BlockInvites = true;
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
            if (OnFriendStateChanged != null)
                OnFriendStateChanged.Invoke(this, e);
        }
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


            if (OnCategoryChanged != null)
            {
                var args = new MessengerCategoryEventArgs(id, oldName, newName);
                OnCategoryChanged.Invoke(this, args);
            }

            return this;
        }

        public Category GetCategory(Database.MessengerCategory category)
        {
            if (category == null)
                return GetCategory(0);
            return GetCategory(category.category_id);
        }
        public Category GetCategory(int id)
        {
            if (_categories.ContainsKey(id))
                return _categories[id];
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

        /// <summary>
        /// Adds a friend request to the messenger.
        /// </summary>
        /// <param name="from">The IBefriendable the friend request is from.</param>
        /// <returns></returns>
        public MessengerObject NotifyFriendRequest(IBefriendable from)
        {
            if (_friendRequests.Contains(from))
                return this;

            if (OnFriendRequestReceived != null)
            {
                var args = new MessengerFriendRequestEventArgs(from, _owner);
                OnFriendRequestReceived.Invoke(this, args);
            }

            _friendRequests.Add(from);
            return this;
        }
        public MessengerObject RemoveFriendRequest(IBefriendable from)
        {
            if (OnFriendRequestRemoved != null)
            {
                var args = new MessengerFriendRequestEventArgs(from, _owner);
                OnFriendRequestRemoved.Invoke(this, args);

            }
            _friendRequests.Remove(from);
            return this;
        }
        #endregion
        #endregion
    }
}
#region GPLv3

// 
// Copyright (C) 2012  Chris Chenery
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

#endregion

#region Usings

using System.Collections.Generic;
using IHI.Server.Habbos;
using MessengerManager;

#endregion

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

        #region Properties

        private readonly IDictionary<int, Category> _categories;
        private readonly IDictionary<int, IBefriendable> _friendRequests;
        private readonly IDictionary<int, Friend> _friends;

        public Habbo Owner { get; private set; }

        #endregion

        #region Constructors

        public MessengerObject(Habbo owner)
        {
            _categories = new Dictionary<int, Category>();
            _friends = new Dictionary<int, Friend>();
            _friendRequests = new Dictionary<int, IBefriendable>();

            Owner = owner;

            // TODO: Fix at later date
            /*
            if (Owner.GetPersistantVariable("Messenger.StalkBlock") != null)
                Owner.BlockStalking = true;
            if (Owner.GetPersistantVariable("Messenger.RequestBlock") != null)
                Owner.BlockRequests = true;
            if (Owner.GetPersistantVariable("Messenger.InviteBlock") != null)
                Owner.BlockInvites = true;
             */
        }

        #endregion

        #region Methods

        #region Friends

        /// <summary>
        ///   Returns an IEnumerable of type Friend containing all Friends in all Categories.
        /// </summary>
        public IEnumerable<Friend> GetAllFriends()
        {
            return _friends.Values;
        }

        /// <summary>
        ///   Check if an IBefriendable is already an friend.
        /// </summary>
        /// <param name = "befriendable">The IBefriendable to check for.</param>
        /// <returns>True if befriendable is already a friend, false otherwise.</returns>
        public bool IsFriend(IBefriendable befriendable)
        {
            return _friends.ContainsKey(befriendable.GetID());
        }

        /// <summary>
        ///   Get a Friend from an IBefriendable.
        /// </summary>
        /// <param name = "befriendable">The IBefriendable to return the Friend instance for.</param>
        /// <returns>The instance of Friend for befriendable. If befriendable is not a friend then null is returned.</returns>
        public Friend GetFriend(IBefriendable befriendable)
        {
            if (!_friends.ContainsKey(befriendable.GetID()))
                return null;
            return _friends[befriendable.GetID()];
        }

        /// <summary>
        ///   Add an IBefriendable as a friend only if the IBefriendable is not already a friend.
        /// </summary>
        /// <param name = "befriendable">The IBefriendable to add.</param>
        /// <returns>The instance of MessengerObject this was called on. This allows for chaining.</returns>
        public MessengerObject AddNewFriend(IBefriendable befriendable)
        {
            if (_friends.ContainsKey(befriendable.GetID()))
                return this;

            Friend friend = new Friend(befriendable);
            _friends.Add(befriendable.GetID(), friend);

            return this;
        }

        public MessengerObject RemoveFriend(Friend friend)
        {
            RemoveFriend(friend.Befriendable.GetID());
            return this;
        }

        public MessengerObject RemoveFriend(IBefriendable befriendable)
        {
            RemoveFriend(befriendable.GetID());
            return this;
        }

        public MessengerObject RemoveFriend(int friendID)
        {
            if (!_friends.ContainsKey(friendID))
                return this;

            Friend friend = _friends[friendID];
            foreach (Category category in friend.GetCategories())
            {
                RemoveFriendFromCategory(friend, category);
            }
            return this;
        }

        #endregion

        #region Friend-Category

        public MessengerObject AddFriendToCategory(Friend friend, Category category)
        {
            AddFriendToCategory(friend.Befriendable, category);
            return this;
        }

        public MessengerObject AddFriendToCategory(IBefriendable befriendable, Category category)
        {
            if (!_friends.ContainsKey(befriendable.GetID()))
                return this;

            Friend friend = _friends[befriendable.GetID()];

            if (!friend.Categories.ContainsKey(category.ID))
                friend.Categories.Add(category.ID, category);

            if (!category.Friends.ContainsKey(befriendable.GetID()))
                category.Friends.Add(befriendable.GetID(), befriendable);
            return this;
        }

        public MessengerObject RemoveFriendFromCategory(Friend friend, Category category)
        {
            RemoveFriendFromCategory(friend.Befriendable.GetID(), category);
            return this;
        }

        public MessengerObject RemoveFriendFromCategory(IBefriendable befriendable, Category category)
        {
            RemoveFriendFromCategory(befriendable.GetID(), category);
            return this;
        }

        public MessengerObject RemoveFriendFromCategory(int friendID, Category category)
        {
            if (category.Friends.ContainsKey(friendID))
                category.Friends.Remove(friendID);
            return this;
        }

        #endregion

        #region Categories

        /// <summary>
        ///   Returns an IEnumerable of type Category containing all Categories.
        /// </summary>
        public IEnumerable<Category> GetAllCategories()
        {
            return _categories.Values;
        }

        /// <summary>
        ///   Get a Category from an ID.
        /// </summary>
        /// <param name = "id">The ID of the Category to get.</param>
        /// <returns>The Category instance for ID. If the Category is not part of this MessengerObject instance then null is returned.</returns>
        public Category GetCategory(int id)
        {
            if (_categories.ContainsKey(id))
                return _categories[id];
            return null;
        }

        /// <summary>
        ///   Get all Categories with a specific name.
        /// </summary>
        /// <param name = "name">The name of the Categories.</param>
        /// <returns>An IEnumerable of type Category containing all the categories with the given name.</returns>
        public IEnumerable<Category> GetCategories(string name)
        {
            foreach (Category category in _categories.Values)
            {
                if (category.Name == name)
                    yield return category;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name = "category"></param>
        /// <returns></returns>
        public MessengerObject AddCategory(Category category)
        {
            if (_categories.ContainsKey(category.ID))
                return this;
            _categories.Add(category.ID, category);
            return this;
        }

        public MessengerObject RemoveCategory(Category category)
        {
            RemoveCategory(category.ID);
            return this;
        }

        public MessengerObject RemoveCategory(int id)
        {
            if (!_categories.ContainsKey(id))
                return this;
            _categories.Remove(id);
            return this;
        }

        #endregion

        #region Friend Requests

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBefriendable> GetAllFriendRequests()
        {
            return _friendRequests.Values;
        }

        /// <summary>
        ///   Check if there is an outstanding request from an IBefriendable.
        /// </summary>
        /// <param name = "befriendable">The IBefriendable to check for requests from.</param>
        /// <returns>True if befriendable is already a friend, false otherwise.</returns>
        public bool IsRequestedBy(IBefriendable befriendable)
        {
            return _friendRequests.ContainsKey(befriendable.GetID());
        }

        ///<summary>
        ///</summary>
        ///<param name = "befriendable"></param>
        public MessengerObject ReceiveFriendRequest(IBefriendable befriendable)
        {
            if (!_friendRequests.ContainsKey(befriendable.GetID()))
                return this;

            _friendRequests.Add(befriendable.GetID(), befriendable);

            if (OnFriendRequestReceived == null)
                OnFriendRequestReceived.Invoke(this, new MessengerFriendRequestEventArgs(befriendable, Owner));

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name = "befriendable"></param>
        /// <returns></returns>
        public MessengerObject RemoveFriendRequest(IBefriendable befriendable)
        {
            if (!_friendRequests.ContainsKey(befriendable.GetID()))
                return this;

            _friendRequests.Remove(befriendable.GetID());

            if (OnFriendRequestRemoved == null)
                OnFriendRequestRemoved.Invoke(this, new MessengerFriendRequestEventArgs(befriendable, Owner));

            return this;
        }

        public MessengerObject SendFriendRequest(IBefriendable befriendable)
        {
            MessengerObject messenger = befriendable.GetMessenger();
            if (messenger != null)
                messenger.ReceiveFriendRequest(Owner);
            return this;
        }

        #endregion

        #region Events

        internal void InvokeFriendStateChanged(MessengerFriendEventArgs e)
        {
            if (OnFriendStateChanged != null)
                OnFriendStateChanged.Invoke(this, e);
        }

        #endregion

        #endregion
    }
}
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
using System.Collections.Generic;
using IHI.Server.Habbos;
using IHIDB = IHI.Database;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class Category
    {
        #region Properties

        public MessengerObject Owner { get; private set; }
        public int ID { get; private set; }
        public string Name { get; set; }

        internal IDictionary<int, IBefriendable> Friends { get; set; }

        #endregion

        #region Constructors

        public Category(MessengerObject owner, int id)
        {
            Owner = owner;
            ID = id;
            Friends = new Dictionary<int, IBefriendable>();
        }

        #endregion

        #region Methods

        public Category AddFriend(Friend friend)
        {
            Owner.AddFriendToCategory(friend, this);
            return this;
        }

        public Category AddFriend(IBefriendable befriendable)
        {
            Owner.AddFriendToCategory(befriendable, this);
            return this;
        }

        public Category RemoveFriend(IBefriendable befriendable)
        {
            Owner.RemoveFriendFromCategory(befriendable, this);
            return this;
        }

        public Category RemoveFriend(Friend friend)
        {
            Owner.RemoveFriendFromCategory(friend, this);
            return this;
        }

        #endregion
    }
}
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

#endregion

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class Friend
    {
        #region Properties

        public IBefriendable Befriendable { get; private set; }
        internal IDictionary<int, Category> Categories { get; private set; }

        #endregion

        #region Constructors

        public Friend(IBefriendable befriendableObject)
        {
            Befriendable = befriendableObject;
            Categories = new Dictionary<int, Category>();
        }

        #endregion

        #region Methods

        public IEnumerable<Category> GetCategories()
        {
            return Categories.Values;
        }

        public Friend AddFriend(Category category)
        {
            category.Owner.AddFriendToCategory(this, category);
            return this;
        }

        public Friend RemoveFromCategory(Category category)
        {
            category.Owner.RemoveFriendFromCategory(this, category);
            return this;
        }

        #endregion
    }
}
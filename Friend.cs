using System;
using System.Collections.Generic;
using IHI.Server.Habbos;
using IHI.Server.Rooms;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class Friend
    {
        #region Properties
        public IBefriendable Befriendable
        {
            get;
            private set;
        }
        internal IDictionary<int, Category> Categories
        {
            get;
            private set;
        }
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
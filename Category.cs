using System.Collections.Generic;
using System.Linq;
using IHI.Database;
using IHI.Server.Habbos;
using IHIDB = IHI.Database;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public class Category
    {
        #region Properties
        public MessengerObject Owner
        {
            get;
            private set;
        }
        public int ID
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            set;
        }

        internal IDictionary<int, IBefriendable> Friends
        {
            get;
            set;
        }
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
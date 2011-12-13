using System;
using IHI.Server.Habbos;
using IHI.Server.Rooms;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public struct Friend : IBefriendable
    {
        private readonly IBefriendable _befriendableObject;

        public int Category
        {
            get;
            internal set;
        }

        public Friend(IBefriendable befriendableObject, int category) : this()
        {
            _befriendableObject = befriendableObject;
            Category = category;
        }

        #region IBefriendable Methods

        public event MessengerBlockFlagEventHandler OnBlockFlagChanged;

        public int GetID()
        {
            return _befriendableObject.GetID();
        }

        public bool IsLoggedIn()
        {
            return _befriendableObject.IsLoggedIn();
        }

        public string GetDisplayName()
        {
            return _befriendableObject.GetDisplayName();
        }

        public string GetMotto()
        {
            return _befriendableObject.GetMotto();
        }

        public DateTime GetLastAccess()
        {
            return _befriendableObject.GetLastAccess();
        }

        public IFigure GetFigure()
        {
            return _befriendableObject.GetFigure();
        }

        public Room GetRoom()
        {
            return _befriendableObject.GetRoom();
        }

        /// <summary>
        /// Gets/Sets the stalk block flag of the IBefriendable.
        /// </summary>
        public bool BlockStalking { get; set; }

        /// <summary>
        /// Gets/Sets the request block flag of the IBefriendable.
        /// </summary>
        public bool BlockRequests { get; set; }

        /// <summary>
        /// Gets/Sets the invite block flag of the IBefriendable.
        /// </summary>
        public bool BlockInvites { get; set; }

        /// <summary>
        /// Returns true if the IBefriendable can be requested, false otherwise.
        /// </summary>
        public bool IsStalkable()
        {
            if (GetRoom() == null)
                return false;
            return !BlockStalking;
        }

        /// <summary>
        /// Returns true if the IBefriendable can be requested, false otherwise.
        /// </summary>
        public bool IsRequestable()
        {
            return !BlockRequests;
        }

        /// <summary>
        /// Returns true if the IBefriendable can be invited, false otherwise.
        /// </summary>
        public bool IsInviteable()
        {
            return !BlockInvites;
        }

        /// <summary>
        /// Warning: This acts on the underlying IBefriendable.
        /// </summary>
        public object GetInstanceVariable(string name)
        {
            return _befriendableObject.GetInstanceVariable(name);
        }

        /// <summary>
        /// Warning: This acts on the underlying IBefriendable.
        /// </summary>
        public IInstanceVariables SetInstanceVariable(string name, object value)
        {
            _befriendableObject.SetInstanceVariable(name, value);
            return this;
        }

        /// <summary>
        /// Warning: This acts on the underlying IBefriendable.
        /// </summary>
        public string GetPersistantVariable(string name)
        {
            return _befriendableObject.GetPersistantVariable(name);
        }

        /// <summary>
        /// Warning: This acts on the underlying IBefriendable.
        /// </summary>
        public IPersistantVariables SetPersistantVariable(string name, string value)
        {
            _befriendableObject.SetPersistantVariable(name, value);
            return this;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //        return false;
        //    if (ReferenceEquals(this, obj))
        //        return base.Equals(obj); // No unintended recursion.

        //    return base.Equals(obj) || fBefriendableObject.Equals(obj);
        //}

        #endregion
    }
}
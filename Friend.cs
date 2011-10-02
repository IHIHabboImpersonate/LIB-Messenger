using System;
using IHI.Server.Habbos;
using IHI.Server.Rooms;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public struct Friend : IBefriendable
    {
        private readonly IBefriendable _befriendableObject;
        private readonly bool _isStalkingAllowed;
        private int _category;

        public Friend(IBefriendable befriendableObject, int category, bool isStalkingAllowed = true)
        {
            _befriendableObject = befriendableObject;
            _isStalkingAllowed = isStalkingAllowed;
            _category = category;
        }

        public int GetCategory()
        {
            return _category;
        }

        internal Friend SetCategory(int categoryID)
        {
            _category = categoryID;
            return this;
        }

        /// <summary>
        /// Returns true if the IBefriendable allows stalking, false otherwise.
        /// </summary>
        public bool IsStalkingAllowed()
        {
            return _isStalkingAllowed;
        }

        #region IBefriendable Methods

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
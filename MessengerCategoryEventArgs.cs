using System;

namespace IHI.Server.Libraries.Cecer1.Messenger
{
    public delegate void MessengerCategoryEventHandler(object source, MessengerCategoryEventArgs e);
    public class MessengerCategoryEventArgs : EventArgs
    {
        private readonly int _categoryID;
        private readonly string _newName;
        private readonly string _oldName;

        public MessengerCategoryEventArgs(int categoryID, string oldName, string newName)
        {
            _categoryID = categoryID;
            _oldName = oldName;
            _newName = newName;
        }


        /// <summary>
        /// Returns the ID of the Category that was effected by this event.
        /// </summary>
        public int GetCategoryID()
        {
            return _categoryID;
        }

        /// <summary>
        /// Returns the state of the Category before this event.
        /// This returns returns null is the category is new.
        /// </summary>
        public string GetOldName()
        {
            return _oldName;
        }

        /// <summary>
        /// Returns the name of the Category after this event.
        /// This returns null if the category was removed.
        /// </summary>
        public string GetNewName()
        {
            return _newName;
        }
    }
}
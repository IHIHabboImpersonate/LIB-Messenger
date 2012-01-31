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
        ///   Returns the ID of the Category that was effected by this event.
        /// </summary>
        public int GetCategoryID()
        {
            return _categoryID;
        }

        /// <summary>
        ///   Returns the state of the Category before this event.
        ///   This returns returns null is the category is new.
        /// </summary>
        public string GetOldName()
        {
            return _oldName;
        }

        /// <summary>
        ///   Returns the name of the Category after this event.
        ///   This returns null if the category was removed.
        /// </summary>
        public string GetNewName()
        {
            return _newName;
        }
    }
}
/*
 * Copyright (c) Cenium AS. All Right Reserved
 *
 * This source is subject to the Cenium License.
 * Please see the License.txt file for more information.
 * All other rights reserved.
 * 
 * http://www.cenium.com
 * 
 * Change History:
 * 
 * User        Date          Comment
 * ----------- ------------- --------------------------------------------------------------------------------------------
 * Thilini.Y   11/24/2021    Created
 */


using Cenium.Framework.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cenium.Reservations.Data
{
    /// <summary>
    /// Explain the purpose of the class here
    /// </summary>
    [EntityInfo]
    public partial class Reservation
    {

        private string _name;
        private string _idnumber;

        /// <summary>
        /// Not mapped queryable field to query by conference group number.
        /// </summary>
        [NotMapped]
        [EntityMember(IsReadOnly = false, IsSortable = false, IsQueryable = true, Order = 101)]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [NotMapped]
        [EntityMember(IsReadOnly = false, IsSortable = false, IsQueryable = true, Order = 102)]
        public virtual string IdNumber
        {
            get { return _idnumber; }
            set { _idnumber = value; }
        }



    }

}

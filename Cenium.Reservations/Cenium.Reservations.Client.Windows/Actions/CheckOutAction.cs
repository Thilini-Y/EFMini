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
 * Thilini.Y   12/30/2021    Created
 */


using Cenium.Framework.Client;
using Cenium.Framework.Client.AppResources.UI;
using Cenium.Framework.Client.Model;
using Cenium.Framework.Client.Windows;
using Cenium.Framework.Client.Windows.Pages;
using Cenium.Framework.Client.Windows.Pages.Actions;
using Cenium.Framework.ComponentModel;
using System;
using System.ComponentModel;
using System.Windows;

namespace Cenium.Reservations.Client.Windows.Actions
{
    /// <summary>
    /// Explain the purpose of the class here
    /// </summary>
    [RegisterActionType("frontdesk.checkoutaction")]
    public class CheckOutAction : AbstractActionHandler
    {
        private Record _record = null;

        /// <summary>
        /// Initializes a new instance of the CheckInAction class
        /// </summary>
        public CheckOutAction()
        {
            IsActionEnabled = true;
            IsActionItemRequired = true;
        }

        /// <summary>
        /// Evaluates the conditions when the ActionItem property is set.
        /// </summary>
        /// <returns>true if the conditions are fullfilled; otherwise false</returns>
        protected override bool EvaluateConditions()
        {
            bool test = base.EvaluateConditions();
            if (test)
            {
                if (ClientConnector.Current.CheckUserAccess("Reservations/Reservation/CheckOut"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Action to execute when generating prices.
        /// </summary>
        protected override void OnExecute()
        {
            var rec = GetRecord();


            if (rec == null)
            {
                MessageBox.Show("Unable to find record.");
                return;
            }
            

            else if (rec.State == RecordState.Added || rec.IsDirty)
            {
                MessageBox.Show("Please save changes.");
                return;
            }


            else if (rec["Status"].ToString() != "CheckedIn") 
            {
                MessageBox.Show("You can only check out reservations in status Confirm.");
                return;
            }


            else if (rec["Name"] == null)
            {
                MessageBox.Show("Please add a primary guest before checking out.");
                return;
            }


            else if (rec["RoomTypeName"] == null)
            {
                MessageBox.Show("The reservation does not have a room type so no room can be assigned.");
                return;
            }

            else if (rec["RoomNumber"] == null)
            {
                MessageBox.Show("The reservation does not have a room  so can not check out.");
                return;
            }



            else
            {
                var executingtitlemessage = "Checking out. Please wait...";
                string executingMessageTitle = executingtitlemessage;
                string executingMessage = "Checking out All Shares...";
                CheckInImpl(rec, executingMessageTitle, executingMessage);
            }

        }


        private void CheckInImpl(Record request, string executingMessageTitle, string executingMessage)
        {
            WindowManager.ShowPageProgress(Owner, executingMessageTitle, executingMessage);
            try
            {
                ClientConnector.Current.InvokeAsync(new ServiceInvokeParameters
                {
                    ServiceMethod = "Reservations/Reservation/CheckOut",
                    Record = request,
                    IncludeChildren = true,
                }, Finished);
            }
            catch (Exception ex)
            {
                WindowManager.HidePageProgress(Owner);



                MessageBox.Show(string.Format("{0}\n{1}\n{2}", "Error occured when doing Check Out.",
                    "Error message: ", ex.Message),
                    "Check In Failed");
            }
        }

        private void Finished(ServiceOperationResult result)
        {
            WindowManager.HidePageProgress(Owner);
            if (!result.IsError)
            {
                //Merge Record From CheckOut.
                Record record = GetRecord();
                if (record == null)
                {
                    return;
                }
                var newRecord = result.Result as Record;

                if (newRecord != null)
                {

                    record.Merge(newRecord, true);
                    record.State = newRecord.State;

                }
            }
            else
            {
                EventDispatchManager.ExecuteOnUIThread(
                    (Action)delegate ()
                    {
                        MessageBox.Show(string.Format("{0}\n{1}\n{2}", "An error occured when doing check in.",
                            "Error message: ", result.Error.Message),
                            "Check In did not Complete.", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
            }
            Invalidate();

        }


        protected virtual RecordModel GetPageModel()
        {
            var page = Owner as CListPage;

            if ((page == null) || (page.PageModel == null))
                return null;

            return page.PageModel.Data as RecordModel;
        }


        protected Record GetRecord()
        {
            if (Owner == null)
                return null;

            if (ActionItem != null)
            {
                var rec = ActionItem as RecordItem;
                return rec.Item;
            }
            return null;
        }

       


    }

}

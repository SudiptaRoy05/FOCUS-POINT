using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Utility
{
    public static class SD
    {
        public const string Role_User = "User";
        public const string Role_Photographer = "Photographer";
        public const string Role_Admin = "Admin";


        public const string StatusPending = "Pending";
        public const string StatusApprove = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusCompleteEvent  = "CompleteEvent";
        public const string StatusCancelled = "Cancelled";
        public const string StatusCompleted = "Completed";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
     
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejectrd = "Rejected";

		public const string SessionHire = "SessionHire";

        public const string SessionCart = "SessitonShoppingCart";
	}
}

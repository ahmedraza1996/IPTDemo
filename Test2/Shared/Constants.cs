using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test2.Shared
{
    public class Constants
    {
        public const int ZERO = 0;

        public const string STR_YES = "Y";
        public const string STR_NO = "N";

        public const string SESSION_USER = "SessionUser";
        public const string SESSION_ADMIN = "SessionAdmin";
        public const string SESSION_CART = "SessionCart";
        public static DDList MSG_SESSION_USER_EMPTY = new DDList("sessionUserEmpty", "No user seems to be signed in! ");

        public static DDList MSG_SESSION_USER_UNV = new DDList("sessionUserUnverified", "User is not yet verified! ");
        public static DDList MSG_SESSION_USER_INC_PROFILE = new DDList("sessionUserIncProfile", "The profile seems to be incomplete! ");
        public static DDList MSG_ALREADY_APPLIED = new DDList("AlreadyApplied", "You have already applied for this job opening! ");
        public static DDList MSG_POS_CLOSED = new DDList("PositionClosed", "This job opening is now closed! ");
        public static DDList MSG_SUCCESS = new DDList("success", "Successfully completed! ");
        public static DDList MSG_ERROR = new DDList("error", "Errmm... There seems to be an error! ");
        public static DDList MSG_ERR_INVALIDCRED = new DDList("InvalidCredentials", "The username or password seems to be incorrect ");
        public static DDList MSG_ERR_NOUSEREXIST = new DDList("UserDoesnotExist", "The provided username/email is not listed in our system! ");
        public static DDList MSG_ERR_SERVER = new DDList("DBError", "Now this is embarrassing... there seems to be a problem! ");
        public static DDList MSG_ERR_DBSAVE = new DDList("Unable to save, Please contact ISD", "Unable to retrieve or save record in the database. ");
        public static DDList MSG_OK_DBSAVE = new DDList("OK", "Record successfully saved / retrieved from the database");
        public const string DATE_RFC_FORMAT = "yyyy-MM-dd";
        public const string DATE_MONTH_YEAR_FORMAT = "MMMM yyyy";
        public const string DATE_DISP_TAB_FORMAT = "MMMM dd, yyyy HH:mm";
        public const string DATE_RFC_YEAR_MONTH_FORMAT = "yyyy-MM";


        public class DDList
        {
            public DDList(string Text, string Value)
            {
                this.Text = Text;
                this.Value = Value;
            }
            public string Text { get; set; }
            public string Value { get; set; }
            public bool isSelected { get; set; }
        }
    }
}
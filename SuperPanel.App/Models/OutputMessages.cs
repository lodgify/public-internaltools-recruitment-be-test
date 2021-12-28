using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.Models
{
    public class OutputMessages
    {
        //Ouput messages for user
        public static string alert_message = "Internal Error. Please try again and contact Support.";
        public static string user_not_found = "User not found. Deletion not possible.";
        public static string sucess = "Sucessfully deleted User.";
        public static string general_error = "Unknown error deleting user. Contact Support.";
    }
}

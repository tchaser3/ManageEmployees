/* Title:           Logon
 * Date:            6-13-16
 * Author:          Terry Holmes
 *
 * Description:     This form is used for logon on */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagesDLL;
using EmployeeDLL;
using DataValidationDLL;
using EventLogDLL;
using LastTransactionDLL;

namespace ManageEmployees
{
    public partial class Logon : Form
    {
        //setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        LastTransactionClass TheLastTransactionClass = new LastTransactionClass();

        //setting public variables
        //setting modular varaiables
        public static string mstrErrorMessage;
        public static string mstrFirstName;
        public static string mstrLastName;
        public static int mintEmployeeIDLoggedIn;
        public static int mintEmployeeID;
        public static string mstrGroup;
        int mintNumberOfMisses;

        public Logon()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //closing the program
            TheMessagesClass.CloseTheProgram();

        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            bool blnLogonVerified = false;
            string strMessageForUser;

            //beginning data validation
            strValueForValidation = txtEmployeeID.Text;
            mstrLastName = txtLogonLastName.Text;
            mstrErrorMessage = "";

            blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
            if(blnFatalError == true)
            {
                blnThereIsAProblem = true;
                mstrErrorMessage = mstrErrorMessage + "The Employee ID Entered is not an Integer\n";
            }
            blnFatalError = TheDataValidationClass.VerifyTextData(mstrLastName);
            if (blnFatalError == true)
            {
                blnThereIsAProblem = true;
                mstrErrorMessage = mstrErrorMessage + "The Employee Last Name Was Not Entered\n";
            }
            if(blnThereIsAProblem == true)
            {
                TheMessagesClass.ErrorMessage(mstrErrorMessage);
                return;
            }

            //verifying log on
            blnLogonVerified = TheEmployeeClass.VerifyLogon(mintEmployeeIDLoggedIn, mstrLastName);

            //if statements
            if (blnLogonVerified == false)
            {
                LogonFailed();
            }
            else
            {
                //getting the group id
                mstrGroup = TheEmployeeClass.FindEmployeesGroupByID(mintEmployeeIDLoggedIn);

                if (mstrGroup != "ADMIN")
                {
                    LogonFailed();
                }
                else
                {
                    //getting the first name
                    mstrFirstName = TheEmployeeClass.FindEmployeeFirstNamewithID(mintEmployeeIDLoggedIn);

                    strMessageForUser = mstrFirstName + " " + mstrLastName + " Has Successfully Logged Into Time and Attendance";

                    //making a last transaction entry
                    TheLastTransactionClass.CreateLastTransactionEntry(mintEmployeeIDLoggedIn, strMessageForUser);

                    TheMessagesClass.InformationMessage(strMessageForUser);

                    //showing main menu
                    MainMenu MainMenu = new MainMenu();
                    MainMenu.Show();
                    this.Hide();

                }
            }

        }
        private void LogonFailed()
        {
            //message to user
            TheMessagesClass.InformationMessage("Logon Information is Incorrect, Please Try Again");

            //incrementing the number of misses
            mintNumberOfMisses++;

            //if statement
            if (mintNumberOfMisses == 3)
            {
                TheMessagesClass.ErrorMessage("There Have Been Three Attempts to Logon, The Program Will Close");

                TheEventLogClass.CreateEventLogEntry("There Has Been Three Attempts to Log into the Time And Attendance Program");

                Application.Exit();
            }
        }

        private void Logon_Load(object sender, EventArgs e)
        {
            mintNumberOfMisses = 0;
        }
    }
}

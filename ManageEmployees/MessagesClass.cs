/* Title:           The Messages Class
 * Date:            5-1-16
 * Author:          Terry Holmes
 *
 * Description:     This class provides all classes */       

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessagesDLL
{
    public class MessagesClass
    {
        //Public method to get the information
        public void ErrorMessage(string strErrorMessage)
        {
            MessageBox.Show(strErrorMessage, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void InformationMessage(string strInformationMessage)
        {
            MessageBox.Show(strInformationMessage, "Thank You", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void UnderDevelopment()
        {
            MessageBox.Show("The Module Is Under Development", "Thank You", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void CloseTheProgram()
        {
            DialogResult results = MessageBox.Show("Are You Sure You Want to Close The Program", "Thank You", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (results == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}

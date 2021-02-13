using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace SomerenDAL
{
     public class ErrorDAO 
    {
        // Handles the errors and displaying in a message box. Also Adds it in an Error log file.
        
        public ErrorDAO(string explanation)
        {
            string file = @"../../../Error.log";
            StreamWriter writer = File.AppendText(file);
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            //write the error into the error.log file
            writer.WriteLine($"Occurance time: {DateTime.Now.ToString()} Explanation: {explanation}");
            writer.Close();
            MessageBox.Show(explanation,"Error Occured",MessageBoxButtons.OK,MessageBoxIcon.Error);
           


        }
    }
}

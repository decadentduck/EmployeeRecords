using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EmployeeRecords
{
    public partial class mainForm : Form
    {
        List<Employee> employeeDB = new List<Employee>();

        public mainForm()
        {
            InitializeComponent();
            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string newID = idInput.Text;
            string newFirstName = fnInput.Text;
            string newLastName = lnInput.Text;
            string newStartDate = dateInput.Text;
            string newSalary = salaryInput.Text;

            Employee newEmployee = new Employee(newID, newFirstName, newLastName, newStartDate, newSalary);
            employeeDB.Add(newEmployee);

            ClearLabels();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            bool found = false;

            for (int i = 0; i < employeeDB.Count(); i++)
            {
                if (employeeDB[i].id == idInput.Text)
                {
                    found = true;
                    outputLabel.Text = "Employee " + employeeDB[i].id + " removed";

                    employeeDB.RemoveAt(i);
                }
            }

            if (!found)
            {
                outputLabel.Text = "Employee ID not found";
            }

            ClearLabels();
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";

            for (int i = 0; i < employeeDB.Count(); i++)
            {
                outputLabel.Text += employeeDB[i].id + " ";
                outputLabel.Text += employeeDB[i].firstName + " ";
                outputLabel.Text += employeeDB[i].lastName + " ";
                outputLabel.Text += employeeDB[i].date + " ";
                outputLabel.Text += employeeDB[i].salary + "\n";
            }

        }

        private void ClearLabels()
        {
            idInput.Text = "";
            fnInput.Text = "";
            lnInput.Text = "";
            dateInput.Text = "";
            salaryInput.Text = "";
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlTextWriter writer = new XmlTextWriter("employees.xml", null);

            //Write the "Class" element
            writer.WriteStartElement("Employees");
            for (int i = 0; i < employeeDB.Count(); i++)
            {

                //Start "student" element
                writer.WriteStartElement("Employee");

                //Write sub-elements
                writer.WriteElementString("id", employeeDB[i].id);
                writer.WriteElementString("firstName", employeeDB[i].firstName);
                writer.WriteElementString("lastName", employeeDB[i].lastName);
                writer.WriteElementString("date", employeeDB[i].date);
                writer.WriteElementString("salary", employeeDB[i].salary);

                // end the "student" element
                writer.WriteEndElement();
            }

            // end the "Class" element
            writer.WriteEndElement();

            //Write the XML to file and close the writer
            writer.Close();
        }

        public void loadDB()
        {

            string newID, newFirstName, newLastName, newStartDate, newSalary;
            newID = newFirstName = newLastName = newStartDate = newSalary = "";

            // Open the file to be read
            XmlTextReader reader = new XmlTextReader("employees.xml");

            int i = 1;

            // Continue to read each element and text until the file is done
            while (reader.Read())
            {
                // If the currently read item is text then print it to screen,
                // otherwise the loop repeats getting the next piece of information
                if (reader.NodeType == XmlNodeType.Text)
                {
                    if (i == 1) { newID = reader.Value; i++; }
                    else if (i == 2) { newFirstName = reader.Value; i++; }
                    else if (i == 3) { newLastName = reader.Value; i++; }
                    else if (i == 4) { newStartDate = reader.Value; i++; }
                    else if (i == 5)
                    {
                        newSalary = reader.Value;
                        i = 1;
                        Employee e = new Employee(newID, newFirstName, newLastName, newStartDate, newSalary);
                        employeeDB.Add(e);
                    }

                }
            }
            // When done reading the file close it
            reader.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Students
{
    public class StudentDetails
    {
        private int studentID;
        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = Fill(value); }
        }
        private string secondName;
        public string SecondName
        {
            get { return secondName; }
            set { secondName = Fill(value); }
        }
        private string dateOfBirth;
        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = Fill(value); }
        }
        private string foto;
        public string Foto
        {
            get { return (foto); }
            set { foto = Fill(value); }
        }
        public StudentDetails()
        {
        }
        public StudentDetails(int studentID, string firstName, string secondName, string dateOfBirth,string foto)
        {
            StudentID = studentID;
            FirstName = firstName;
            SecondName = secondName;
            DateOfBirth = dateOfBirth;
            Foto = foto;
        }
        private string Fill(string value)
        {
            if (value != null)
                return value;
            else
                return "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Linq;
using System.IO;

namespace Students
{
    public class XmlStudentDB
    {
        private string strStudentID = "StudentID";
        private string strFirstName = "FirstName";
        private string strSecondName = "SecondName";
        private string strDateOfBirth = "DateOfBirth";
        private string strFoto = "Foto";
        private string file = "";
        private XElement studentsList = null;
        public XmlStudentDB()
        {
            this.file = System.AppDomain.CurrentDomain.BaseDirectory + "Files\\Students.xml";
            try { studentsList = XElement.Load(file); }
            catch (FileNotFoundException ex) { }
        }
        public XmlStudentDB(string file)
        {
            this.file = file;
            try { studentsList = XElement.Load(file); }
            catch (FileNotFoundException ex) { }
        }
        public int InsertStudent(StudentDetails stud)
        {
            int maxId = studentsList.Elements("Student").Max(t => Int32.Parse(t.Element(strStudentID).Value));
            XElement track = new XElement("Student",
                new XElement(strStudentID, ++maxId),
                new XElement(strFirstName, stud.FirstName),
                new XElement(strSecondName, stud.SecondName),
                new XElement(strDateOfBirth, stud.DateOfBirth),
                new XElement(strFoto, ""));

            studentsList.Add(track);
            studentsList.Save(file);
            AddFoto(stud);
            return 0;
        }

        public void DeleteStudent(StudentDetails stud)
        {
            foreach (XElement elem in studentsList.Descendants("Student"))
            {
                if ((int)elem.Element(strStudentID) == stud.StudentID)
                {
                    elem.Remove();
                    studentsList.Save(file);
                    break;
                }
            }
        }
        public void UpdateStudent(StudentDetails stud)
        {
            foreach (XElement elem in studentsList.Descendants("Student"))
            {
                if ((int)elem.Element(strStudentID) == stud.StudentID)
                {
                    elem.Element(strFirstName).Value = stud.FirstName;
                    elem.Element(strSecondName).Value = stud.SecondName;
                    elem.Element(strDateOfBirth).Value = stud.DateOfBirth;
                    studentsList.Save(file);
                }
            }
            AddFoto(stud);
        }
        public DataSet GetStudents()
        {
            IEnumerable<XElement> match = from student in studentsList.Descendants("Student")
                                          select student;
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(new XElement("Student", match).ToString()));
            return ds;
        }
        public DataSet GetStudent(int studentID)
        {

            IEnumerable<XElement> stud = from student in studentsList.Descendants("Student")
                                         where (int)student.Element(strStudentID) == studentID
                                         select student;
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(new XElement("Student", stud).ToString()));
            return ds;
        }
        public DataSet GetStudent()
        {
            return null;
        }
        public int CountStudents()
        {
            return 0;
        }
        private void AddFoto(StudentDetails stud)
        {
            string pathToFoto = System.AppDomain.CurrentDomain.BaseDirectory + "\\Foto\\";
            if (File.Exists(pathToFoto + "Temp.jpg"))
            {
                int id = 0;
                foreach (XElement elem in studentsList.Descendants("Student"))
                {
                    if ((string)elem.Element(strFirstName) == stud.FirstName && (string)elem.Element(strSecondName) == stud.SecondName && (string)elem.Element(strDateOfBirth) == stud.DateOfBirth)
                    {
                        id = (int)elem.Element(strStudentID);
                    }
                }

                if (File.Exists(pathToFoto + id + ".jpg"))
                    File.Delete(pathToFoto + id + ".jpg");
                File.Copy(pathToFoto + "Temp.jpg", pathToFoto + id + ".jpg");
                File.Delete(pathToFoto + "Temp.jpg");


                string foto = "";
                if (File.Exists(pathToFoto + id + ".jpg"))
                    foto = @"\Foto\" + id + ".jpg";
                foreach (XElement elem in studentsList.Descendants("Student"))
                {
                    if ((int)elem.Element(strStudentID) == id)
                    {
                        elem.Element(strFoto).Value = foto;
                        studentsList.Save(file);
                    }
                }
            }
        }
    }
}
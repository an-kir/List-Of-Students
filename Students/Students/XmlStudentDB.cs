using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;

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
            int maxId=0;
            if (studentsList.HasElements)
                maxId = studentsList.Elements("Student").Max(t => Int32.Parse(t.Element(strStudentID).Value));
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
            //(new XmlStudentDB()).ExportToJson();
            //(new XmlStudentDB()).ExportToSql();

            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(new XElement("Student", match).ToString()));
            if (ds.Tables.Count != 0)
                return ds;
            else
            {
                DataTable dataTable = new DataTable();
                PropertyDescriptorCollection property = TypeDescriptor.GetProperties(typeof(StudentDetails));
                foreach (PropertyDescriptor prop in property)
                {
                    dataTable.Columns.Add(prop.Name, prop.PropertyType);
                }
                ds.Tables.Add(dataTable);
                return ds;
            }
        }
        public void ExportToSql()
        {
            IEnumerable<XElement> match = from student in studentsList.Descendants("Student")
                                          select student;

            List<StudentDetails> list = new List<StudentDetails>();
            foreach (XElement e in match.ToList())
            {
                StudentDetails st = new StudentDetails((int)e.Element(strStudentID), (string)e.Element(strFirstName), (string)e.Element(strSecondName), (string)e.Element(strDateOfBirth), (string)e.Element(strFoto));
                list.Add(st);
            }
            if (list != null)
                (new SqlStudentDB()).Fill(list);
        }
        public void ExportToJson()
        {
            IEnumerable<XElement> match = from student in studentsList.Descendants("Student")
                                          select student;

            List<StudentDetails> list = new List<StudentDetails>();
            foreach (XElement e in match.ToList())
            {
                StudentDetails st = new StudentDetails((int)e.Element(strStudentID), (string)e.Element(strFirstName), (string)e.Element(strSecondName), (string)e.Element(strDateOfBirth), (string)e.Element(strFoto));
                list.Add(st);
            }
            if (list != null)
                (new JsonStudentDB()).WriteListToJsonFile(list);
        }
        public DataSet GetStudent(int studentID)
        {

            IEnumerable<XElement> stud = from student in studentsList.Descendants("Student")
                                         where (int)student.Element(strStudentID) == studentID
                                         select student;
            
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(new XElement("Student", stud).ToString()));
            if (ds.Tables.Count != 0)
                return ds;
            else
            {
                ds.Tables.Add(new DataTable());
                return ds;
            }
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
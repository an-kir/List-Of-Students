using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.IO;
using System.ComponentModel;

namespace Students
{
    public class JsonStudentDB
    {
        string path = "";
        public JsonStudentDB()
        {
            path = System.AppDomain.CurrentDomain.BaseDirectory + "Files\\Json.txt";
        }
        public JsonStudentDB(string file)
        {
            path = System.AppDomain.CurrentDomain.BaseDirectory + "Files\\" + file;
        }
        public int InsertStudent(StudentDetails stud)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string JsonStudent = js.Serialize(stud);
            List<StudentDetails> list = ReadToListFromJsonFile(path);
            int max = 0;
            foreach (StudentDetails student in list)
            {
                if (student.StudentID > max)
                    max = student.StudentID;
            }
            stud.StudentID = ++max;
            list.Add(stud);
            WriteListToJsonFile(list, path);
            AddFoto(stud);
            return 0;
        }

        public void DeleteStudent(StudentDetails stud)
        {
            List<StudentDetails> list = ReadToListFromJsonFile(path);
            foreach (StudentDetails student in list)
            {
                if (student.StudentID == stud.StudentID)
                {
                    list.Remove(student);
                    break;
                }
            }
            WriteListToJsonFile(list, path);
        }
        public void UpdateStudent(StudentDetails stud)
        {
            List<StudentDetails> list = ReadToListFromJsonFile(path);
            foreach (StudentDetails student in list)
            {
                if (student.StudentID == stud.StudentID)
                {
                    student.FirstName = stud.FirstName;
                    student.SecondName = stud.SecondName;
                    student.DateOfBirth = stud.DateOfBirth;
                }
            }

            WriteListToJsonFile(list, path);
            AddFoto(stud);
        }
        public DataTable GetStudents()
        {
            //наполнение списка студентов List<StudentDetails> из файла json
            List<StudentDetails> list = ReadToListFromJsonFile(path);
            //копирование полученного списка List<StudentDetails> в DataTable
            return CopyListToDataTable(list);
        }
        public DataTable GetStudent(int studentID)
        {
            List<StudentDetails> list = ReadToListFromJsonFile(path);
            List<StudentDetails> oneStudent = new List<StudentDetails>();
            foreach (StudentDetails student in list)
            {
                if (student.StudentID == studentID)
                    oneStudent.Add(student);
            }
            return CopyListToDataTable(oneStudent);
        }
        public DataTable GetStudent()
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
                List<StudentDetails> list = ReadToListFromJsonFile(path);
                int id = 0;
                foreach (StudentDetails student in list)
                {
                    if (student.FirstName == stud.FirstName && student.SecondName == student.SecondName && stud.DateOfBirth == stud.DateOfBirth)
                        id = student.StudentID;
                }
                if (File.Exists(pathToFoto + id + ".jpg"))
                    File.Delete(pathToFoto + id + ".jpg");
                File.Copy(pathToFoto + "Temp.jpg", pathToFoto + id + ".jpg");
                File.Delete(pathToFoto + "Temp.jpg");

                string foto = "";
                if (File.Exists(pathToFoto + id + ".jpg"))
                    foto = @"\Foto\" + id + ".jpg";
                foreach (StudentDetails student in list)
                {
                    if (student.StudentID == id)
                        student.Foto = foto;
                }
                WriteListToJsonFile(list, path);
            }
        }
        private List<StudentDetails> ReadToListFromJsonFile(string path)
        {
            List<StudentDetails> list = new List<StudentDetails>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string student;
            StreamReader reader = new StreamReader(path);
            do
            {
                student = reader.ReadLine();
                if (student != null)
                    list.Add((StudentDetails)js.Deserialize(student, typeof(StudentDetails)));
            }
            while (student != null);
            reader.Close();
            return list;
        }
        private DataTable CopyListToDataTable(List<StudentDetails> list)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection property = TypeDescriptor.GetProperties(typeof(StudentDetails));
            foreach (PropertyDescriptor prop in property)
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            foreach (StudentDetails item in list)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyDescriptor prop in property)
                {
                    if (row[prop.Name] != null)
                        row[prop.Name] = prop.GetValue(item);
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
        private void WriteListToJsonFile(List<StudentDetails> list, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            JavaScriptSerializer js = new JavaScriptSerializer();
            foreach (StudentDetails st in list)
            {
                sw.WriteLine(js.Serialize(st));
            }
            sw.Close();
        }
        public void WriteListToJsonFile(List<StudentDetails> list)
        {
            StreamWriter sw = new StreamWriter(path);
            JavaScriptSerializer js = new JavaScriptSerializer();
            foreach (StudentDetails st in list)
            {
                sw.WriteLine(js.Serialize(st));
            }
            sw.Close();
        }
        public void WriteTOJsonFile(DataSet ds)
        {
            //ds.w
        }
    }
}
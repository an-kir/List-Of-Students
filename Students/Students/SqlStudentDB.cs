using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Students
{
    public class SqlStudentDB
    {
        private string connectionString;
        private SqlConnection con;
        public SqlStudentDB()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
            con = new SqlConnection(connectionString);
            
        }
        public SqlStudentDB(string connectionString)
        {
            this.connectionString = connectionString;
            con = new SqlConnection(connectionString);
        }
        public int InsertStudent(StudentDetails stud)
        {
            string sql = "Insert Into Student (FirstName,SecondName,DateOfBirth) Values ('" + stud.FirstName + "','" + stud.SecondName + "','" + stud.DateOfBirth + "')";
            SqlCommand cmd = new SqlCommand(sql, con);
            int number;
            using (con)
            {
                con.Open();
                number = cmd.ExecuteNonQuery();
            }

            AddFoto(stud);
            return number;
        }
        
        public void DeleteStudent(StudentDetails stud)
        {
            //SqlConnection con = new SqlConnection(connectionString);
            string sql = "Delete Student Where studentID=" + stud.StudentID;
            SqlCommand cmd = new SqlCommand(sql, con);
            using (con)
            {
                con.Open();
                int numAff = cmd.ExecuteNonQuery();
            }
            string path = System.AppDomain.CurrentDomain.BaseDirectory + stud.Foto;
            if (File.Exists(path))
                File.Delete(path);
        }
        public void UpdateStudent(StudentDetails stud)
        {
            string sql = "";
            sql += "Update Student";
            sql += " Set firstName='" + stud.FirstName + "',secondName='" + stud.SecondName + "', dateOfBirth='" + stud.DateOfBirth + "'";
            sql += " From Student Where studentID='" + stud.StudentID+"'";
            SqlCommand cmd = new SqlCommand(sql, con);
            using (con)
            {
                con.Open();
                int numAff = cmd.ExecuteNonQuery();
            }
            AddFoto(stud);
        }
        public DataSet GetStudent(int studentID)
        {
            string sqlSelect = "Select * from Student where studentID=" + studentID;
            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Student");
            return ds;
        }
        public DataSet GetStudent()
        {
            return null;
        }
        public DataSet GetStudents()
        {
            //SqlConnection con = new SqlConnection(connectionString);
            string sqlSelect = "Select * from Student";
            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Student");
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + "Files\\Students.xml";
            //ds.WriteXml(path);
            
            return ds;
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
                con = new SqlConnection(connectionString);
                string sql = "select StudentID from Student Where FirstName='" + stud.FirstName + "' AND SecondName='" + stud.SecondName + "' AND DateOfBirth='" + stud.DateOfBirth + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader;
                int id = 0;
                using (con)
                {
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        reader.Read();
                        id = (int)reader["StudentID"];
                    }
                    //id = (string)cmd.ExecuteScalar();
                }

                if (File.Exists(pathToFoto + id + ".jpg"))
                    File.Delete(pathToFoto + id + ".jpg");
                File.Copy(pathToFoto + "Temp.jpg", pathToFoto + id + ".jpg");
                File.Delete(pathToFoto + "Temp.jpg");

                string foto = "";
                con = new SqlConnection(connectionString);
                if (File.Exists(pathToFoto + id + ".jpg"))
                    foto = @"\Foto\" + id + ".jpg";
                sql = "update Student Set Foto='" + foto + "' From Student Where studentID='" + id + "'";
                cmd = new SqlCommand(sql, con);
                using (con)
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Fill(List<StudentDetails> list)
        {
            string sql = "Delete Student";
            con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, con);
            using (con)
            {
                con.Open();
                int numAff = cmd.ExecuteNonQuery();
            }
            con = new SqlConnection(connectionString);
            int number;
            using (con)
            {
                con.Open();
                foreach (StudentDetails stud in list)
                {
                    string sqlins = "Insert Into Student (FirstName,SecondName,DateOfBirth,Foto) Values ('" + stud.FirstName + "','" + stud.SecondName + "','" + stud.DateOfBirth + "','" + stud.Foto + "')";
                    SqlCommand cmdins = new SqlCommand(sqlins, con);
                    number = cmdins.ExecuteNonQuery();
                }
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventar_bearbeiten.Model;

namespace Inventar_bearbeiten.Controller
{
    class ProjectController
    {


        public static int getProjectID(string projectname)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT ProjectID FROM Projects WHERE ProjectName = '" + projectname + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var projectID = cmd.ExecuteScalar();
            con.Close();
            if (projectID != null)
            {
                return Convert.ToInt32(projectID);
            }
            else
            {
                return 0;
            }
        }

        public static string getProjectNameByID(int projectID)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT ProjectName FROM Projects WHERE ProjectID = '" + projectID + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            var projectname = cmd.ExecuteScalar();
            con.Close();
            if (projectname != null)
            {
                return projectname.ToString();
            }
            else
            {
                return "none";
            }
        }


        public static List<ProjectModel> getProjectList()
        {
            List<ProjectModel> projectModels = new List<ProjectModel>();
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Projects";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ProjectModel model = new ProjectModel()
                    {
                        ProjectID = Convert.ToInt32(reader[0]),
                        Name = reader[1].ToString()
                    };
                    projectModels.Add(model);
                }
                con.Close();
                return projectModels;
            }
        }


        public static void addProject(ProjectModel project)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            string query = "INSERT INTO Projects (ProjectName) VALUES(@ProjectName);";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            con.Open();
          
            cmd.Parameters.AddWithValue("@ProjectName", project.Name);
           
            cmd.ExecuteNonQuery();
            con.Close();
        }


    }
}

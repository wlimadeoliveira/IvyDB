using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventar_bearbeiten;
using Ivy_DB.Model;

namespace Ivy_DB.Controller
{
    class Article_ProjectController
    {
        public static void addArticleProjects(List<Article_ProjectModel> article_projects)
        {
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            string query = "INSERT INTO Article_Project (ID_Article, ID_Project) VALUES(@ID_Article, @ID_Project);";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            con.Open();
            foreach (Article_ProjectModel project in article_projects)
            {
                if (!articleprojectExists(project))
                {
                    cmd.Parameters.AddWithValue("@ID_Article", project.ID_Article);
                    cmd.Parameters.AddWithValue("@ID_Project", project.ID_Project);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    
                }

            }
            con.Close();
        }

        public static bool articleprojectExists(Article_ProjectModel proj)
        {
        
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT * FROM Article_Project WHERE ID_Project = '" + proj.ID_Project + "' AND ID_Article = '" + proj.ID_Article + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
     
        } 

        public static string getProjects(string articleID)
        {
            string projects = "";
            SQLiteConnection con = new SQLiteConnection(InventoryDBSqliteConnection.LoadConnectionString());
            con.Open();
            string query = "SELECT  proj.ProjectName FROM Article_Project AS artproj" +
                            " JOIN Projects AS proj ON proj.ProjectID = artproj.ID_Project"+
                            " WHERE artproj.ID_Article = '" + articleID + "'";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        projects += "," + reader[0].ToString();

                    }
                    return projects.Remove(0, 1);
                }
                else return "";
   
            }
            
        }




    }
}

using Kolokwium1Poprawa.DTOs.Responses;
using Kolokwium1Poprawa.Exceptions;
using Kolokwium1Poprawa.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Kolokwium1Poprawa.Services
{
    public class SqlDbTaskService : IDbService
    {
        private const string _ConnectionString = "Data Source=db-mssql;Initial Catalog=s18819;Integrated Security=True";

        public GetTeamMemberRes GetTeamMemberBy(int id_member)
        {
            var response = new GetTeamMemberRes();

            using (var con = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = con;
                command.CommandText = @"select * from teammember where IdTeamMember = @Id ";
                command.Parameters.AddWithValue("Id", id_member);

                con.Open();
                var dataReader = command.ExecuteReader();
                if (!dataReader.Read())
                {
                    dataReader.Close();
                    throw new MemberDoesntExistsException($"Member id: {id_member} doesn't exists!");
                }
                if (dataReader.Read())
                {
                    response.IdTeamMember = int.Parse(dataReader["IdTeamMember"].ToString());
                    response.FirstName = dataReader["FirstName"].ToString();
                    response.LastName = dataReader["LastName"].ToString();
                    response.Email = dataReader["Email"].ToString();
                }
            }

                using (var con = new SqlConnection(_ConnectionString))
                using (var command = new SqlCommand())
                {
                //ja nadałem
                command.Connection = con;
                command.CommandText = @"select * from teammember tm join task t on tm.IdTeamMember = t.IdCreator join project p on t.IdTeam=p.IdTeam where tm.IdTeamMember = @Id and t.IdTeam=p.IdTeam order by Deadline desc";
                command.Parameters.AddWithValue("Id", id_member);

                con.Open();
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var task = new MyTask();
                    task.Deadline = DateTime.Parse(dataReader["t.Deadline"].ToString());
                    task.Description = dataReader["t.Description"].ToString();
                    task.Name = dataReader["t.Name"].ToString();
                    task.Project.Name = dataReader["p.Name"].ToString();
                    task.Project.IdTeam = int.Parse(dataReader["p.IdTeam"].ToString());
                    response.MyTasks.Add(task);
                }
                con.Close();
            }

            using (var con = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                // do zrobienia przeze mnie
                command.Connection = con;
                command.CommandText = @"select * from teammember tm join task t on tm.IdTeamMember = t.IdAssignedTo join project p on t.IdTeam=p.IdTeam where tm.IdTeamMember = @Id and t.IdTeam=p.IdTeam order by Deadline desc";
                command.Parameters.AddWithValue("Id", id_member);

                con.Open();
                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var task = new MyTask();
                    task.Deadline = DateTime.Parse(dataReader["t.Deadline"].ToString());
                    task.Description = dataReader["t.Description"].ToString();
                    task.Name = dataReader["t.Name"].ToString();
                    task.Project.Name = dataReader["p.Name"].ToString();
                    task.Project.IdTeam = int.Parse(dataReader["p.IdTeam"].ToString());
                    response.ToDoTasks.Add(task);
                }
                con.Close();
            }
            return response;
        }
        public void RemoveProjectBy(int id_project)
        {
            using (var con = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = con;

                con.Open();
                var transaction = con.BeginTransaction();

                command.Transaction = transaction;

                command.CommandText = "select * from Project where IdTeam = @id";
                command.Parameters.AddWithValue("id", id_project);

                var dataReader = command.ExecuteReader();

                if (!dataReader.Read())
                {
                    dataReader.Close();
                    transaction.Rollback();
                    throw new ProjectDoesntExistsException($"Project id = {id_project} doesn't exists");
                }

                dataReader.Close();

                command.CommandText ="select IdTask from Task where IdTeam = @id";
                command.Parameters.AddWithValue("id", id_project);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    command.CommandText = $"delete from Task where IdTask = {dataReader["IdTask"].ToString()}";
                    command.ExecuteNonQuery();
                }
                dataReader.Close();

                command.CommandText ="delete from Project where IdTeam = @id";
                command.Parameters.AddWithValue("id", id_project);
                command.ExecuteNonQuery();

                transaction.Commit();
            }
        }
    }
}

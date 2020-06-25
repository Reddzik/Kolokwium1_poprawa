using Kolokwium1Poprawa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kolokwium1Poprawa.DTOs.Responses
{
    public class GetTeamMemberRes
    {
        public int IdTeamMember { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<MyTask> MyTasks { get; set; }
        public List<MyTask> ToDoTasks { get; set; }
    }
}

using Kolokwium1Poprawa.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kolokwium1Poprawa.Services
{
    public interface IDbService
    {
        public void RemoveProjectBy(int id);
        public GetTeamMemberRes GetTeamMemberBy(int id);
    }
}

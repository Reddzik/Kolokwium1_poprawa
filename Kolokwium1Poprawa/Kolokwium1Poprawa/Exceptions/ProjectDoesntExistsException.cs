using System;


namespace Kolokwium1Poprawa.Exceptions
{
    public class ProjectDoesntExistsException : Exception
    {
        public ProjectDoesntExistsException(string msg): base(msg)
        {

        }
    }
}

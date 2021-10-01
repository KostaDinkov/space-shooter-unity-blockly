using System;
using System.Collections.Generic;


namespace Models
{
    public class Person
    {
        public Person()
        {
            this.ProblemStates = new List<ProblemState>();
        }
        public Guid Id { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<ProblemState> ProblemStates { get; set; }
    }
}

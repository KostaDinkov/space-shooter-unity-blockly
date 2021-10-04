using System;

namespace Models
{
    public class ProblemState
    {
        public Guid Id { get; set; }
        
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public string LevelName { get; set; }
        public string ProblemName { get; set; }
        public string ProblemBlocksXml { get; set; }
        public int ProblemScore { get; set; }
        public bool ProblemLocked { get; set; }

        public bool ProblemCompleted { get; set; }

    }
}

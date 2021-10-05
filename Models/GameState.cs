using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class GameState
    {
        [ForeignKey("Person")]
        public Guid GameStateId { get; set; }
        public bool GameCompleted { get; set; }

        public Guid LastUnlockedProblemId { get; set; }
        public ProblemState LastUnlockedProblem { get; set; }

        public Person Person { get; set; }
    }
}

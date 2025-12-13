using System;
using System.Collections.ObjectModel;

namespace project_chess_DB.Models
{
    public class Competition
    {
        public string CompetitionDate { get; set; } = string.Empty;
        public string CompetitionNumber { get; set; } = string.Empty;

        public string Player1_RegNumber { get; set; } = string.Empty;
        public string Player1_Result { get; set; } = string.Empty;
        public string Player1_Moves { get; set; } = string.Empty; 

        public string Player2_RegNumber { get; set; } = string.Empty;
        public string Player2_Result { get; set; } = string.Empty;
        public string Player2_Moves { get; set; } = string.Empty;
        public Competition() { }

        public Competition(string competitionDate,string competitionNumber, string p1Reg, string p1Result, string p2Reg, string p2Result)
        {
            this.CompetitionDate = competitionDate;
            this.CompetitionNumber = competitionNumber;
            this.Player1_RegNumber = p1Reg;
            this.Player1_Result = p1Result;
            this.Player2_RegNumber = p2Reg;
            this.Player2_Result = p2Result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvPolisGame
{
    class Thief : Person
    {
        public int IdNumber { get; set; }
        public List<Item> Swag { get; set; }
        public bool IsInPrison { get; set; }
        public DateTime TimeOfCapture { get; set; }
        //public double TimeInPrison { get; set; }

        public Thief(int idNumber, int xPosition, int yPosition, int xDirection, int yDirection, List<Item> swag, bool isInPrison, DateTime timeOfCapture/*,*/
            /*double timeInPrison*/) :
            base(xPosition, yPosition, xDirection, yDirection)
        {
            IdNumber = idNumber;
            Swag = swag;
            IsInPrison = isInPrison;
            TimeOfCapture = timeOfCapture;
            //TimeInPrison = timeInPrison;
        }
    }
}

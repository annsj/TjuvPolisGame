using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvPolisGame
{
    class Citizen : Person
    {
        public List<Item> Belongings { get; set; }
        public Citizen(int xPosition, int yPosition, int xDirection, int yDirection, List<Item> belongings) :
            base(xPosition, yPosition, xDirection, yDirection)
        {
            Belongings = belongings;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvPolisGame
{
    class Police : Person
    {
        public List<Item> ConfiscatedItems { get; set; }
        public Police(int xPosition, int yPosition, int xDirection, int yDirection, List<Item> confiscatedItems) :
            base(xPosition, yPosition, xDirection, yDirection)
        {
            ConfiscatedItems = confiscatedItems;
        }
    }
}

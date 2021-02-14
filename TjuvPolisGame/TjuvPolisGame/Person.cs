using System;
using System.Collections.Generic;
using System.Text;

namespace TjuvPolisGame{
    
    class Person
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int XDirection { get; set; }
        public int YDirection { get; set; }
        public Person(int xPosition, int yPosition, int xDirection, int yDirection)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            XDirection = xDirection;
            YDirection = yDirection;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace TjuvPolisGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int cityWidth = 100;
            int cityHeight = 25;

            int noOfCitizens = 30;
            int noOfPolice = 10;
            int noOfThiefs = 20;

            int robberies = 0;
            int captures = 0;

            // bools som används vid utskrift av stad
            bool isCitizen = false;
            bool isPolice = false;
            bool isThief = false;
            bool isRobbery = false;
            bool isCapture = false;
            bool isCitizenMeetsPolice = false;

            List<Citizen> citizenList = new List<Citizen>();
            CreateCitizenList(noOfCitizens, cityWidth, cityHeight, citizenList);

            List<Police> policeList = new List<Police>();
            CreatePoliceList(noOfPolice, cityWidth, cityHeight, policeList);

            List<Thief> thiefList = new List<Thief>();
            CreateThiefList(noOfThiefs, cityWidth, cityHeight, thiefList);

            List<Thief> prison = new List<Thief>();

            while (true)
            {
                Console.Clear();

                // bools som används för att styra utskrift av statistik
                bool robberyEvent = false;
                bool captureEvent = false;
                bool freedomEvent = false;
                for (int y = 0; y < cityHeight; y++)
                {
                    for (int x = 0; x < cityWidth; x++)
                    {
                        if (CheckIfThief(x, y, thiefList))
                        {
                            if (CheckIfCitizen(x, y, citizenList) == false &&
                                CheckIfPolice(x, y, policeList) == false)
                            {
                                isThief = true;
                            }
                            if (CheckIfCitizen(x, y, citizenList))
                            {
                                foreach (Thief thief in thiefList)
                                {
                                    if (thief.XPosition == x && thief.YPosition == y)
                                    {
                                        foreach (Citizen citizen in citizenList)
                                        {
                                            if (citizen.XPosition == x && citizen.YPosition == y)
                                            {
                                                PerformTheft(citizen, thief);
                                                robberies++; // Räknas som robbery även om M inte har ngt att stjäla
                                                robberyEvent = true;
                                            }
                                        }
                                    }
                                }
                                if (CheckIfPolice(x, y, policeList) == false)
                                {
                                    isRobbery = true;
                                }
                            }


                            if (CheckIfPolice(x, y, policeList)) // Om alla tre kommer på samma ställe sker först rånet och sedan tar polisen genast tjuven
                            {
                                foreach (Thief thief in thiefList)
                                {
                                    if (thief.XPosition == x && thief.YPosition == y)
                                    {
                                        foreach (Police police in policeList)
                                        {
                                            if (police.XPosition == x && police.YPosition == y)
                                            {
                                                if (CheckSwag(thief))  //Polisen tar brara tjuven om hen har stöldgods på sig
                                                {
                                                    ConfiscateSwag(thief, police);
                                                    SendToPrison(thief, prison);
                                                    captureEvent = true;
                                                    captures++;
                                                    isCapture = true; //Det hade gått att använda capturEvent tror jag men kanske förvirrande eftrsom de andra boolarna som styr utskrift heter isNånting
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (CheckIfPolice(x, y, policeList))
                        {
                            if (CheckIfCitizen(x, y, citizenList))
                            {
                                isCitizenMeetsPolice = true;
                            }
                            else
                            {
                                isPolice = true;
                            }
                        }
                        else if (CheckIfCitizen(x, y, citizenList))
                        {
                            isCitizen = true;
                        }

                        Console.Write(PrintBoardChar(isCitizen, isPolice, isThief, isRobbery, isCapture, isCitizenMeetsPolice));

                        isCitizen = false;
                        isPolice = false;
                        isThief = false;
                        isRobbery = false;
                        isCapture = false;
                        isCitizenMeetsPolice = false;
                    }

                    Console.WriteLine();

                }

                Console.WriteLine();

                Thread.Sleep(20);

                foreach (Thief thief in prison)
                {
                    if (GetPrisonTime(thief) >= 30)
                    {
                        Console.WriteLine($"Tjuv med ID {thief.IdNumber} frigavs från fängelset.");
                        freedomEvent = true;
                    }
                }

                if (robberyEvent || captureEvent || freedomEvent)
                {
                    if (robberyEvent)
                    {
                        Console.WriteLine("Medborgare rånad av tjuv");
                    }
                    if (captureEvent)
                    {
                        Console.WriteLine("Tjuv fångad av polis");
                    }
                    Console.WriteLine("\nStatistik:");
                    Console.WriteLine($"Antal rånade medborgare: {robberies}");
                    Console.WriteLine($"Antal gripna tjuvar: {captures}");
                    Console.WriteLine();

                    while (freedomEvent)
                    {
                        freedomEvent = false;
                        foreach (Thief thief in prison)
                        {
                            if (GetPrisonTime(thief) >= 30)
                            {
                                SendToFreedom(thief, prison);
                                freedomEvent = true;
                                break; // Börjar om med uppdaterad prisonlista, annars kraschar SendToFreedom eftersom listan ändras under tiden metoden loopar genom listan
                            }
                        }
                    }

                    switch (prison.Count)
                    {
                        case 0:
                            Console.WriteLine("Fängelset är tomt.");
                            break;
                        case 1:
                            Console.WriteLine($"{prison.Count} tjuv avtjänar 30 sekunder i fängelse.");
                            break;
                        default:
                            Console.WriteLine($"{prison.Count} tjuvar avtjänar 30 sekunder i fängelse.");
                            break;
                    }
                    Console.WriteLine();

                    if (prison.Count > 0)
                    {
                        Console.WriteLine("Fängelset har just nu följande interner:");
                        Console.WriteLine(PrintPrisonList(prison));
                    }

                    Thread.Sleep(2000);

                    //// Behövde ibland skriva ut alla personer för att se vad som hände
                    //Console.WriteLine();
                    //Console.WriteLine("Detljerad info om alla personer:");
                    //Console.WriteLine();
                    //Console.WriteLine(CtrlMethods.PrintCitizenList(citizenList));
                    //Console.WriteLine(CtrlMethods.PrintPoliceList(policeList));
                    //Console.WriteLine(CtrlMethods.PrintThiefList(thiefList));

                    //Console.WriteLine("Tryck på valfri tangent för att fortsätta");
                    //Console.ReadKey();
                }

                foreach (Citizen citizen in citizenList)
                {
                    Move(cityWidth, cityHeight, citizen);
                }
                foreach (Police police in policeList)
                {
                    Move(cityWidth, cityHeight, police);
                }
                foreach (Person thief in thiefList)  // Testar att använda typen Person istället för Thief, det verkar gå lika bra
                {
                    Move(cityWidth, cityHeight, thief);
                }
            }
        }


        private static Random r = new Random();

        private static char PrintBoardChar(bool isCitizen, bool isPolice, bool isThief, bool isRobbery, bool isCapture, bool isCitizenMeetsPolice)
        {
            char c;

            if (isCitizen)
            {
                c = 'M';
            }
            else if (isPolice)
            {
                c = 'P';
            }
            else if (isThief)
            {
                c = 'T';
            }
            else if (isRobbery)
            {
                c = '!';
            }
            else if (isCapture)
            {
                c = '#';
            }
            else if (isCitizenMeetsPolice)
            {
                c = '*';
            }
            else
            {
                c = ' ';
            }
            return c;
        }

        private static string PrintPrisonList(List<Thief> prison)
        {
            string s = "";
            foreach (Thief thief in prison)
            {
                s += $"Tjuv med ID {thief.IdNumber } har suttit i fängelse {Math.Round(GetPrisonTime(thief))} sekunder.\n";
            }
            return s;
        }

        private static void Move(int width, int height, Person person)
        {
            person.XPosition += person.XDirection;
            if (person.XPosition > width - 1 && person.XDirection == 1)
            {
                person.XPosition = 0;
            }
            if (person.XPosition < 0 && person.XDirection == -1)
            {
                person.XPosition = width - 1;
            }

            person.YPosition += person.YDirection;
            if (person.YPosition > height - 1 && person.YDirection == 1)
            {
                person.YPosition = 0;
            }
            if (person.YPosition < 0 && person.YDirection == -1)
            {
                person.YPosition = height - 1;
            }
        }

        private static void SendToPrison(Thief thief, List<Thief> prison)
        {
            thief.TimeOfCapture = DateTime.Now;
            prison.Add(thief);
            thief.IsInPrison = true;
        }

        private static double GetPrisonTime(Thief thief)
        {
            return (DateTime.Now - thief.TimeOfCapture).TotalSeconds;
        }

        private static void SendToFreedom(Thief thief, List<Thief> prison)
        {
            prison.Remove(thief);
            thief.IsInPrison = false;
        }

        private static void ConfiscateSwag(Thief thief, Police police)
        {
            for (int i = 0; i < thief.Swag.Count; i++)
            {
                police.ConfiscatedItems[i].NoOfItems += thief.Swag[i].NoOfItems;
                thief.Swag[i].NoOfItems = 0;
            }
        }

        private static void PerformTheft(Citizen citizen, Thief thief)
        {
            if (CheckBelongings(citizen))
            {
                int selectedItem = r.Next(0, citizen.Belongings.Count);
                while (citizen.Belongings[selectedItem].NoOfItems < 1)
                {
                    selectedItem = r.Next(0, citizen.Belongings.Count);
                }
                thief.Swag[selectedItem].NoOfItems++;
                citizen.Belongings[selectedItem].NoOfItems--;
            }
        }

        private static bool CheckBelongings(Citizen citizen)
        {
            bool hasBelonging = false;

            for (int i = 0; i < 4; i++)
            {
                if (citizen.Belongings[i].NoOfItems > 0)
                {
                    hasBelonging = true;
                    break;
                }
            }
            return hasBelonging;
        }

        private static bool CheckSwag(Thief thief)
        {
            bool hasSwag = false;

            for (int i = 0; i < 4; i++)
            {
                if (thief.Swag[i].NoOfItems > 0)
                {
                    hasSwag = true;
                    break;
                }
            }
            return hasSwag;
        }

        private static bool CheckIfCitizen(int x, int y, List<Citizen> citizens)
        {
            bool isCitizen = false;
            foreach (Citizen citizen in citizens)
            {
                if (x == citizen.XPosition && y == citizen.YPosition)
                {
                    isCitizen = true;
                    break;
                }
            }
            return isCitizen;
        }

        private static bool CheckIfPolice(int x, int y, List<Police> policeOfficers)
        {
            bool isPolice = false;
            foreach (Police police in policeOfficers)
            {
                if (x == police.XPosition && y == police.YPosition)
                {
                    isPolice = true;
                    break;
                }
            }
            return isPolice;
        }

        private static bool CheckIfThief(int x, int y, List<Thief> thiefs)
        {
            bool isThief = false;
            foreach (Thief thief in thiefs)
            {
                if (x == thief.XPosition && y == thief.YPosition && thief.IsInPrison == false)
                {
                    isThief = true;
                    break;
                }
            }
            return isThief;
        }

        // Försökte lägga CreateList-metoderna och CreateDirection i klasserna men fick inte till det
        private static void CreateCitizenList(int noOfCitizens, int width, int height, List<Citizen> citizens)
        {
            for (int i = 0; i < noOfCitizens; i++)
            {
                int xPos = r.Next(0, width);
                int yPos = r.Next(0, height);
                (int xDir, int yDir) = CreateDirection();
                List<Item> belongings = new List<Item>();
                belongings.Add(new Item("Keys", 1));
                belongings.Add(new Item("Telephone", 1));
                belongings.Add(new Item("Money", 1));
                belongings.Add(new Item("Watch", 1));
                citizens.Add(new Citizen(xPos, yPos, xDir, yDir, belongings));
            }
        }
        private static void CreatePoliceList(int noOfPolice, int width, int height, List<Police> policeOfficers)
        {
            for (int i = 0; i < noOfPolice; i++)
            {
                int xPos = r.Next(0, width);
                int yPos = r.Next(0, height);
                (int xDir, int yDir) = CreateDirection();
                List<Item> confiscatedItems = new List<Item>
                {
                    new Item("Keys", 0),
                    new Item("Telephone", 0),
                    new Item("Money", 0),
                    new Item("Watch", 0)
                };
                policeOfficers.Add(new Police(xPos, yPos, xDir, yDir, confiscatedItems));
            }
        }
        private static void CreateThiefList(int noOfThiefs, int width, int height, List<Thief> thiefs)
        {
            for (int i = 0; i < noOfThiefs; i++)
            {
                int idNumber = i + 1;
                int xPos = r.Next(0, width);
                int yPos = r.Next(0, height);
                (int xDir, int yDir) = CreateDirection();
                List<Item> swag = new List<Item>
                {
                    new Item("Keys", 0),
                    new Item("Telephone", 0),
                    new Item("Money", 0),
                    new Item("Watch", 0)
                };
                bool isInPrison = false;
                DateTime timeOfCapture = new DateTime();
                //double timeInPrison = 0;
                thiefs.Add(new Thief(idNumber, xPos, yPos, xDir, yDir, swag, isInPrison, timeOfCapture/*, timeInPrison*/));
            }
        }

        private static (int xDir, int yDir) CreateDirection()
        {
            int xDir = r.Next(-1, 1 + 1);
            int yDir = r.Next(-1, 1 + 1);
            while (xDir == 0 && yDir == 0)
            {
                yDir = r.Next(-1, 1 + 1);
            }
            return (xDir, yDir);
        }
    }
}

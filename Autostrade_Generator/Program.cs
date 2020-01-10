using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autostrade_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool success = false;

            //Get the start room ID
            uint startRoomID = 0;
            while (success == false)
            {
                Console.WriteLine("Start room ID?");
                string input = Console.ReadLine();
                success = uint.TryParse(input, out startRoomID);

                //If it failed it will loop around
                if (success == false)
                    Console.WriteLine("Invalid input. It should be an unsigned integer.");
            }

            success = false;

            //Get the amount of rooms
            uint amountOfRooms = 0;
            while (success == false)
            {
                Console.WriteLine("Amount of rooms?");
                string input = Console.ReadLine();
                success = uint.TryParse(input, out amountOfRooms);

                //If it failed it will loop around
                if (success == false)
                    Console.WriteLine("Invalid input. It should be an unsigned integer.");
            }

            Console.WriteLine("Generating...");
            Generator generator = new Generator();
            generator.Generate(startRoomID, amountOfRooms);

            //End of program
            Console.WriteLine("Done! Press any key to exit.");
            Console.ReadLine(); //Wait for input
        }
    }

    class Generator
    {
        string m_RoomData = string.Empty;
        int m_StartExitX = 0; //Inclusive
        int m_StartExitY = 0;
        int m_EndExitX = 0;
        int m_EndExitY = 0;
        int m_PaletteID = 0;

        //Constructor
        public Generator()
        {
            //Init
            m_StartExitX = 6;
            m_StartExitY = 0;
            m_EndExitX = 9;
            m_EndExitY = 15;
            m_PaletteID = 0;

            m_RoomData = @"0,0,0,i,j,e,0,a,b,0,f,r,q,0,0,0
0,0,0,h,0,e,0,c,d,0,f,0,p,0,0,0
0,0,0,g,0,e,0,a,b,0,f,0,o,0,0,0
0,0,0,m,n,e,0,c,d,0,f,v,u,0,0,0
0,0,0,l,0,e,0,a,b,0,f,0,t,0,0,0
0,0,0,k,0,e,0,c,d,0,f,0,s,0,0,0
0,0,0,i,j,e,0,a,b,0,f,r,q,0,0,0
0,0,0,h,0,e,0,c,d,0,f,0,p,0,0,0
0,0,0,g,0,e,0,a,b,0,f,0,o,0,0,0
0,0,0,m,n,e,0,c,d,0,f,v,u,0,0,0
0,0,0,l,0,e,0,a,b,0,f,0,t,0,0,0
0,0,0,k,0,e,0,c,d,0,f,0,s,0,0,0
0,0,0,i,j,e,0,a,b,0,f,r,q,0,0,0
0,0,0,h,0,e,0,c,d,0,f,0,p,0,0,0
0,0,0,g,0,e,0,a,b,0,f,0,o,0,0,0
0,0,0,0,0,e,0,c,d,0,f,0,0,0,0,0";
        }

        public void Generate(uint startRoomID, uint amountOfRooms)
        {
            //Generate all the data
            StringBuilder stringBuilder = new StringBuilder();

            for (uint roomID = startRoomID; roomID <= (startRoomID + amountOfRooms); ++roomID)
            {
                bool addExists = !(roomID == (startRoomID + amountOfRooms));

                GenerateRoom(roomID, stringBuilder, addExists);

                //Empty line n between rooms
                if (addExists == true)
                {
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(Environment.NewLine);
                }
            }

            //Write data to file
            WriteToFile(stringBuilder);
        }

        public void GenerateRoom(uint roomID, StringBuilder stringBuilder, bool addExits = true)
        {
            //Room ID
            stringBuilder.Append("ROOM ");
            stringBuilder.Append(roomID);
            stringBuilder.Append(Environment.NewLine);

            //Room Data
            stringBuilder.Append(m_RoomData);
            stringBuilder.Append(Environment.NewLine);

            //Room Name
            stringBuilder.Append("NAME road ");
            stringBuilder.Append(roomID);
            stringBuilder.Append(Environment.NewLine);

            //Exists
            if (addExits)
            {
                for (int y = m_StartExitY; y <= m_EndExitY; ++y)
                {
                    for (int x = m_StartExitX; x <= m_EndExitX; ++x)
                    {
                        stringBuilder.AppendFormat("EXT {0},{1} {2} {0},{1}", x, y, roomID + 1);
                        stringBuilder.Append(Environment.NewLine);
                    }
                }
            }

            //Palette
            stringBuilder.Append("PAL ");
            stringBuilder.Append(m_PaletteID);
        }

        private void WriteToFile(StringBuilder stringBuilder)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"./generated_rooms.txt");

            file.WriteLine(stringBuilder.ToString());

            file.Close();
        }
    }
}

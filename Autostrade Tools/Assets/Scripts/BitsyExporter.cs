﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BitsyExporter : MonoBehaviour
{
    public class Ending
    {
        private Vector2Int m_Position;
        public Vector2Int Position
        {
            get { return m_Position; }
        }

        private string m_Reason;
        public string Reason
        {
            get { return m_Reason; }
        }

        public Ending(Vector2Int position, string reason)
        {
            m_Position = position;
            m_Reason = reason;
        }

        public bool Equals(Vector2Int other)
        {
            return this.m_Position.x == other.x &&
                   this.m_Position.y == other.y;
        }

        public bool Equals(Ending other)
        {
            return this.m_Position.x == other.Position.x &&
                   this.m_Position.y == other.Position.y &&
                   this.m_Reason == other.Reason;
        }
    }

    [SerializeField]
    private LevelTimeline m_LevelTimeline = null;

    [SerializeField]
    private string m_GameName = string.Empty;

    [SerializeField]
    private string m_Filename = string.Empty;

    [SerializeField]
    private Color m_BackgroundColor = Color.white;

    [SerializeField]
    private Color m_TileColor = Color.white;

    [SerializeField]
    private Color m_SpriteColor = Color.white;

    //Some default data
    public static int s_PlayableZoneStartX = 6;
    public static int s_PlayableZoneStartY = 0;
    public static int s_PlayableZoneEndX = 9;
    public static int s_PlayableZoneEndY = 15;

    private static int s_RoomSize = 16;
    private static string[] s_EmptyStillRoomData =
    {
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","env03","env04","0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","env03","env04","0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","env03","env04","0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","env03","env04","0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0"
    };
    private static string[] s_EmptyAnimatedRoomData =
    {
        "0","0","0","env26","env28","env01","0","env05","env06","0","env02","env32","env30","0","0","0",
        "0","0","0","env17","env19","env01","0","env07","env08","0","env02","env23","env21","0","0","0",
        "0","0","0","env18","env20","env01","0","env05","env06","0","env02","env24","env22","0","0","0",
        "0","0","0","env25","env27","env01","0","env07","env08","0","env02","env31","env29","0","0","0",
        "0","0","0","env26","env28","env01","0","env05","env06","0","env02","env32","env30","0","0","0",
        "0","0","0","env17","env19","env01","0","env07","env08","0","env02","env23","env21","0","0","0",
        "0","0","0","env18","env20","env01","0","env05","env06","0","env02","env24","env22","0","0","0",
        "0","0","0","env25","env27","env01","0","env07","env08","0","env02","env31","env29","0","0","0",
        "0","0","0","env26","env28","env01","0","env05","env06","0","env02","env32","env30","0","0","0",
        "0","0","0","env17","env19","env01","0","env07","env08","0","env02","env23","env21","0","0","0",
        "0","0","0","env18","env20","env01","0","env05","env06","0","env02","env24","env22","0","0","0",
        "0","0","0","env25","env27","env01","0","env07","env08","0","env02","env31","env29","0","0","0",
        "0","0","0","env26","env28","env01","0","env05","env06","0","env02","env32","env30","0","0","0",
        "0","0","0","env17","env19","env01","0","env07","env08","0","env02","env23","env21","0","0","0",
        "0","0","0","env18","env20","env01","0","env05","env06","0","env02","env24","env22","0","0","0",
        "0","0","0","env25","env27","env01","0","env07","env08","0","env02","env31","env29","0","0","0"
    };
    private static string[] s_IntroRoomData =
    {
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","env03","env04","0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","env03","env04","0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","0"    ,"0"    ,"0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","0"    ,"0"    ,"0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","0"    ,"0"    ,"0","env02","0"    ,"0"    ,"0","0","0",
        "0","0","0","env09","env11","env01","0","env03","env04","0","env02","env15","env13","0","0","0",
        "0","0","0","env10","env12","env01","0","0"    ,"0"    ,"0","env02","env16","env14","0","0","0",
        "0","0","0","0"    ,"0"    ,"env01","0","env03","env04","0","env02","0"    ,"0"    ,"0","0","0"
    };
    private static string[] s_EndRoomData =
    {
        "hosp01","hosp12","hosp13","hosp07","hosp02","hosp01","hosp01","hosp28","hosp29","hosp01","hosp01","hosp03","hosp08","hosp20","hosp21","hosp01",
        "hosp14","hosp15","hosp16","hosp07","hosp04","hosp06","hosp06","hosp30","hosp31","hosp06","hosp06","hosp05","hosp08","hosp22","hosp23","hosp24",
        "hosp17","hosp18","hosp19","hosp07","0"     ,"0"     ,"0"     ,"hosp32","hosp33","0"     ,"0"     ,"0"     ,"hosp08","hosp25","hosp26","hosp27",
        "hosp11","hosp11","hosp11","hosp09","0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"vb06"  ,"0"     ,"hosp10","hosp11","hosp11","hosp11",
        "vp42"  ,"vp43"  ,"vp42"  ,"vp43"  ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"vc11"  ,"vc11"  ,"vp42"  ,"vp43",
        "va05"  ,"va06"  ,"va05"  ,"va06"  ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"vc12"  ,"vc12"  ,"vp44"  ,"vp45",
        "va07"  ,"va08"  ,"va07"  ,"va08"  ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"vp46"  ,"vp47",
        "0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0",
        "env33" ,"env33" ,"env33" ,"env33" ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"env33" ,"env33" ,"env33" ,"env33",
        "0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0",
        "env35" ,"0"     ,"env35" ,"0"     ,"env35" ,"0"     ,"env35" ,"0"     ,"env35" ,"0"     ,"env35" ,"0"     ,"env35" ,"0"     ,"env35" ,"0",
        "env36" ,"0"     ,"env36" ,"0"     ,"env36" ,"0"     ,"env36" ,"0"     ,"env36" ,"0"     ,"env36" ,"0"     ,"env36" ,"0"     ,"env36" ,"0",
        "0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"0",
        "env34" ,"env34" ,"env34" ,"env34" ,"env34" ,"env37" ,"0"     ,"0"     ,"env35" ,"env35" ,"env38" ,"env34" ,"env34" ,"env34" ,"env34" ,"env34",
        "0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"env01" ,"0"     ,"0"     ,"env36" ,"env36" ,"env02" ,"0"     ,"0"     ,"0"     ,"0"     ,"0",
        "0"     ,"0"     ,"0"     ,"0"     ,"0"     ,"env01" ,"0"     ,"0"     ,"0"     ,"0"     ,"env02" ,"0"     ,"0"     ,"0"     ,"0"     ,"0"
    };
    //Export functions
    public void Export()
    {
        StringBuilder stringBuilder = new StringBuilder();

        //Generate the string
        bool success = GenerateData(stringBuilder);

        //Write it to a file
        if (success)
        {
            WriteToFile(stringBuilder);
        }

        Debug.Log("Export successful!");
    }

    private bool GenerateData(StringBuilder stringBuilder)
    {
        //Name & Flags
        stringBuilder.AppendLine(m_GameName);
        stringBuilder.AppendLine();

        stringBuilder.AppendLine("# BITSY VERSION 6.4");
        stringBuilder.AppendLine();

        stringBuilder.AppendLine("! ROOM_FORMAT 1");
        stringBuilder.AppendLine();

        //Palettes
        bool success = ExportPalettes(stringBuilder);
        if (success == false) { return false; }

        stringBuilder.AppendLine(); //Whiteline

        //Rooms
        List<Ending> endings = new List<Ending>();
        success = ExportRooms(stringBuilder, ref endings);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        //Tiles
        success = ExportTiles(stringBuilder);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        //Sprites
        success = ExportPlayerSprite(stringBuilder);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        success = ExportSprites(stringBuilder);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        //Items
        success = ExportItems(stringBuilder);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        //Dialogue
        success = ExportDialogue(stringBuilder);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        //Endings
        success = ExportEndings(stringBuilder, endings);
        if (success == false) { return false; }

        stringBuilder.AppendLine();

        //Variables
        success = ExportVariables(stringBuilder);
        if (success == false) { return false; }

        return true;
    }

    private void WriteToFile(StringBuilder stringBuilder)
    {
        System.IO.StreamWriter file = new System.IO.StreamWriter(@"./" + m_Filename + ".bitsy");

        file.WriteLine(stringBuilder.ToString());

        file.Close();
    }

    //Utility
    private bool ExportPalettes(StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine("PAL 0");

        stringBuilder.AppendLine("0,27,63");
        stringBuilder.AppendLine("129,129,129");
        stringBuilder.AppendLine("255,255,255");

        /*
        stringBuilder.AppendFormat("{0},{1},{2}", (m_BackgroundColor.r * 255.0f), (m_BackgroundColor.g * 255.0f), (m_BackgroundColor.b * 255.0f));
        stringBuilder.Append(Environment.NewLine);

        stringBuilder.AppendFormat("{0},{1},{2}", (m_TileColor.r * 255.0f), (m_TileColor.g * 255.0f), (m_TileColor.b * 255.0f));
        stringBuilder.Append(Environment.NewLine);

        stringBuilder.AppendFormat("{0},{1},{2}", (m_SpriteColor.r * 255.0f), (m_SpriteColor.g * 255.0f), (m_SpriteColor.b * 255.0f));
        stringBuilder.Append(Environment.NewLine);
        */

        return true;
    }

    private bool ExportRooms(StringBuilder stringBuilder, ref List<Ending> globalEndings)
    {
        if (m_LevelTimeline == null)
            return false;

        //Loop trough all the rooms
        int originalFrame = m_LevelTimeline.CurrentFrame;

        //Loop trough all the frames (rooms)
        for (int roomID = m_LevelTimeline.TimelineMinRange; roomID <= m_LevelTimeline.TimelineMaxRange; ++roomID)
        {
            //Set the timeline to the correct frame (room)!
            m_LevelTimeline.SetFrame(roomID);

            bool addExists = !(roomID >= m_LevelTimeline.TimelineMaxRange);

            bool success = ExportRoom(stringBuilder, roomID, addExists, ref globalEndings);
            if (success == false) { return false; }

            if (addExists)
            {
                stringBuilder.AppendLine(); //Whiteline for the next room
            }
        }

        //Reset the timeline
        m_LevelTimeline.SetFrame(originalFrame);

        return true;
    }

    private bool ExportRoom(StringBuilder stringBuilder, int roomID, bool addExits, ref List<Ending> globalEndings)
    {
        List<Ending> localEndings = new List<Ending>();

        //Room ID
        stringBuilder.AppendFormat("ROOM {0}", roomID);
        stringBuilder.Append(Environment.NewLine);

        //Room Data
        ExportRoomData(stringBuilder, roomID, ref localEndings);
        stringBuilder.Append(Environment.NewLine);

        //Room Name
        stringBuilder.AppendFormat("NAME road {0}", roomID);
        stringBuilder.Append(Environment.NewLine);

        //Exists
        if (addExits)
        {
            for (int y = s_PlayableZoneStartY; y <= s_PlayableZoneEndY; ++y)
            {
                for (int x = s_PlayableZoneStartX; x <= s_PlayableZoneEndX; ++x)
                {
                    bool foundEnding = false;
                    foreach (Ending ending in localEndings)
                    {
                        if (ending.Position == new Vector2Int(x, y))
                        {
                            foundEnding = true;
                            break;
                        }
                    }

                    //Only add exists if there isn't an ending at that position
                    if (foundEnding == false)
                    {
                        stringBuilder.AppendFormat("EXT {0},{1} {2} {0},{1}", x, y, roomID + 1);
                        stringBuilder.Append(Environment.NewLine);
                    }
                }
            }
        }

        //Endings
        foreach(Ending ending in localEndings)
        {
            stringBuilder.AppendFormat("END {0} {1},{2}", globalEndings.Count - 1, ending.Position.x, ending.Position.y);
            stringBuilder.Append(Environment.NewLine);

            globalEndings.Add(ending); //Add to the global list
        }

        //Palette
        stringBuilder.Append("PAL 0"); //Simplified (no different palettes yet)
        stringBuilder.Append(Environment.NewLine);

        return true;
    }

    private void ExportRoomData(StringBuilder stringBuilder, int roomID, ref List<Ending> localEndings)
    {
        //We are already at the correct frame
        List<string> roomData = null;

        if (roomID == m_LevelTimeline.TimelineMinRange)
        {
            roomData = new List<string>(s_IntroRoomData);
        }
        else if (roomID == m_LevelTimeline.TimelineMaxRange)
        {
            roomData = new List<string>(s_EndRoomData);
        }
        else
        {
            roomData = new List<string>(s_EmptyAnimatedRoomData);
        }

        //Find all the vehicles & write the room data
        Vehicle[] vehicles = GameObject.FindObjectsOfType<Vehicle>(); //I'm not a fan of GetComponent, but in a 1 off function like this it's fine. (Otherwise human error and misassignement will take over)

        foreach(Vehicle vehicle in vehicles)
        {
            //Offset the positions so 0,0 is the top left corner (counting up to the bottom & right)
            //NOTE: If a vehicle is bigger than 1 tile, top left is also the origin
            int posX = (int)vehicle.transform.position.x;
            int posY = (int)vehicle.transform.position.y * -1;

            //Add tile references to the room data
            List<string> bitsyTileReferences = vehicle.GetBitsyTileReferences();
            for (int heightY = 0; heightY < vehicle.Height; ++heightY)
            {
                for (int widthX = 0; widthX < vehicle.Width; ++widthX)
                {
                    //Check if it's off screen, if so ignore
                    if (posX + widthX  >= 0 && posX + widthX  < s_RoomSize &&
                        posY + heightY >= 0 && posY + heightY < s_RoomSize)
                    {
                        roomData[posX + ((posY + heightY) * s_RoomSize) + widthX] = bitsyTileReferences[widthX + (heightY * vehicle.Width)];
                    }
                }
            }

            //Hitboxes
            List<Vector2Int> hitboxes = vehicle.GetHitboxes();

            foreach(Vector2Int hitbox in hitboxes)
            {
                //Check if it's off screen, if so ignore
                if (hitbox.x >= 0 && hitbox.x < s_RoomSize &&
                    hitbox.y >= 0 && hitbox.y < s_RoomSize)
                {
                    bool foundEnding = false;
                    foreach(Ending ending in localEndings)
                    {
                        if (ending.Position == hitbox)
                        {
                            foundEnding = true;
                            break;

                        }
                    }
                    
                    if (foundEnding == false)
                        localEndings.Add(new Ending(hitbox, vehicle.GetRandomEnding()));
                }
            }
        }

        //Write the room data to the stringbuilder
        for (int y = 0; y < s_RoomSize; ++y)
        {
            for (int x = 0; x < s_RoomSize; ++x)
            {
                stringBuilder.Append(roomData[x + (y * s_RoomSize)]);

                //Add a comma for the next one, or end the line
                if (x < s_RoomSize - 1)      { stringBuilder.Append(","); }
                else if (y < s_RoomSize - 1) { stringBuilder.Append(Environment.NewLine); }
            }
        }
    }


    private bool ExportTiles(StringBuilder stringBuilder)
    {
        //We don't actually read texture data or anything, just shove this datablob in there
        //NOTE: Don't tab! Otherwise it will add whitespace that we don't need
        stringBuilder.AppendLine(@"TIL env01
11000000
11000000
11000000
11000000
11000000
11000000
11000000
11000000
NAME solid_l
WAL true

TIL env02
00000011
00000011
00000011
00000011
00000011
00000011
00000011
00000011
NAME solid_r
WAL true

TIL env03
00000001
00000001
00000001
00000001
00000001
00000001
00000001
00000001
NAME dot_l

TIL env04
10000000
10000000
10000000
10000000
10000000
10000000
10000000
10000000
NAME dot_r

TIL env05
00000001
00000001
00000001
00000001
00000001
00000001
00000001
00000001
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME dot_l_f1

TIL env06
10000000
10000000
10000000
10000000
10000000
10000000
10000000
10000000
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME dot_r_f1

TIL env07
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
00000001
00000001
00000001
00000001
00000001
00000001
00000001
00000001
NAME dot_l_f2

TIL env08
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
10000000
10000000
10000000
10000000
10000000
10000000
10000000
10000000
NAME dot_r_f2

TIL env09
00000000
00000000
00000000
00000000
00000000
00111100
01100110
01011001
NAME light_l1

TIL env10
01011001
01100110
00111100
00000000
00000000
00000000
00000000
00000000
NAME light_l2

TIL env11
00000000
00000000
00000000
00000000
00000000
00000000
01111100
11000010
NAME light_l3

TIL env12
11000010
01111100
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_l4

TIL env13
00000000
00000000
00000000
00000000
00000000
00111100
01100110
10011010
NAME light_r1

TIL env14
10011010
01100110
00111100
00000000
00000000
00000000
00000000
00000000
NAME light_r2

TIL env15
00000000
00000000
00000000
00000000
00000000
00000000
00111110
01000011
NAME light_r3

TIL env16
01000011
00111110
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_r4

TIL env17
00000000
00000000
00000000
00000000
00000000
00111100
01100110
01011001
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_l1f1

TIL env18
01011001
01100110
00111100
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_l2f1

TIL env19
00000000
00000000
00000000
00000000
00000000
00000000
01111100
11000010
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_l3f1

TIL env20
11000010
01111100
00000000
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_l4f1

TIL env21
00000000
00000000
00000000
00000000
00000000
00111100
01100110
10011010
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_r1f1

TIL env22
10011010
01100110
00111100
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_r2f1

TIL env23
00000000
00000000
00000000
00000000
00000000
00000000
00111110
01000011
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_r3f1

TIL env24
01000011
00111110
00000000
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_r4f1

TIL env25
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00111100
01100110
01011001
NAME light_l1f2

TIL env26
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
01011001
01100110
00111100
00000000
00000000
00000000
00000000
00000000
NAME light_l2f2

TIL env27
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00000000
01111100
11000010
NAME light_l3f2

TIL env28
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
11000010
01111100
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_l1f2

TIL env29
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00111100
01100110
10011010
NAME light_r1f2

TIL env30
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
10011010
01100110
00111100
00000000
00000000
00000000
00000000
00000000
NAME light_r2f2

TIL env31
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
00000000
00000000
00000000
00000000
00000000
00000000
00111110
01000011
NAME light_r3f2

TIL env32
00000000
00000000
00000000
00000000
00000000
00000000
00000000
00000000
>
01000011
00111110
00000000
00000000
00000000
00000000
00000000
00000000
NAME light_r4f2

TIL env33
11111111
11111111
00000000
00000000
00000000
00000000
00000000
00000000
NAME solid_t
WAL true

TIL env34
00000000
00000000
00000000
00000000
00000000
00000000
11111111
11111111
NAME solid_b
WAL true

TIL env35
00000000
00000000
00000000
00000000
00000000
00000000
00000000
11111111
NAME dot_t

TIL env36
11111111
00000000
00000000
00000000
00000000
00000000
00000000
00000000
NAME dot_b

TIL env37
00000000
00000000
00000000
00000000
00000000
00000000
10000000
11000000
NAME corner_l
WAL true

TIL env38
00000000
00000000
00000000
00000000
00000000
00000000
00000001
00000011
NAME corner_r
WAL true

TIL hosp01
11111111
11111111
11111111
11111111
11111111
11111111
11111111
11111111
NAME hosp_01
WAL true

TIL hosp02
01111111
01111111
01111111
01111111
01111111
01111111
01111111
01111111
NAME hosp_02
WAL true

TIL hosp03
11111110
11111110
11111110
11111110
11111110
11111110
11111110
11111110
NAME hosp_03
WAL true

TIL hosp04
01111111
01111111
01111111
01111111
01111111
01111111
00000000
11111111
NAME hosp_04
WAL true

TIL hosp05
11111110
11111110
11111110
11111110
11111110
11111110
00000000
11111111
NAME hosp_05
WAL true

TIL hosp06
11111111
11111111
11111111
11111111
11111111
11111111
00000000
11111111
NAME hosp_06
WAL true

TIL hosp07
11111101
11111101
11111101
11111101
11111101
11111101
11111101
11111101
NAME hosp_07
WAL true

TIL hosp08
10111111
10111111
10111111
10111111
10111111
10111111
10111111
10111111
NAME hosp_08
WAL true

TIL hosp09
11111101
11111101
11111101
11111101
00000011
11111110
00000000
00000000
NAME hosp_09
WAL true

TIL hosp10
10111111
10111111
10111111
10111111
11000000
01111111
00000000
00000000
NAME hosp_10
WAL true

TIL hosp11
11111111
11111111
11111111
11111111
00000000
11111111
00000000
00000000
NAME hosp_11
WAL true

TIL hosp12
11111111
11111111
11111111
11111111
11111111
11110000
11101111
11101000
NAME hosp_cross_l_1
WAL true

TIL hosp13
11111111
11111111
11111111
11111111
11111111
00111111
11011111
01011111
NAME hosp_cross_l_2
WAL true

TIL hosp14
11111111
11111111
11111110
11111101
11111101
11111101
11111101
11111101
NAME hosp_cross_l_3
WAL true

TIL hosp15
11101000
11101000
00001000
11111000
00000000
00000000
00000000
00000000
NAME hosp_cross_l_4
WAL true

TIL hosp16
01011111
01011111
01000001
01111110
00000010
00000010
00000010
00000010
NAME hosp_cross_l_5
WAL true

TIL hosp17
11111101
11111110
11111111
11111111
11111111
11111111
11111111
11111111
NAME hosp_cross_l_6
WAL true

TIL hosp18
11111000
00001000
11101000
11101000
11101000
11101111
11110000
11111111
NAME hosp_cross_l_7
WAL true

TIL hosp19
01111110
01000001
01011111
01011111
01011111
11011111
00111111
11111111
NAME hosp_cross_l_8
WAL true

TIL hosp20
11111111
11111111
11111111
11111111
11111111
11111100
11111011
11111010
NAME hosp_cross_r_1
WAL true

TIL hosp21
11111111
11111111
11111111
11111111
11111111
00001111
11110111
00010111
NAME hosp_cross_r_2
WAL true

TIL hosp22
11111010
11111010
10000010
01111110
01000000
01000000
01000000
01000000
NAME hosp_cross_r_3
WAL true

TIL hosp23
00010111
00010111
00010000
00011111
00000000
00000000
00000000
00000000
NAME hosp_cross_r_4
WAL true

TIL hosp24
11111111
11111111
01111111
10111111
10111111
10111111
10111111
10111111
NAME hosp_cross_r_5
WAL true

TIL hosp25
01111110
10000010
11111010
11111010
11111010
11111011
11111100
11111111
NAME hosp_cross_r_6
WAL true

TIL hosp26
00011111
00010000
00010111
00010111
00010111
11110111
00001111
11111111
NAME hosp_cross_r_7
WAL true

TIL hosp27
10111111
01111111
11111111
11111111
11111111
11111111
11111111
11111111
NAME hosp_cross_r_8
WAL true

TIL hosp28
11111111
11111111
11110001
11111011
11111011
11111011
11110001
11111111
NAME hosp_in_01
WAL true

TIL hosp29
11111111
11111111
01101111
00101111
01001111
01101111
01101111
11111111
NAME hosp_in_02
WAL true

TIL hosp30
11111000
11111100
11111110
11111111
11111111
11111111
00000000
11111111
>
11111111
11111000
11111100
11111110
11111111
11111111
00000000
11111111
NAME hosp_in_03
WAL true

TIL hosp31
00011111
00111111
01111111
11111111
11111111
11111111
00000000
11111111
>
11111111
00011111
00111111
01111111
11111111
11111111
00000000
11111111
NAME hosp_in_04
WAL true

TIL hosp32
10000000
10000000
01000000
00110000
00001100
00000011
00000000
00000000
NAME hosp_gate_l

TIL hosp33
00000001
00000001
00000010
00001100
00110000
11000000
00000000
00000000
NAME hosp_gate_r

TIL vb01
00001000
00111110
01010101
00001000
00011100
00011100
00001000
00001000
>
00001000
00001000
00111110
01010101
00001000
00011100
00011100
00001000
NAME bike

TIL vb02
00010001
01111101
10101011
00010001
00111001
00111001
00010001
00010001
>
00010000
00010000
01111100
10101010
00010000
00111000
00010000
00010000
NAME bike_p2f1

TIL vb03
10001000
10111110
11010101
10001000
10011100
10011100
10001000
10001000
>
00001000
00001000
00111110
01010101
00001000
00011100
00011100
00001000
NAME bike_p3f1

TIL vb04
00010000
01111100
10101010
00010000
00111000
00111000
00010000
00010000
>
00010001
00010001
01111101
10101011
00010001
00111001
00010001
00010001
NAME bike_p2f2

TIL vb05
00001000
00111110
01010101
00001000
00011100
00011100
00001000
00001000
>
10001000
10001000
10111110
11010101
10001000
10011100
10011100
10001000
NAME bike_p3f2

TIL vb06
00001000
00111110
01010101
00001000
00011100
00011100
00001000
00001000
NAME bike_static
WAL true

TIL vc01
00111100
01111110
01111110
01000010
10111101
11111111
11111111
01111110
>
00000000
00111100
01111110
01111110
11000011
10111101
11111111
01111110
NAME car1

TIL vc02
01111110
01111110
11111111
10111101
11000011
01111110
00111100
00000000
>
01111110
01111110
11111111
11111111
10111101
01000010
01111110
00111100
NAME car2

TIL vc03
00111101
01111111
01111111
01000011
10111101
11111111
11111111
01111111
>
00000000
00111100
01111110
01111110
11000011
10111101
11111111
01111110
NAME car1_p2f1

TIL vc04
01111110
01111110
11111111
10111101
11000011
01111110
00111100
00000000
>
01111111
01111111
11111111
11111111
10111101
01000011
01111111
00111101
NAME car2_p2f1

TIL vc05
10111100
11111110
11111110
11000010
10111101
11111111
11111111
11111110
>
00000000
00111100
01111110
01111110
11000011
10111101
11111111
01111110
NAME car1_p3f1

TIL vc06
01111110
01111110
11111111
10111101
11000011
01111110
00111100
00000000
>
11111110
11111110
11111111
11111111
10111101
11000010
11111110
10111100
NAME car2_p3f1

TIL vc07
00111100
01111110
01111110
01000010
10111101
11111111
11111111
01111110
>
00000001
00111101
01111111
01111111
11000011
10111101
11111111
01111111
NAME car1_p2f2

TIL vc08
01111111
01111111
11111111
10111101
11000011
01111111
00111101
00000001
>
01111110
01111110
11111111
11111111
10111101
01000010
01111110
00111100
NAME car2_p2f2

TIL vc09
00111100
01111110
01111110
01000010
10111101
11111111
11111111
01111110
>
10000000
10111100
11111110
11111110
11000011
10111101
11111111
11111110
NAME car1_p3f2

TIL vc10
11111110
11111110
11111111
10111101
11000011
11111110
10111100
10000000
>
01111110
01111110
11111111
11111111
10111101
01000010
01111110
00111100
NAME car2_p3f2

TIL vc11
00111100
01111110
01111110
01000010
10111101
11111111
11111111
01111110
NAME car1_static
WAL true

TIL vc12
01111110
01111110
11111111
10111101
11000011
01111110
00111100
00000000
NAME car2_static
WAL true

TIL vp01
00000000
00000000
00000001
00000001
00000001
00000001
00000001
00000010
>
00000000
00000000
00000000
00000001
00000001
00000001
00000001
00000011
NAME pick1

TIL vp02
11111111
11111111
11111111
10000001
01111110
11111111
11111111
11111111
>
00000000
11111111
11111111
11111111
10000001
01111110
11111111
11111111
NAME pick2

TIL vp03
00000000
00000000
10000000
10000000
10000000
10000000
10000000
01000000
>
00000000
00000000
00000000
10000000
10000000
10000000
10000000
11000000
NAME pick3

TIL vp04
00000010
00000011
00000001
00000001
00000001
00000001
00000001
00000001
>
00000010
00000010
00000001
00000001
00000001
00000001
00000001
00000001
NAME pick4

TIL vp05
10000001
11111111
10000001
01111110
00000000
01111110
00000000
01111110
>
11111111
10000001
11111111
10000001
01111110
00000000
01111110
00000000
NAME pick5

TIL vp06
01000000
11000000
10000000
10000000
10000000
10000000
10000000
10000000
>
01000000
01000000
10000000
10000000
10000000
10000000
10000000
10000000
NAME pick6

TIL vp07
00000011
00000011
00000011
00000001
00000001
00000001
00000000
00000000
>
00000011
00000011
00000011
00000001
00000001
00000001
00000001
00000000
NAME pick7

TIL vp08
00000000
01111110
00000000
01111110
10000001
11111111
00000000
00000000
>
01111110
00000000
01111110
00000000
01111110
10000001
11111111
00000000
NAME pick8

TIL vp09
11000000
11000000
11000000
10000000
10000000
10000000
00000000
00000000
>
11000000
11000000
11000000
10000000
10000000
10000000
10000000
00000000
NAME pick9

TIL vp10
11000000
11000000
11000001
11000001
11000001
11000001
11000001
11000010
>
11000000
11000000
11000000
11000001
11000001
11000001
11000001
11000011
NAME pick1_p1

TIL vp11
00000001
00000001
10000001
10000001
10000001
10000001
10000001
01000001
>
00000000
00000000
00000000
10000000
10000000
10000000
10000000
11000000
NAME pick3_p1f1

TIL vp12
11000010
11000011
11000001
11000001
11000001
11000001
11000001
11000001
>
11000010
11000010
11000001
11000001
11000001
11000001
11000001
11000001
NAME pick4_p1

TIL vp13
01000000
11000000
10000000
10000000
10000000
10000000
10000000
10000000
>
01000001
01000001
10000001
10000001
10000001
10000001
10000001
10000001
NAME pick6_p1f1

TIL vp14
11000011
11000011
11000011
11000001
11000001
11000001
11000000
11000000
>
11000011
11000011
11000011
11000001
11000001
11000001
11000001
11000000
NAME pick7_p1

TIL vp15
11000001
11000001
11000001
10000001
10000001
10000001
00000001
00000001
>
11000000
11000000
11000000
10000000
10000000
10000000
10000000
00000000
NAME pick9_p1f1

TIL vp16
10000000
10000000
10000000
10000000
10000000
10000000
10000000
01000000
>
00000000
00000000
00000000
10000000
10000000
10000000
10000000
11000000
NAME pick3_p2f1

TIL vp17
00000000
01111110
00000000
01111110
10000001
11111111
00000001
00000001
>
01111110
00000000
01111110
00000000
01111110
10000001
11111111
00000000
NAME pick8_p2f1

TIL vp18
11000000
11000000
11000000
10000000
10000000
10000000
10000000
10000000
>
11000000
11000000
11000000
10000000
10000000
10000000
10000000
00000000
NAME pick9_p2f1

TIL vp19
00000001
00000001
00000001
00000001
00000001
00000001
00000001
00000010
>
00000000
00000000
00000000
00000001
00000001
00000001
00000001
00000011
NAME pick1_p3f1

TIL vp20
00000011
00000011
00000011
00000001
00000001
00000001
00000001
00000001
>
00000011
00000011
00000011
00000001
00000001
00000001
00000001
00000000
NAME pick7_p3f1

TIL vp21
00000000
01111110
00000000
01111110
10000001
11111111
10000000
10000000
>
01111110
00000000
01111110
00000000
01111110
10000001
11111111
00000000
NAME pick8_p3f1

TIL vp22
10000000
10000000
10000001
10000001
10000001
10000001
10000001
10000010
>
00000000
00000000
00000000
00000001
00000001
00000001
00000001
00000011
NAME pick1_p4f1

TIL vp23
00000011
00000011
10000011
10000011
10000011
10000011
10000011
01000011
>
00000011
00000011
00000011
10000011
10000011
10000011
10000011
11000011
NAME pick3_p4

TIL vp24
00000010
00000011
00000001
00000001
00000001
00000001
00000001
00000001
>
10000010
10000010
10000001
10000001
10000001
10000001
10000001
10000001
NAME pick4_p4f1

TIL vp25
01000011
11000011
10000011
10000011
10000011
10000011
10000011
10000011
>
01000011
01000011
10000011
10000011
10000011
10000011
10000011
10000011
NAME pick6_p4

TIL vp26
10000011
10000011
10000011
10000001
10000001
10000001
10000000
10000000
>
00000011
00000011
00000011
00000001
00000001
00000001
00000001
00000000
NAME pick7_p4f1

TIL vp27
11000011
11000011
11000011
10000011
10000011
10000011
00000011
00000011
>
11000011
11000011
11000011
10000011
10000011
10000011
10000011
00000011
NAME pick9_p4

TIL vp28
00000000
00000000
10000000
10000000
10000000
10000000
10000000
01000000
>
00000001
00000001
00000001
10000001
10000001
10000001
10000001
11000001
NAME pick3_p1f2

TIL vp29
01000001
11000001
10000001
10000001
10000001
10000001
10000001
10000001
>
01000000
01000000
10000000
10000000
10000000
10000000
10000000
10000000
NAME pick6_p1f2

TIL vp30
11000000
11000000
11000000
10000000
10000000
10000000
00000000
00000000
>
11000001
11000001
11000001
10000001
10000001
10000001
10000001
00000001
NAME pick9_p1f2

TIL vp31
11111111
11111111
11111111
10000001
01111110
11111111
11111111
11111111
>
00000001
11111111
11111111
11111111
10000001
01111110
11111111
11111111
NAME pick2_p2f2

TIL vp32
00000000
00000000
10000000
10000000
10000000
10000000
10000000
01000000
>
10000000
10000000
10000000
10000000
10000000
10000000
10000000
11000000
NAME pick3_p2f2

TIL vp33
00000000
01111110
00000000
01111110
10000001
11111111
00000000
00000000
>
01111110
00000000
01111110
00000000
01111110
10000001
11111111
00000001
NAME pick8_p2f2

TIL vp34
11000000
11000000
11000000
10000000
10000000
10000000
00000000
00000000
>
11000000
11000000
11000000
10000000
10000000
10000000
10000000
10000000
NAME pick9_p2f2

TIL vp35
00000000
00000000
00000001
00000001
00000001
00000001
00000001
00000010
>
00000001
00000001
00000001
00000001
00000001
00000001
00000001
00000011
NAME pick1_p3f2

TIL vp36
11111111
11111111
11111111
10000001
01111110
11111111
11111111
11111111
>
10000000
11111111
11111111
11111111
10000001
01111110
11111111
11111111
NAME pick2_p3f2

TIL vp37
00000011
00000011
00000011
00000001
00000001
00000001
00000000
00000000
>
00000011
00000011
00000011
00000001
00000001
00000001
00000001
00000001
NAME pick7_p3f2

TIL vp38
00000000
01111110
00000000
01111110
10000001
11111111
00000000
00000000
>
01111110
00000000
01111110
00000000
01111110
10000001
11111111
10000000
NAME pick8_p3f2

TIL vp39
00000000
00000000
00000001
00000001
00000001
00000001
00000001
00000010
>
10000000
10000000
10000000
10000001
10000001
10000001
10000001
10000011
NAME pick1_p4f2

TIL vp40
10000010
10000011
10000001
10000001
10000001
10000001
10000001
10000001
>
00000010
00000010
00000001
00000001
00000001
00000001
00000001
00000001
NAME pick4_p4f2

TIL vp41
00000011
00000011
00000011
00000001
00000001
00000001
00000000
00000000
>
10000011
10000011
10000011
10000001
10000001
10000001
10000001
10000000
NAME pick7_p4f2

TIL vp42
00001111
00001111
00011111
00011000
00010111
00011111
00011111
00101111
NAME pick1_static
WAL true

TIL vp43
11110000
11110000
11111000
00011000
11101000
11111000
11111000
11110100
NAME pick2_static
WAL true

TIL vp44
00101000
00111111
00011000
00010111
00010000
00010111
00010000
00010111
NAME pick3_static
WAL true

TIL vp45
00010100
11111100
00011000
11101000
00001000
11101000
00001000
11101000
NAME pick4_static
WAL true

TIL vp46
00110000
00110111
00110000
00010111
00011000
00011111
00000000
00000000
NAME pick5_static
WAL true

TIL vp47
00001100
11101100
00001100
11101000
00011000
11111000
00000000
00000000
NAME pick6_static
WAL true

TIL va01
10000001
11111111
11111111
11111111
11111111
11100111
11100111
10000001
>
11111111
10000001
11111111
11111111
11111111
11111111
11100111
11100111
NAME ambu5

TIL va02
10000001
11100111
11100111
11111111
11111111
11111111
11111111
00000000
>
10000001
10000001
11100111
11100111
11111111
11111111
11111111
11111111
NAME ambu8

TIL va03
10000001
11100111
11100111
11111111
11111111
11111111
11111111
00000001
>
10000001
10000001
11100111
11100111
11111111
11111111
11111111
11111111
NAME ambu8_p2f1

TIL va04
10000001
11100111
11100111
11111111
11111111
11111111
11111111
10000000
>
10000001
10000001
11100111
11100111
11111111
11111111
11111111
11111111
NAME ambu8_p3f1

TIL va05
00101000
00011111
00010000
00010000
00010000
00010001
00010001
00010111
NAME ambu3_static
WAL true

TIL va06
00010100
11111000
00001000
00001000
00001000
10001000
10001000
11101000
NAME ambu4_static
WAL true

TIL va07
00110111
00110001
00110001
00010000
00010000
00010000
00001111
00000000
NAME ambu5_static
WAL true

TIL va08
11101100
10001100
10001100
00001000
00001000
00001000
11110000
00000000
NAME ambu6_static
WAL true

TIL vt01
00000000
00000001
00000011
00001110
00010111
00000101
00001101
00001101
>
00000000
00000000
00000001
00000011
00001110
00010111
00001101
00001101
NAME truck1

TIL vt02
11111111
11111111
00000000
11111111
11111111
11111111
11111111
11111111
>
00000000
11111111
11111111
00000000
11111111
11111111
11111111
11111111
NAME truck2

TIL vt03
00000000
10000000
11000000
01110000
11101000
10100000
10110000
10110000
>
00000000
00000000
10000000
11000000
01110000
11101000
10110000
10110000
NAME truck3

TIL vt04
00001111
00000001
00000111
00000110
00000101
00000100
00000100
00000101
>
00001101
00000111
00000001
00000111
00000110
00000101
00000100
00000100
NAME truck4

TIL vt05
11111111
11111111
11111111
00000000
11111111
00000000
00000000
11111111
>
11111111
11111111
11111111
11111111
00000000
11111111
00000000
00000000
NAME truck5

TIL vt06
11110000
11000000
11100000
01100000
10100000
00100000
00100000
10100000
>
10110000
11100000
11000000
11100000
01100000
10100000
00100000
00100000
NAME truck6

TIL vt07
00000100
00000100
00000101
00000100
00000100
00000101
00000100
00000100
>
00000101
00000100
00000100
00000101
00000100
00000100
00000101
00000100
NAME truck7

TIL vt08
00000000
00000000
11111111
00000000
00000000
11111111
00000000
00000000
>
11111111
00000000
00000000
11111111
00000000
00000000
11111111
00000000
NAME truck8

TIL vt09
00100000
00100000
10100000
00100000
00100000
10100000
00100000
00100000
>
10100000
00100000
00100000
10100000
00100000
00100000
10100000
00100000
NAME truck9

TIL vt10
00000101
00000100
00000100
00000101
00001100
00001100
00001101
00000100
>
00000100
00000101
00000100
00000100
00001101
00001100
00001100
00000101
NAME truck10

TIL vt11
11111111
00000000
00000000
11111111
00000000
00000000
11111111
00000000
>
00000000
11111111
00000000
00000000
11111111
00000000
00000000
11111111
NAME truck11

TIL vt12
10100000
00100000
00100000
10100000
00110000
00110000
10110000
00100000
>
00100000
10100000
00100000
00100000
10110000
00110000
00110000
10100000
NAME truck12

TIL vt13
00000100
00001101
00001100
00001100
00000101
00000110
00000111
00000000
>
00000100
00001100
00001101
00001100
00000100
00000101
00000110
00000111
NAME truck13

TIL vt14
00000000
11111111
00000000
00000000
11111111
00000000
11111111
00000000
>
00000000
00000000
11111111
00000000
00000000
11111111
00000000
11111111
NAME truck14

TIL vt15
00100000
10110000
00110000
00110000
10100000
01100000
11100000
00000000
>
00100000
00110000
10110000
00110000
00100000
10100000
01100000
11100000
NAME truck15

TIL vt16
11000000
11000001
11000011
11001110
11010111
11000101
11001101
11001101
>
11000000
11000000
11000001
11000011
11001110
11010111
11001101
11001101
NAME truck1_p1

TIL vt17
00000001
10000001
11000001
01110001
11101001
10100001
10110001
10110001
>
00000000
00000000
10000000
11000000
01110000
11101000
10110000
10110000
NAME truck3_p1f1

TIL vt18
11001111
11000001
11000111
11000110
11000101
11000100
11000100
11000101
>
11001101
11000111
11000001
11000111
11000110
11000101
11000100
11000100
NAME truck4_p1

TIL vt19
11110000
11000000
11100000
01100000
10100000
00100000
00100000
10100000
>
10110001
11100001
11000001
11100001
01100001
10100001
00100001
00100001
NAME truck6_p1f1

TIL vt20
11000100
11000100
11000101
11000100
11000100
11000101
11000100
11000100
>
11000101
11000100
11000100
11000101
11000100
11000100
11000101
11000100
NAME truck7_p1

TIL vt21
00100001
00100001
10100001
00100001
00100001
10100001
00100001
00100001
>
10100000
00100000
00100000
10100000
00100000
00100000
10100000
00100000
NAME truck9_p1f1

TIL vt22
11000101
11000100
11000100
11000101
11001100
11001100
11001101
11000100
>
11000100
11000101
11000100
11000100
11001101
11001100
11001100
11000101
NAME truck10_p1

TIL vt23
10100000
00100000
00100000
10100000
00110000
00110000
10110000
00100000
>
00100001
10100001
00100001
00100001
10110001
00110001
00110001
10100001
NAME truck12_p1f1

TIL vt24
11000100
11001101
11001100
11001100
11000101
11000110
11000111
11000000
>
11000100
11001100
11001101
11001100
11000100
11000101
11000110
11000111
NAME truck13_p1

TIL vt25
00100001
10110001
00110001
00110001
10100001
01100001
11100001
00000001
>
00100000
00110000
10110000
00110000
00100000
10100000
01100000
11100000
NAME truck15_p1f1

TIL vt26
10000000
10000000
11000000
01110000
11101000
10100000
10110000
10110000
>
00000000
00000000
10000000
11000000
01110000
11101000
10110000
10110000
NAME truck3_p2f1

TIL vt27
00000000
11111111
00000000
00000000
11111111
00000000
11111111
00000001
>
00000000
00000000
11111111
00000000
00000000
11111111
00000000
11111111
NAME truck14_p2f1

TIL vt28
00100000
10110000
00110000
00110000
10100000
01100000
11100000
10000000
>
00100000
00110000
10110000
00110000
00100000
10100000
01100000
11100000
NAME truck15_p2f1

TIL vt29
00000001
00000001
00000011
00001110
00010111
00000101
00001101
00001101
>
00000000
00000000
00000001
00000011
00001110
00010111
00001101
00001101
NAME truck1_p3f1

TIL vt30
00000100
00001101
00001100
00001100
00000101
00000110
00000111
00000001
>
00000100
00001100
00001101
00001100
00000100
00000101
00000110
00000111
NAME truck13_p3f1

TIL vt31
00000000
11111111
00000000
00000000
11111111
00000000
11111111
10000000
>
00000000
00000000
11111111
00000000
00000000
11111111
00000000
11111111
NAME truck14_p3f1

TIL vt32
10000000
10000001
10000011
10001110
10010111
10000101
10001101
10001101
>
00000000
00000000
00000001
00000011
00001110
00010111
00001101
00001101
NAME truck1_p4f1

TIL vt33
00000011
10000011
11000011
01110011
11101011
10100011
10110011
10110011
>
00000011
00000011
10000011
11000011
01110011
11101011
10110011
10110011
NAME truck3_p4

TIL vt34
00001111
00000001
00000111
00000110
00000101
00000100
00000100
00000101
>
10001101
10000111
10000001
10000111
10000110
10000101
10000100
10000100
NAME truck4_p4f1

TIL vt35
11110011
11000011
11100011
01100011
10100011
00100011
00100011
10100011
>
10110011
11100011
11000011
11100011
01100011
10100011
00100011
00100011
NAME truck6_p4

TIL vt36
10000100
10000100
10000101
10000100
10000100
10000101
10000100
10000100
>
00000101
00000100
00000100
00000101
00000100
00000100
00000101
00000100
NAME truck7_p4f1

TIL vt37
00100011
00100011
10100011
00100011
00100011
10100011
00100011
00100011
>
10100011
00100011
00100011
10100011
00100011
00100011
10100011
00100011
NAME truck9_p4

TIL vt38
00000101
00000100
00000100
00000101
00001100
00001100
00001101
00000100
>
10000100
10000101
10000100
10000100
10001101
10001100
10001100
10000101
NAME truck10_p4f1

TIL vt39
10100011
00100011
00100011
10100011
00110011
00110011
10110011
00100011
>
00100011
10100011
00100011
00100011
10110011
00110011
00110011
10100011
NAME truck12_p4

TIL vt40
10000100
10001101
10001100
10001100
10000101
10000110
10000111
10000000
>
00000100
00001100
00001101
00001100
00000100
00000101
00000110
00000111
NAME truck13_p4f1

TIL vt41
00100011
10110011
00110011
00110011
10100011
01100011
11100011
00000011
>
00100011
00110011
10110011
00110011
00100011
10100011
01100011
11100011
NAME truck15_p4

TIL vt42
00000000
10000000
11000000
01110000
11101000
10100000
10110000
10110000
>
00000001
00000001
10000001
11000001
01110001
11101001
10110001
10110001
NAME truck3_p1f2

TIL vt43
11110001
11000001
11100001
01100001
10100001
00100001
00100001
10100001
>
10110000
11100000
11000000
11100000
01100000
10100000
00100000
00100000
NAME truck6_p1f2

TIL vt44
00100000
00100000
10100000
00100000
00100000
10100000
00100000
00100000
>
10100001
00100001
00100001
10100001
00100001
00100001
10100001
00100001
NAME truck9_p1f2

TIL vt45
10100001
00100001
00100001
10100001
00110001
00110001
10110001
00100001
>
00100000
10100000
00100000
00100000
10110000
00110000
00110000
10100000
NAME truck12_p1f2

TIL vt46
00100000
10110000
00110000
00110000
10100000
01100000
11100000
00000000
>
00100001
00110001
10110001
00110001
00100001
10100001
01100001
11100001
NAME truck15_p1f2

TIL vt47
11111111
11111111
00000000
11111111
11111111
11111111
11111111
11111111
>
00000000
11111111
11111111
00000000
11111111
11111111
11111111
11111111
NAME truck2_p2f2

TIL vt48
00000000
10000000
11000000
01110000
11101000
10100000
10110000
10110000
>
00000000
00000000
10000000
11000000
01110000
11101000
10110000
10110000
NAME truck3_p2f2

TIL vt49
00000000
00000001
00000011
00001110
00010111
00000101
00001101
00001101
>
00000001
00000001
00000001
00000011
00001110
00010111
00001101
00001101
NAME truck1_p3f2

TIL vt50
11111111
11111111
00000000
11111111
11111111
11111111
11111111
11111111
>
10000000
11111111
11111111
00000000
11111111
11111111
11111111
11111111
NAME truck2_p3f2

TIL vt51
00000000
00000001
00000011
00001110
00010111
00000101
00001101
00001101
>
10000000
10000000
10000001
10000011
10001110
10010111
10001101
10001101
NAME truck1_p4f1

TIL vt52
10001111
10000001
10000111
10000110
10000101
10000100
10000100
10000101
>
00001101
00000111
00000001
00000111
00000110
00000101
00000100
00000100
NAME truck4_p4f1

TIL vt53
00000100
00000100
00000101
00000100
00000100
00000101
00000100
00000100
>
10000101
10000100
10000100
10000101
10000100
10000100
10000101
10000100
NAME truck7_p4f1

TIL vt54
10000101
10000100
10000100
10000101
10001100
10001100
10001101
10000100
>
00000100
00000101
00000100
00000100
00001101
00001100
00001100
00000101
NAME truck10_p4f1

TIL vt55
00000100
00001101
00001100
00001100
00000101
00000110
00000111
00000000
>
10000100
10001100
10001101
10001100
10000100
10000101
10000110
10000111
NAME truck13_p4f1

TIL uib01
00010101
01000000
00010101
10100000
00000000
10100000
00000000
10100000
>
00101010
00000000
10101010
00000000
10100000
00000000
10100000
00000000
NAME border_1

TIL uib02
01010101
00000000
01010101
00000000
00000000
00000000
00000000
00000000
>
10101010
00000000
10101010
00000000
00000000
00000000
00000000
00000000
NAME border_2

TIL uib03
01010100
00000000
01010101
00000000
00000101
00000000
00000101
00000000
>
10101000
00000010
10101000
00000101
00000000
00000101
00000000
00000101
NAME border_3

TIL uib04
00000000
10100000
00000000
10100000
00000000
10100000
00000000
10100000
>
10100000
00000000
10100000
00000000
10100000
00000000
10100000
00000000
NAME border_4

TIL uib05
00000101
00000000
00000101
00000000
00000101
00000000
00000101
00000000
>
00000000
00000101
00000000
00000101
00000000
00000101
00000000
00000101
NAME border_5

TIL uib06
00000000
10100000
00000000
10100000
00000000
10101010
00000000
00101010
>
10100000
00000000
10100000
00000000
10100000
00010101
01000000
00010101
NAME border_6

TIL uib07
00000000
00000000
00000000
00000000
00000000
10101010
00000000
10101010
>
00000000
00000000
00000000
00000000
00000000
01010101
00000000
01010101
NAME border_7

TIL uib08
00000101
00000000
00000101
00000000
00000101
10101000
00000010
10101000
>
00000000
00000101
00000000
00000101
00000000
01010101
00000000
01010100
NAME border_8");

        return true;
    }

    private bool ExportPlayerSprite(StringBuilder stringBuilder)
    {
        //We only have 1 sprite, and it's the player.
        //Player texture data (don't mess with the whitespace)
        stringBuilder.AppendLine(@"SPR A
00011000
00101100
01111100
00011100
00111110
01111111
00111110
00000100
>
00011000
00110100
00111110
00111000
11111111
01111110
00111100
00010000");

        //Player start position

        //I'm not a fan of GetComponent, but in a 1 off function like this it's fine. (Otherwise human error and misassignement will take over)

        Player player = GameObject.FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("GameExporter: No player found!");
            return false;
        }

        //Offset the positions so 0,0 is the top left corner (counting up to the bottom & right)
        int playerX = (int)player.transform.position.x;
        int playerY = (int)player.transform.position.y * -1;

        stringBuilder.AppendFormat("POS {0} {1},{2}", 0, playerX, playerY); //RoomID, x, y
        stringBuilder.Append(Environment.NewLine);
        return true;
    }

    private bool ExportSprites(StringBuilder stringBuilder)
    {
        //We don't actually read texture data or anything, just shove this datablob in there
        //NOTE: Don't tab! Otherwise it will add whitespace that we don't need
        stringBuilder.AppendLine(@"SPR uictrl01
00111111
01000000
10011111
10100000
10100000
10100000
10100000
10100000
NAME ui_ctrl_01
POS 0 6,6

SPR uictrl02
11111111
00000000
11111111
00000000
00000000
00000000
00000000
01110000
NAME ui_ctrl_02
POS 0 7,6

SPR uictrl03
11111111
00000000
11111111
00000000
00000000
00000000
00000000
00011100
>
11111111
00000000
11111111
00000000
00000000
00000000
00000000
00000000
NAME ui_ctrl_03
POS 0 8,6

SPR uictrl04
11111100
00000010
11111001
00000101
00000101
00000101
00000101
00000101
NAME ui_ctrl_04
POS 0 9,6

SPR uictrl05
10100000
10100001
10100010
10100010
10100010
10100011
10100011
10100001
NAME ui_ctrl_05
POS 0 6,7

SPR uictrl06
10001000
00000100
01110010
01100001
01010010
00000100
10001000
11010010
NAME ui_ctrl_06
POS 0 7,7

SPR uictrl07
00100010
01000001
10011100
00001100
10010100
01000001
00100011
10010111
>
00011100
00100010
01000001
10011100
10001100
01010100
00100001
10010011
NAME ui_ctrl_07
POS 0 8,7

SPR uictrl08
00000101
00000101
10000101
10000101
10000101
10000101
10000101
00000101
>
00000101
00000101
00000101
10000101
10000101
10000101
10000101
00000101
NAME ui_ctrl_08
POS 0 9,7

SPR uictrl09
10100000
10100000
10100000
10100000
10100000
10100000
10100000
10100000
NAME ui_ctrl_09
POS 0 6,8

SPR uictrl0a
11100011
01110011
00111000
00011100
00001110
00000111
00000011
00000001
NAME ui_ctrl_10
POS 0 7,8

SPR uictrl0b
00001110
10010100
00100010
01000001
10010100
00001100
10011100
11000001
NAME ui_ctrl_11
POS 0 8,8

SPR uictrl0c
00000101
00000101
00000101
00000101
10000101
10000101
10000101
10000101
NAME ui_ctrl_12
POS 0 9,8

SPR uictrl0d
10100000
10100000
10100000
10100000
10100000
10011111
01000000
00111111
NAME ui_ctrl_13
POS 0 6,9

SPR uictrl0e
00000000
00000000
00000000
00000000
00000000
11111111
00000000
11111111
NAME ui_ctrl_14
POS 0 7,9

SPR uictrl0f
11100011
01111111
00111110
00000000
00000000
11111111
00000000
11111111
NAME ui_ctrl_15
POS 0 8,9

SPR uictrl0g
10000101
00000101
00000101
00000101
00000101
11111001
00000010
11111100
NAME ui_ctrl_16
POS 0 9,9");

        return true;
    }

    private bool ExportItems(StringBuilder stringBuilder)
    {
        //This game doesn't use items (yet) but Bitsy requires us to have at least 1
        //So here's just the default tea sprite (don't mess with the whitespace!)
        stringBuilder.AppendLine(@"ITM 0
00000000
00000000
00000000
00111100
01100100
00100100
00011000
00000000
NAME tea
DLG ITM_0");

        return true;
    }

    private bool ExportDialogue(StringBuilder stringBuilder)
    {
        //This game doesn't use dialogue (yet)
        //So here's just the default tea item dialogue (don't mess with the whitespace!)
        stringBuilder.AppendLine(@"DLG ITM_0
You found a nice warm cup of tea");

        return true;
    }

    private bool ExportEndings(StringBuilder stringBuilder, List<Ending> endings)
    {
        for (int i = 0; i < endings.Count; ++i)
        {
            stringBuilder.AppendFormat("END {0}", i);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.AppendLine(endings[i].Reason);

            //Add whitespace
            if (i < endings.Count - 1)
                stringBuilder.AppendLine();
        }

        return true;
    }

    private bool ExportVariables(StringBuilder stringBuilder)
    {
        //This game doesn't use variables (yet)
        //So here's just the default variable (don't mess with the whitespace!)
        stringBuilder.AppendLine(@"VAR a
42");

        return true;
    }

    //Misc
    public void OpenExportedFile()
    {
        System.Diagnostics.Process.Start(@".\" + m_Filename + ".bitsy");
    }
}

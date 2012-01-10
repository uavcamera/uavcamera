/* Copyright 2011 Michael Hodgson, Piyabhum Sornpaisarn, Andrew Busse, John Charlesworth, Paramithi Svastisinha

    This file is part of uavcamera.

    uavcamera is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    uavcamera is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with uavcamera.  If not, see <http://www.gnu.org/licenses/>.

*/

﻿//#define SIMULATE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net.Configuration;
using System.IO;
using System.Threading;

namespace NCamGS
{
    class UAVConnector
    {
        Socket dataPort = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket consolePort = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Random random = new Random();

        int clearCount = 0;
        int clearNum = 20;

        public void Connect(byte dataStream)
        {
            try
            {
                dataPort.Connect("localhost", 8802);
                //MessageBox.Show("Connection to Port Name" + portName + "Port Number" + portNumber + "is complete!");
                Console.WriteLine("Data port connection to Port Name {0}  Port Number {1} is complete!", "localhost", 8802);

                // send selection byte to select datastream 0
                byte[] dataStreamSelection = { dataStream };
                dataPort.Send(dataStreamSelection);
                Console.WriteLine("Data stream selection completed.");

                consolePort.Connect("localhost", 8800);
                Console.WriteLine("Data port connection to Port Name {0} Port Number {1} is complete!", "localhost", 8800);
                //this.SendTextToUAV("da 40 payload[0].mem_bytes[0]");
            }
            catch
            {
                Console.WriteLine("Error code\n Unable to connect to UAV:\n Please check that:\n1.The UAV is connected \n2.The program gcs has opened \n3. The program has connect to the UAV\n4.The program has send stream dat\n5. The program as to run testbyte on da");
                //MessageBox.Show("Error code\n Unable to connect to UAV:\n Please check that:\n1.The UAV is connected \n2.The program gcs has opened \n3. The program has connect to the UAV\n4.The program has send stream dat\n5. The program as to run testbyte on da");
            }
        }

        public byte[] GetDataBytes()
        {
            //byte[] getByteSendBytes = new byte[numBytes]; // you need to send 0's to get bytes out for some reason (NOPE, it does not)
            //for(int i = 0; i < numBytes; i++) {
            //    getByteSendBytes[i] = 0;
            //}
            //dataPort.Send(getByteSendBytes);
            /*byte[] sizeByte = { 0 };
            try
            {
                dataPort.Receive(sizeByte, 1, SocketFlags.None);
            }
            catch
            {
                
            }
            int numBytes = sizeByte[0];
            byte[] recBytes = new byte[numBytes];
            try
            {
                dataPort.Receive(recBytes, numBytes, SocketFlags.None);
            }
            catch
            { 
                
            }
            return recBytes;*/

            clearCount++;
            if (clearCount >= clearNum)
            {
                this.SendTextToUAV("console clear"); // speeds everyting up by keeping the console nice and clear
                clearCount = 0;
            }
            for (int i = 0; i < 10; i++)
            {

                this.SendTextToUAV("payload[0].mem_bytes[0]");

                byte[] cByteIn = new byte[1];
                for (int pau = 0; pau < 50; pau++)
                {
                    while (consolePort.Available != 0) // length of PAYLOAD[0].SEND_BYTES\n
                    {
                        string lineOut = this.ReadConsoleLine();
                        //Console.Write(lineOut);
                        if (lineOut.IndexOf("PAYLOAD[0].MEM_BYTES[0]") != -1)
                        {
                            //Console.WriteLine("MEM_BYTES found.");
                            string[] parts = lineOut.Split(' ');
                            int pLen = parts.Length;
                            Console.WriteLine(lineOut);
                            byte[] bytesOut = new byte[parts.Length - 1];

                            for (int partIndex = 1; partIndex < parts.Length; partIndex++)
                            {
                                //Console.Write(parts[partIndex]);
                                string hex = parts[partIndex].Remove(0, 2);
                                if (hex == "")
                                {
                                    return bytesOut;
                                }
                                bytesOut[partIndex - 1] = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                                //Console.WriteLine((int)bytesOut[partIndex - 1]);   
                            }

                            return bytesOut;
                        }

                    }
                    //Console.WriteLine("Sleeping... {0}", pau);
                    Thread.Sleep(20);
                }

                Console.WriteLine("MEM_BYTES request failed, resending request.");
            }
            Console.WriteLine("MEM_BYTES request failed completley.");
            return null;
        }

        public void SendTextToUAV(string textToUAV)
        {
            char[] toUAVChar = new char[512];
            byte[] toUAVByte = new byte[512];
            byte[] fromUAVByte = new byte[1000];
            byte[] oneByteArray = new byte[1];
            textToUAV += "\n";
            toUAVChar = textToUAV.ToCharArray();
            toUAVByte = System.Text.Encoding.ASCII.GetBytes(toUAVChar);
            try
            {
                int sendByte = consolePort.Send(toUAVByte, toUAVChar.Length, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
        public void SendCommand(byte[] command, bool ack)
        {

            clearCount++;
            if (clearCount >= clearNum)
            {
                this.SendTextToUAV("console clear"); // speeds everyting up by keeping the console nice and clear
                clearCount = 0;
            }

            // use to send commmand to the uav
            string toUAV;
            //string fromUAV;
            char[] toUAVChar = new char[512];
            byte[] toUAVByte = new byte[512];
            byte[] fromUAVByte = new byte[1000];
            byte[] oneByteArray = new byte[1];

            toUAV = "payload[0].send_bytes";

            for (int commandByteCount = 0; commandByteCount < command.Length; commandByteCount++)
            {
                oneByteArray[0] = command[commandByteCount];
                toUAV += " 0x" + BitConverter.ToString(oneByteArray);
                //toUAV += " 0x" +System.Text.Encoding.ASCII.GetString(oneByteArray); 
            }

            toUAV += "\n";
            toUAVChar = toUAV.ToCharArray();
            toUAVByte = System.Text.Encoding.ASCII.GetBytes(toUAVChar);
            if (ack) // if the command is an ack it doesn't want acks
            {
                try
                {
                    int sendByte = consolePort.Send(toUAVByte, toUAVChar.Length, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
#if SIMULATE
                        if (random.Next(100) != 1)
                        {
#endif
                        //Thread.Sleep(20);
                        byte[] byIn = new byte[1];
                         
                        while (dataPort.Available != 0)
                        {
                            
                            dataPort.Receive(byIn, 1, SocketFlags.None);
                        }

                        while (consolePort.Available != 0)
                        {
                            consolePort.Receive(byIn, 1, SocketFlags.None);
                        }

                        int sendByte = consolePort.Send(toUAVByte, toUAVChar.Length, SocketFlags.None);
#if SIMULATE


                        }
                        else
                        {
                            Console.WriteLine("NOT SENDING!");
                        }
#endif
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("ERROR: " + ex.Message);
                    }
                    /*for (int ii = 0; ii < 50; ii++)
                    {
                       byte[] packet = this.GetDataBytes();
                        int packetSize = packet.Length;
                        if (packetSize > 0)
                        {
                            if (packet[0] == 8 && packet[1] == command[0])
                            {
                                Console.WriteLine("Command aknowledged sir!");
                                return;
                            }
                        } 
                    }*/
                    byte[] cByteIn = new byte[1];
                    for (int pau = 0; pau < 50; pau++)
                    {
                        while (consolePort.Available != 0) // length of PAYLOAD[0].SEND_BYTES\n
                        {
                            string lineOut = this.ReadConsoleLine();
                            //Console.Write(lineOut);
                            if (lineOut.IndexOf("PAYLOAD[0].SEND_BYTES") != -1)
                            {
                                Console.WriteLine("Command acknowledged!");
                                return;
                            }
                            
                        }
                        //Console.WriteLine("Sleeping... {0}", pau);
                        Thread.Sleep(20);
                    }
                    

                    Console.WriteLine("No acknowledgement, sadface " + i);   
                }
            }



            //Port.Receive(fromUAVByte);
            //fromUAV = System.Text.Encoding.ASCII.GetString(fromUAVByte);
        }


        public string ReadConsoleLine()
        {
            StringBuilder sBuilder = new StringBuilder();
            byte[] cByteIn = new byte[1];
            while ((char)cByteIn[0] != '\n')
            {
                consolePort.Receive(cByteIn, 1, SocketFlags.None);
                sBuilder.Append((char)cByteIn[0]);
            }
            string strOut = sBuilder.ToString();
            // remove trailing carrage returns etc
            char[] trimChars = {'\n', '\r'};
            strOut = strOut.TrimEnd(trimChars);
            return strOut;
        }

        public void Close()
        {
            dataPort.Close();
        }

        ~UAVConnector()
        {
            this.Close();
        }

    }
}

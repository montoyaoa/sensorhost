using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using System.IO.Ports;

namespace Get_CPU_Temp5
{
    class Program
    {
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        static void SetComponentNames()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.HDDEnabled = true;
            computer.RAMEnabled = true;
            computer.FanControllerEnabled = true;
            computer.MainboardEnabled = true;
            computer.Accept(updateVisitor);

            string CPUName = "";
            string GPUName = "";
            string RAMName = "";

            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                switch (computer.Hardware[i].HardwareType)
                {
                    case HardwareType.CPU:
                        CPUName = computer.Hardware[i].Name;
                        break;
                    case HardwareType.GpuNvidia:
                        GPUName = computer.Hardware[i].Name;
                        break;
                    case HardwareType.RAM:
                        RAMName = computer.Hardware[i].Name;
                        break;
                    default:
                        break;
                }
            }

            SerialPort arduino = new SerialPort("COM4", 9600);
            arduino.Open();
            arduino.Write(CPUName + "," + GPUName + "," + RAMName + "," );
            arduino.Close();
        }

        static void SendSensorData()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.HDDEnabled = true;
            computer.RAMEnabled = true;
            computer.FanControllerEnabled = true;
            computer.MainboardEnabled = true;
            computer.Accept(updateVisitor);

            string CPUTotalLoad = "";

            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                switch (computer.Hardware[i].HardwareType)
                {
                    case HardwareType.CPU:
                        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "CPU Total" && computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                                CPUTotalLoad = computer.Hardware[i].Sensors[j].Value.ToString();
                        }
                            break;
                    //case HardwareType.GpuNvidia:
                        //GPUName = computer.Hardware[i].Name;
                        //break;
                    //case HardwareType.RAM:
                        //RAMName = computer.Hardware[i].Name;
                        //break;
                    default:
                        break;
                }
            }

            SerialPort arduino = new SerialPort("COM4", 9600);
            arduino.Open();
            arduino.Write(CPUTotalLoad + ",");
            arduino.Close();

        }

        static void GetSystemInfo()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.HDDEnabled = true;
            computer.RAMEnabled = true;
            computer.FanControllerEnabled = true;
            computer.MainboardEnabled = true;
            computer.Accept(updateVisitor);




            //for (int i = 0; i < computer.Hardware.Length; i++)
            //{
            //    if (computer.Hardware[i].HardwareType == HardwareType.CPU)
            //    {
            //        for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
            //        {
            //            if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
            //                Console.WriteLine(computer.Hardware[i].Sensors[j].Name + ":" + computer.Hardware[i].Sensors[j].Value.ToString() + "\r");
            //        }
            //    }
            //}

            //iterate through every component in the system
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                //prefix the component with its type
                switch (computer.Hardware[i].HardwareType)
                {
                    case HardwareType.CPU:
                        Console.Write("CPU: ");
                        break;
                    case HardwareType.GpuNvidia:
                        Console.Write("GPU: ");
                        break;
                    case HardwareType.HDD:
                        Console.Write("HDD: ");
                        break;
                    case HardwareType.RAM:
                        Console.Write("RAM: ");
                        break;
                    case HardwareType.Mainboard:
                        Console.Write("Mainboard: ");
                        break;
                    default:
                        Console.Write("???: ");
                        break;
                }


                //spit out the name of the CPU to the arduino through the COM port
                if(computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    SerialPort arduino = new SerialPort("COM4", 9600);
                    arduino.Open();
                    arduino.Write(computer.Hardware[i].Name);
                    arduino.Write(" ,");
                    arduino.Close();
                }
                else if(computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    SerialPort arduino = new SerialPort("COM4", 9600);
                    arduino.Open();
                    arduino.Write(computer.Hardware[i].Name);
                    arduino.Close();
                }

                //return the name of each part of the system
                Console.WriteLine(computer.Hardware[i].Name);


                //prefix each reading with what type of data it is
                for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                {
                    switch (computer.Hardware[i].Sensors[j].SensorType)
                    {
                        case SensorType.Clock:
                            Console.Write("(clock)");
                            break;
                        case SensorType.Control:
                            Console.Write("(control)");
                            break;
                        case SensorType.Data:
                            Console.Write("(data)");
                            break;
                        case SensorType.Factor:
                            Console.Write("(factor)");
                            break;
                        case SensorType.Fan:
                            Console.Write("(fan)");
                            break;
                        case SensorType.Flow:
                            Console.Write("(flow)");
                            break;
                        case SensorType.Level:
                            Console.Write("(level)");
                            break;
                        case SensorType.Load:
                            Console.Write("(load)");
                            break;
                        case SensorType.Power:
                            Console.Write("(power)");
                            break;
                        case SensorType.SmallData:
                            Console.Write("(smalldata)");
                            break;
                        case SensorType.Temperature:
                            Console.Write("(temp)");
                            break;
                        case SensorType.Throughput:
                            Console.Write("(throughput)");
                            break;
                        case SensorType.Voltage:
                            Console.Write("(voltage)");
                            break;
                        default:
                            break;
                    }    
                    Console.WriteLine(computer.Hardware[i].Sensors[j].Name + ": " + computer.Hardware[i].Sensors[j].Value.ToString() + "\t");
                }
                Console.WriteLine(" ");
            }
            computer.Close();
        }


        static void Main(string[] args)
        {
            //SerialPort arduino = new SerialPort("COM4", 9600);
            //arduino.Open();

            //while (true)
            //{
            //    Thread.Sleep(500);
            //    Console.Clear();
            //    GetSystemInfo();

            //}
            SetComponentNames();
            while(true)
            {
                SendSensorData();
            }
            //GetSystemInfo();
            
            //arduino.Write("test test test");
            //arduino.Close();




        }
    }
}
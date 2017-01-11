using System;
using System.Collections.Generic;
using System.Text;
using MPI;

namespace RVIP_44_MPI
{
    class Program
    {
        
        static void Main(string[] args)
        {

            using (var mppi = new MPI.Environment(ref args))
            {

                if (Communicator.world.Rank == 0)
                {
                    double[,] mas = new double[3, 3];
                    mas[0, 0] = 4;
                    mas[0, 1] = 0;
                    mas[0, 2] = 1;
                    mas[1, 0] = 2;
                    mas[1, 1] = 4;
                    mas[1, 2] = 3;
                    mas[2, 0] = 9;
                    mas[2, 1] = 8;
                    mas[2, 2] = 7;
                    Console.WriteLine("Массив:");
                    for (int i = 0; i < mas.GetLength(0); i++)
                    {
                        for (int j = 0; j < mas.GetLength(1); j++)
                        {
                            Console.Write(mas[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    double b = 0;
                    for (int i = 0; i < mas.GetLength(0) - 1; i++)
                    {
                        for (int k = 0; k < mas.GetLength(0) - 1; k++)
                        {
                            if (mas[k, 0] > mas[k + 1, 0])
                            {
                                for (int j = 0; j < mas.GetLength(1); j++)
                                {
                                    b = mas[k, j];
                                    mas[k, j] = mas[k + 1, j];
                                    mas[k + 1, j] = b;
                                }
                            }
                        }
                    }

                    Communicator.world.Send(mas, 1, 0);
                }


                if (Communicator.world.Rank == 1)
                {
                    double[,] msg = Communicator.world.Receive<double[,]>(Communicator.world.Rank - 1, 0);
                    Console.WriteLine("Массив сортированный:");
                    for (int i = 0; i < msg.GetLength(0); i++)
                    {
                        for (int j = 0; j < msg.GetLength(1); j++)
                        {
                            Console.Write(msg[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                }

            }

            

        }
    }
}

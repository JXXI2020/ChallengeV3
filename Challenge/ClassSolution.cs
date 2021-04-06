using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Challenge
{
    public class ClassSolution
    {
        public void Main(String[] args)
        {
            int[,] matriz = readFile();
            Print2DArray(matriz);

            var anInstanceofMyClass = new Maze();

            // configurar tamaño de matriz
            int dimension = 4;

            for (int filaAnalizada = 0; filaAnalizada < dimension; filaAnalizada++)
            {
                for (int columnaAnalizada = 0; columnaAnalizada < dimension; columnaAnalizada++)
                {
                    Console.WriteLine(anInstanceofMyClass.minDistance(matriz, filaAnalizada, columnaAnalizada));
                }
            }
        

   


        }

        //  checking where it's valid or not
        private static bool isValid(int x, int y, int[,] grid, bool[,] visited, int ax, int ay)
        {
            if (((x >= 0)
            && ((y >= 0)
            && ((x < grid.GetLength(0))
            && ((y < grid.GetLength(1))
            && ((grid[x, y] < grid[ax, ay])
            && (visited[x, y] == false)))))))
            {
                return true;
            }

            return false;
        }

        //  QItem for current location and distance
        //  from source location
        public class QItem
        {

            public int row;

            public int col;

            public int dist;

            public QItem(int row, int col, int dist)
            {
                this.row = this.row;
                this.col = this.col;
                this.dist = this.dist;
            }
        }

        public class Maze
        {
            public int minDistance(int[,] grid, int ff, int cc)
            {
                QItem source = new QItem(0, 0, 0);
                //  To keep track of visited QItems. Marking
                //  blocked cells as visited.

                //for (int i = 0; (i < grid.GetLength(0)); i++)
                //{
                //    for (int j = 0; (j < grid.GetLength(1)); j++)
                //    {
                //        //  Finding source

                //        if (grid[i, j] == 115)
                //        {
                //            source.row = i;
                //            source.col = j;
                source.row = ff;
                source.col = cc;
                //            break;

                //        }

                //    }

                //}

                //  applying BFS on matrix cells starting from source
                Queue<QItem> queue = new Queue<QItem>();

                queue.Enqueue(new QItem(source.row, source.col, 0));
                bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];

                visited[source.row, source.col] = true;
                while ((queue.Count > 0))
                {
                    QItem p = queue.Dequeue();
                    //  Destination found;
                    if ((grid[p.row, p.col] == 100))
                    {
                        return p.dist;
                    }

                    //  moving up
                    if (isValid((p.row - 1), p.col, grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem((p.row - 1), p.col, (p.dist + 1)));
                        visited[(p.row - 1), p.col] = true;
                    }

                    //  moving down
                    if (isValid((p.row + 1), p.col, grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem((p.row + 1), p.col, (p.dist + 1)));
                        visited[(p.row + 1), p.col] = true;
                    }

                    //  moving left
                    if (isValid(p.row, (p.col - 1), grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem(p.row, (p.col - 1), (p.dist + 1)));
                        visited[p.row, (p.col - 1)] = true;
                    }

                    //  moving right
                    if (isValid((p.row - 1), (p.col + 1), grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem(p.row, (p.col + 1), (p.dist + 1)));
                        visited[p.row, (p.col + 1)] = true;
                    }

                }

                return -1;
            }
        }

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }       

        public int[,] readFile()
        {
            String input = File.ReadAllText(@"C:\Users\vaio\source\repos\Challenge\Challenge\4x4.txt");

            int i = 0, j = 0;
            // configurar tamaño de matriz
            int dimension = 4;
            //dimension = dimension - 1;

            int dimensionFila = dimension;
            int dimensionColumna = dimension;

            int[,] result = new int[dimensionFila, dimensionColumna];
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            return result;


        }
    }

}

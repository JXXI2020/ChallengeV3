using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using System.Linq;



namespace Challenge
{
    

    public class Example 
    {
        static void Main(String[] args)
        {
            // parametros de configuracion
            // configurar tamaño de matriz
            int dimension = 1000;
            
            
            
            int[,] matriz = readFile();
            Console.WriteLine("Matriz cargada. ");
            //Print2DArray(matriz);
            Console.WriteLine("Todos los posible caminos encontrados mas grandes por cada nodo: ");
            var anInstanceofMaze = new Maze();
                        
            int[,] resultados = new int[10000000, 3]; // 3 así 0: celda en que inicia, 1 # de pasos, 2 id 
            string[] resultadosPath = new string[10000000];
            
            int contadorParaResultados = 1;

            for (int filaAnalizada = 0; filaAnalizada < dimension; filaAnalizada++)
            {
                for (int columnaAnalizada = 0; columnaAnalizada < dimension; columnaAnalizada++)
                {
                    string  numeroDePasos = anInstanceofMaze.Paso(matriz, filaAnalizada, columnaAnalizada);
                    //Console.WriteLine(numeroDePasos);
                    Console.WriteLine(filaAnalizada.ToString() + " / " +  columnaAnalizada.ToString());
                    resultados[contadorParaResultados, 2] = contadorParaResultados;

                    resultados[contadorParaResultados, 0] = matriz[filaAnalizada, columnaAnalizada];

                    string str = numeroDePasos;
                    int index = str.IndexOf(' ');
                    index = str.IndexOf(' ', index);
                    string result = str.Substring(0, index);

                    resultados[contadorParaResultados, 1] = Int32.Parse(result);

                    int indexOfPath = str.IndexOf(':');                    
                    indexOfPath = str.IndexOf(':', indexOfPath);
                    int length = str.Length;
                    string result2 = str.Substring(indexOfPath, length - indexOfPath);

                    


                    resultadosPath[contadorParaResultados] = (result2);
                    contadorParaResultados++;
                }
            }

            Console.WriteLine("Resultado: ");
            var sortedByBigPathVal = resultados.OrderByDescending(y => y[1]);
            var sortedByBigHeightVal = sortedByBigPathVal.OrderByDescending(y => y[0]);
            Console.WriteLine("Longitud Mayor de pasos / Length of calculated path :" + sortedByBigHeightVal[0, 1]);

            string strLN = resultadosPath[sortedByBigHeightVal[0, 2]];
            int indexOfLastNode = strLN.LastIndexOf('>');
            indexOfLastNode = strLN.LastIndexOf('>', indexOfLastNode) +2;
            int lengthLN = strLN.Length;
            string resulLN = strLN.Substring(indexOfLastNode, lengthLN - indexOfLastNode);

            Console.WriteLine("Nodo de inicio: " + sortedByBigHeightVal[0,0] + " - Ultimo Nodo: " + resulLN + " >> Caida / Drop of calculated path: " + ((Int32.Parse(sortedByBigHeightVal[0, 0].ToString()) - Int32.Parse(resulLN))));
            Console.WriteLine("Camino Mayor Recorrido / Calculated path: " + resultadosPath[sortedByBigHeightVal[0, 2]]);


        }

        //  checking where it's valid or not
        private static bool isValid(int x, int y, int[,] grid, bool[,] visited, int ax, int ay)
        {
            if (((x >= 0)
            && ((y >= 0)
            && ((x < grid.GetLength(0))
            && ((y < grid.GetLength(1))
            && ((grid[x, y] < grid[ax, ay])
            //&& (visited[x, y] == false)
            ))))))
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
            public int value;
            public string pasoAnterior;
            public QItem(int row, int col, int dist, int value, string pasoAnterior)
            {
                this.row = row;
                this.col = col;
                this.dist = dist;
                this.value = value;
                this.pasoAnterior = pasoAnterior;
            }
        }

        public class Maze
        {
            
            public string Paso(int[,] grid, int ff, int cc)
            {
                bool visualizarAnalisis = false;

                QItem source = new QItem(0, 0, 0, 0, "");
                
                source.row = ff;
                source.col = cc;
                
                //  applying BFS on matrix cells starting from source
                Queue<QItem> queue = new Queue<QItem>();
                
                queue.Enqueue(new QItem(source.row, source.col, 0, grid[source.row, source.col], "Ini"));
                if (visualizarAnalisis)
                    Console.WriteLine("Agrega a la cola: " + grid[source.row, source.col]);
                
                bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
                visited[source.row, source.col] = true;

                int pasoAnterior = grid[source.row, source.col];

                while  (queue.Count > 0) 
                {
                    QItem p = queue.Dequeue();
                    if (visualizarAnalisis)
                        Console.WriteLine("Quita de la cola: " + grid[p.row , p.col] + "(" + p.row + ","  + p.col + ") > pasoAnterior: " + p.pasoAnterior + " > " + p.value.ToString());
                    

                    //  moving up
                    if (isValid((p.row - 1), p.col, grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem((p.row - 1), p.col, (p.dist + 1), grid[(p.row - 1), p.col], p.pasoAnterior + " > " + p.value.ToString()));
                        visited[(p.row - 1), p.col] = true;
                        if (visualizarAnalisis)
                            Console.WriteLine("Agrega a la cola: " + grid[(p.row - 1), p.col]);
                    }

                    //  moving down
                    if (isValid((p.row + 1), p.col, grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem((p.row + 1), p.col, (p.dist + 1), grid[(p.row + 1), p.col], p.pasoAnterior + " > " + p.value.ToString()));
                        visited[(p.row + 1), p.col] = true;
                        if (visualizarAnalisis)
                            Console.WriteLine("Agrega a la cola: " + grid[(p.row + 1), p.col]);
                    }

                    //  moving left
                    if (isValid(p.row, (p.col - 1), grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem(p.row, (p.col - 1), (p.dist + 1), grid[p.row, (p.col - 1)], p.pasoAnterior + " > " + p.value.ToString()));
                        visited[p.row, (p.col - 1)] = true;
                        if (visualizarAnalisis)
                            Console.WriteLine("Agrega a la cola: " + grid[p.row, (p.col - 1)]);
                    }

                    //  moving right
                    if (isValid((p.row), (p.col + 1), grid, visited, p.row, p.col))
                    {
                        queue.Enqueue(new QItem(p.row, (p.col + 1), (p.dist + 1), grid[p.row, (p.col + 1)], p.pasoAnterior + " > " + p.value.ToString()));
                        visited[p.row, (p.col + 1)] = true;
                        if (visualizarAnalisis)
                            Console.WriteLine("Agrega a la cola: " + grid[p.row, (p.col + 1)]);
                    }

                    if (queue.Count < 1)
                    {
                        string p1 = (p.dist.ToString() + " pasos para este camino: " + p.pasoAnterior + " > " + p.value.ToString());
                        return p1;
                    }

                }

                return "-1";
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

        public static int[,] readFile()
        {
            //String input = File.ReadAllText(@"C:\Users\vaio\source\repos\Challenge\Challenge\4x4.txt");
            String input = File.ReadAllText(@"C:\Users\vaio\source\repos\Challenge\Challenge\map.txt");

            int i = 0, j = 0;
            // configurar tamaño de matriz
            int dimension = 1000;
            

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


    public static class MultiDimensionalArrayExtensions
    {
        /// <summary>
        ///   Orders the two dimensional array by the provided key in the key selector.
        /// </summary>
        /// <typeparam name="T">The type of the source two-dimensional array.</typeparam>
        /// <param name="source">The source two-dimensional array.</param>
        /// <param name="keySelector">The selector to retrieve the column to sort on.</param>
        /// <returns>A new two dimensional array sorted on the key.</returns>
        public static T[,] OrderBy<T>(this T[,] source, Func<T[], T> keySelector)
        {
            return source.ConvertToSingleDimension().OrderBy(keySelector).ConvertToMultiDimensional();
        }
        /// <summary>
        ///   Orders the two dimensional array by the provided key in the key selector in descending order.
        /// </summary>
        /// <typeparam name="T">The type of the source two-dimensional array.</typeparam>
        /// <param name="source">The source two-dimensional array.</param>
        /// <param name="keySelector">The selector to retrieve the column to sort on.</param>
        /// <returns>A new two dimensional array sorted on the key.</returns>
        public static T[,] OrderByDescending<T>(this T[,] source, Func<T[], T> keySelector)
        {
            return source.ConvertToSingleDimension().
                OrderByDescending(keySelector).ConvertToMultiDimensional();
        }
        /// <summary>
        ///   Converts a two dimensional array to single dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of the two dimensional array.</typeparam>
        /// <param name="source">The source two dimensional array.</param>
        /// <returns>The repackaged two dimensional array as a single dimension based on rows.</returns>
        private static IEnumerable<T[]> ConvertToSingleDimension<T>(this T[,] source)
        {
            T[] arRow;

            for (int row = 0; row < source.GetLength(0); ++row)
            {
                arRow = new T[source.GetLength(1)];

                for (int col = 0; col < source.GetLength(1); ++col)
                    arRow[col] = source[row, col];

                yield return arRow;
            }
        }
        /// <summary>
        ///   Converts a collection of rows from a two dimensional array back into a two dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of the two dimensional array.</typeparam>
        /// <param name="source">The source collection of rows to convert.</param>
        /// <returns>The two dimensional array.</returns>
        private static T[,] ConvertToMultiDimensional<T>(this IEnumerable<T[]> source)
        {
            T[,] twoDimensional;
            T[][] arrayOfArray;
            int numberofColumns;

            arrayOfArray = source.ToArray();
            numberofColumns = (arrayOfArray.Length > 0) ? arrayOfArray[0].Length : 0;
            twoDimensional = new T[arrayOfArray.Length, numberofColumns];

            for (int row = 0; row < arrayOfArray.GetLength(0); ++row)
                for (int col = 0; col < numberofColumns; ++col)
                    twoDimensional[row, col] = arrayOfArray[row][col];

            return twoDimensional;
        }
    }

}

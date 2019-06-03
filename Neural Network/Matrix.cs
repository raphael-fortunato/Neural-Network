using System;



namespace Neural_Network
{
   public class Matrix 
   {
        public int row, col;
        public double[,] value;
        static Random rnd = new Random();
        public Matrix( int rows, int cols)
        {
            this.row = rows;
            this.col = cols;
            value = new double[row, col];
            
        }
        public void Add(Matrix n)
        {
            
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.col; j++)
                {
                    this.value[i, j] += n.value[i,j];
                }
            }
        }
        public static Matrix Subtract(Matrix a, Matrix b)
        {
            Matrix result = new Matrix(a.row, a.col);
            for (int i = 0; i < result.row; i++)
            {
                for (int j = 0; j < result.col; j++)
                {
                   result.value[i,j] =  a.value[i, j] - b.value[i, j];
                }
            }
            return result;
        }
        public void Randomize()
        {
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    value[i, j] += rnd.NextDouble() * 2 -1;
                }
            }
        }
        public void Multiply(double n)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    value[i, j] *= n;
                }
            }
        }
        public void Print()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Console.Write(value[i, j] + "   ");
                    if(col - j == 1)
                        Console.Write("\n");
                    
                }
            }
            Console.WriteLine("\n------------------------");
            Console.ReadKey();
        }
        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (CompareLength(a,b))
            {
                Matrix result = new Matrix(a.row, b.col);
                for (int i = 0; i < result.row; i++)
                {
                    for (int j = 0; j < result.col; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < a.col; k++)
                        {
                            sum += a.value[i, k] * b.value[k, j];

                        }
                        result.value[i, j] = sum;


                    }

                }
                
                return result;
            }
            return null; 
        }
        public void Multiply(Matrix a)
        {
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.col; j++)
                {
                    this.value[i, j] *= a.value[i, j];
                }
            }
        }
        static bool CompareLength(Matrix a, Matrix b)
        {
            if (a.col == b.row)
                return true;
            return false;
        }
        public static Matrix Transpose(Matrix a)
        {
            Matrix result = new Matrix(a.col, a.row);
            for (int i = 0; i < result.row; i++)
            {
                for (int j = 0; j < result.col; j++)
                {
                    result.value[i, j] = a.value[j, i];
                }
            }
            return result;
        }
        public void Map(Func<double, double> method)
        {
            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.col; j++)
                {
                    double val = this.value[i, j];
                    this.value[i, j] = method(val);
                }
            }
        }
        public static Matrix Map(Matrix a ,Func<double, double> method)
        {
            Matrix result = new Matrix(a.row, a.col);
            for (int i = 0; i < result.row; i++)
            {
                for (int j = 0; j < result.col; j++)
                {
                    double val = a.value[i, j];
                    result.value[i, j] = method(val);
                }
            }
            return result;
        }
        public static Matrix FromArray(double[] array)
        {
            Matrix result = new Matrix(array.Length, 1);
            for (int i = 0; i < array.Length; i++)
            {
                result.value[i, 0] = array[i];
            }
            return result;
        }
        public static double[] ToArray(Matrix a)
        {
            double[] result = new double[a.row * a.col];
            for (int j = 0; j < a.row; j++)
            {
                for (int i = 0; i < a.col; i++)
                {
                    result[a.col * i + j] = a.value[j , i];
                }
            }
            return result;
        }

    }
}

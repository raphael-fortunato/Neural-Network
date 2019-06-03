using System;



namespace Neural_Network
{
    public partial class NeuralNetwork
    {
        public Matrix Getweights_ih{ get { return Weights_ih; } } 
        public Matrix Getweights_ho { get { return Weights_ho; } }
        public Matrix Getbias_h { get { return Bias_h; } }
        public Matrix Getbias_o { get { return Bias_o; } }
        protected double learning_rate = 0.1; 
        int input_nodes, hidden_nodes, output_nodes;
        Matrix Weights_ih, Weights_ho, Bias_h, Bias_o;
        static Random r = new Random();
        public NeuralNetwork(int input_nodes, int hidden_nodes, int output_nodes)
        {
            this.input_nodes = input_nodes;
            this.hidden_nodes = hidden_nodes;
            this.output_nodes = output_nodes;
            //initializing the weights
            this.Weights_ih = new Matrix(this.hidden_nodes, this.input_nodes);
            this.Weights_ho = new Matrix(this.output_nodes, this.hidden_nodes);
            this.Weights_ih.Randomize();
            this.Weights_ho.Randomize();
            //initializing the the biases
            this.Bias_h = new Matrix(this.hidden_nodes, 1);
            this.Bias_o = new Matrix(this.output_nodes, 1);
            this.Bias_h.Randomize();
            this.Bias_o.Randomize();



        }
        public double[] FeedForward(double[] input)
        {
            Matrix inputs = Matrix.FromArray(input);
            Matrix hidden = Matrix.Multiply(Weights_ih, inputs);
            hidden.Add(Bias_h);
            hidden.Map(x => Sigmoid(x));
            Matrix output = Matrix.Multiply(Weights_ho, hidden);
            output.Add(Bias_o);
            output.Map(x => Sigmoid(x));
            return Matrix.ToArray(output);
  
        }
        public void Backpropagantion(double[] Inputs, double[] answer)
        {
            //FeedFoward values
            Matrix inputs = Matrix.FromArray(Inputs);
            Matrix hidden = Matrix.Multiply(Weights_ih, inputs);
            hidden.Add(this.Bias_h);
            hidden.Map(x => Sigmoid(x));
            Matrix Outputs = Matrix.Multiply(Weights_ho, hidden);
            Outputs.Add(this.Bias_o);
            Outputs.Map(x => Sigmoid(x));
            
            //Output Error == Target - Output
            Matrix Target = Matrix.FromArray(answer);
            Matrix output_error = Matrix.Subtract(Target, Outputs);

            //calcuate gradient Outputs
            Matrix Gradient = Matrix.Map(Outputs, x => dSigmoid(x));
            //Matrix gradient_output_error = Matrix.Multiply(Gradient, output_error);
            Gradient.Multiply(output_error);
            Gradient.Multiply(learning_rate);

            //calculate deltas
            Matrix hidden_t = Matrix.Transpose(hidden);
            Matrix weights_ho_delta = Matrix.Multiply(Gradient, hidden_t);
            //add gradient to bias
            Bias_o.Add(Gradient);
            //add delta to weights
            this.Weights_ho.Add(weights_ho_delta);

            //Calculate hidden_errors 
            Matrix Weights_ho_t = Matrix.Transpose(Weights_ho);
            Matrix hidden_error = Matrix.Multiply(Weights_ho_t, output_error);

            //calculate gradient hidden
            Matrix Gradient_hidden = Matrix.Map(hidden, x => dSigmoid(x));
            //Matrix gradient_hidden_hidden = Matrix.Multiply(Gradient_hidden, hidden_error);
            Gradient_hidden.Multiply(hidden_error);
            Gradient_hidden.Multiply(learning_rate);

            //calculate hidden deltas
            Matrix inputs_t =  Matrix.Transpose(inputs);
            Matrix weights_ih_delta = Matrix.Multiply(Gradient_hidden, inputs_t);
            //add gradient to bias
            Bias_h.Add(Gradient_hidden);
            //add delta to weights
            this.Weights_ih.Add(weights_ih_delta);


        }
        double dSigmoid(double y)
        {
            return y * (1 - y);
        }

        double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
        double relu(double x)
        {
            return Math.Max(0, x);
        }
        public double Learning_rate
        {
            get{
                return learning_rate;
            }
            set
            {
                if (value > 0)
                    learning_rate = value;
            }
        }
        public NeuralNetwork Copy()
        {
            NeuralNetwork NN = this;
            return NN;
        }
        public void Mutate(Func<double, double> method)
        {
            this.Weights_ih.Map(x => method(x));
            this.Weights_ho.Map(x => method(x));
            this.Bias_h.Map(x => method(x));
            this.Bias_o.Map(x => method(x));

        }
        public void CrossOver(NeuralNetwork Partner)
        {
            int rw = r.Next(this.Weights_ih.row);
            int cl = r.Next(this.Weights_ih.col);
            
            Matrix change = new Matrix(this.Weights_ih.row, this.Weights_ih.col);
            for (int i = rw; i < this.Weights_ih.row; i++)
            {
                for (int j = cl; j < this.Weights_ih.col; j++)
                {
                    change.value[i,j] = this.Weights_ih.value[i, j];
                    this.Weights_ih.value[i, j] = Partner.Weights_ih.value[i, j];
                    Partner.Weights_ih.value[i, j] = change.value[i, j];
                }

            }
            rw = r.Next(this.Weights_ho.row);
            cl = r.Next(this.Weights_ho.col);
            change = new Matrix(this.Weights_ho.row, this.Weights_ho.col);
            for (int i = rw; i < this.Weights_ho.row; i++)
            {
                for (int j = cl; j < this.Weights_ho.col; j++)
                {
                    change.value[i, j] = this.Weights_ho.value[i, j];
                    this.Weights_ho.value[i, j] = Partner.Weights_ho.value[i, j];
                    Partner.Weights_ho.value[i, j] = change.value[i, j];

                }

            }
            rw = r.Next(this.Bias_h.row);
            cl = r.Next(this.Bias_h.col);
            change = new Matrix(this.Bias_h.row, this.Bias_h.col);
            for (int i = rw; i < this.Bias_h.row; i++)
            {
                for (int j = cl; j < this.Bias_h.col; j++)
                {
                    change.value[i, j] = this.Bias_h.value[i, j];
                    this.Bias_h.value[i, j] = Partner.Bias_h.value[i, j];
                    Partner.Bias_h.value[i, j] = change.value[i, j];

                }

            }
            rw = r.Next(this.Bias_o.row);
            cl = r.Next(this.Bias_o.col);
            change = new Matrix(this.Bias_o.row, this.Bias_o.col);
            for (int i = rw; i < this.Bias_o.row; i++)
            {
                for (int j = cl; j < this.Bias_o.col; j++)
                {
                    change.value[i, j] = this.Bias_o.value[i, j];
                    this.Bias_o.value[i, j] = Partner.Bias_o.value[i, j];
                    Partner.Bias_o.value[i, j] = change.value[i, j];

                }

            }

        }






    }
}

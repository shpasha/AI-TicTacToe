using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrestikiNoliki
{
    class Neuron
    {
        private double[] weights;
        private int numWeights;
        private double y_in, y_out;
        public double delta;

        public double getOutput()
        {
            return y_out;
        }

        public int getWeightsCount()
        {
            return numWeights;
        }

        public double getWeightsI(int i)
        {
            return weights[i];
        }


        public void updateWeights(double trueRes, double speed, double[] next, bool isFirst)
        {
            if (isFirst)
            {
                for (int i = 0; i < numWeights; i++)
                {
                    weights[i] += speed * (trueRes - y_out) * activationFunc1(y_in) * next[i];
                }
                delta = (trueRes - y_out);
            } else
            {
                for (int i = 0; i < numWeights; i++)
                {
                    weights[i] += speed * (trueRes) * activationFunc1(y_in) * next[i];
                }
                delta = trueRes;
            }
        }

        public Neuron(int numWeights)
        {
            this.numWeights = numWeights;
            weights = new double[numWeights];
            for (int i = 0; i < numWeights; i++)
            {
                weights[i] = 0;
            }
        }

        public Neuron(int numWeights, double[] weights)
        {
            this.numWeights = numWeights;
            this.weights = weights;
        }

        public double calcOutput(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < numWeights; i++)
            {
                sum += input[i] * weights[i];
            }
            y_in = sum;
            y_out = activationFunc(y_in);
            return y_out;
        }

        private double activationFunc1(double x)
        {
            return activationFunc(x) * (1 - activationFunc(x));
        }

        private double activationFunc(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrestikiNoliki
{
    class Net
    {
        private int numLayers;
        public List<Neuron>[] layers;
        private double[] input;

        public Net(int numLayers)
        {
            this.numLayers = numLayers;
            layers = new List<Neuron>[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                layers[i] = new List<Neuron>();
            }
        }

        public void pushNeuron(int layerIndex, int numWeights)
        {
           layers[layerIndex].Add(new Neuron(numWeights));
        }

        public void pushNeuron(int layerIndex, int numWeights, double[] weights)
        {
            layers[layerIndex].Add(new Neuron(numWeights, weights));
        }

        public double[] getOutput(double[] input) {
            this.input = input;


            int n = layers[layers.Length - 2].Count;

            double[] output = new double[9];

            double[] nextInput = new double[n];

            for (int i = 0; i < n; i++)
            {
                nextInput[i] = layers[0][i].calcOutput(input);
            }

            for (int i = 0; i < 9; i++)
            {
                output[i] = layers[1][i].calcOutput(nextInput);
            }


            return output;
        } 

        public void train(double[] trueRes, double speed)
        {
       //     System.Diagnostics.Debugger.Break();
            int k = 0;
            double[] next;
            double[] res;


            bool isFirst = true;
            for (int i = numLayers-1; i >= 0; i--)
            {
                if (i == numLayers - 1)
                {
                    res = trueRes;
                } else
                {
                    k = 0;
                    res = new double[layers[i+1][0].getWeightsCount()];


                    for (int q = 0; q < res.Length; q++)
                    {
                        res[q] = 0;
                        foreach (Neuron neuron in layers[i + 1])
                        {
                            res[q] += neuron.getWeightsI(q) * neuron.delta;
                        }
                    }
                } 

                if ( i == 0)
                {
                    next = input;
                } else
                {
                    k = 0;
                    next = new double[layers[i - 1].Count];
                    foreach (Neuron neuron in layers[i - 1])
                    {
                        next[k] = neuron.getOutput();
                        k++;
                    }
                }
                k = 0;
                
                
                foreach (Neuron neuron in layers[i])
                {
                    neuron.updateWeights(res[k], speed, next, isFirst);
                    k++;
                }
                isFirst = false;
            }
            
        }

    }
}

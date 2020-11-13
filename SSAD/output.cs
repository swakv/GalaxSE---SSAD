using UnitySVM;
using System;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OrbitGameBuild
{
	public abstract class OrbitNetworkRT
	{
		
		public string Name = string.Empty;
		
		public int modelNo;

		public double[] _stren;
		 
		public double[] _l;
		public double[] _exp;
	}


	public sealed class ScoreNetwork : OrbitNetworkRT 
	{
		public enum Skill 
		{ 
			ScoreDB=0,
			ScoreNow=1,
		}

		private double C = 0;
		private double Gamma = 0.0;
		private Node[][] vectors = new Node[1][];
		private double[] labels = new double[1]{
			-1};

		private double[] k = new double[2]
			{Infinity, Infinity};

		private double[] mins = new double[ 2]
			{35,31};

		private double[] maxes = new double[ 2]
			{35,31};

		private Model _model;
		public Model model
		{
			get
			{
				if(_model.numberOfClasses == 0)
				{
					vectors[0] = new Node[2] 
						{new Node(1, 35), new Node(2, 31)};

					Problem problem = new Problem(vectors, labels, 2);
					Parameter parameters = new Parameter(0);

					if(C != 0)
					{
						parameters.C = C;
						parameters.gamma = Gamma;
					}

						Problem sProb = SVM.ScaleProblem(problem, mins,maxes,0,1);
					

					_model = SVM.Train(sProb,parameters);
				}

				return _model;
			}
		}
		public int Classify(double[] levels)
		{
			if(levels.Length != 2)
				return -1;

			Node[] currentExample = ConvertToNodeArray(levels);

			for(int i=0; i < levels.Length; ++i)
				currentExample[i].value *= k[i];

			if(model.numberOfClasses != 0)
			{
				return Convert.ToInt32(SVM.Predict(model,currentExample));
			}
			else return -2;
		}

		private Node[] ConvertToNodeArray(double[] trainingExample)
		{
			Node[] array = new Node[trainingExample.Length];

			for(int i=0; i< array.Length; ++i)
			{
				array[i] = new Node();

				array[i].index = i+1;
				array[i].value = trainingExample[i];
			}

			 return array;
		}		private void Init()
		{
			modelNo = 2; 

			_stren = new double[4]	{	
			0, 0, 	
			0, 0	};	

			if(model.numberOfClasses == 0)
			{
				Debug.LogError("You got problems with your Orbit Training, mate");
			}
		}

		public ScoreNetwork()
		{
			Init();
		}

	}

}

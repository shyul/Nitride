/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nitride
{
	public class TridiagonalMatrix
	{
		public TridiagonalMatrix(int n)
		{
			N = n;
			if (N > 0)
			{
				A = new double[N];
				B = new double[N];
				C = new double[N];
			}
			else
				throw new Exception("N must be greater than 0.");
		}

		public double[] A { get; }

		public double[] B { get; }

		public double[] C { get; }

		public int N { get; }

		public double this[int y, int x]
		{
			get
			{
				int i = y - x;

				if (i == 0)
				{
					return B[y];
				}
				else if (i == -1)
				{
					return C[y];
				}
				else if (i == 1)
				{
					return A[y];
				}
				else return 0;
			}
			set
			{
				int i = y - x;

				if (i == 0)
				{
					B[y] = value;
				}
				else if (i == -1)
				{
					C[y] = value;
				}
				else if (i == 1)
				{
					A[y] = value;
				}
			}
		}

		public double[] Solve(double[] d)
		{
			// cPrime
			double[] cp = new double[N];
			cp[0] = C[0] / B[0];

			for (int i = 1; i < N; i++)
			{
				cp[i] = C[i] / (B[i] - cp[i - 1] * A[i]);
			}

			// dPrime
			double[] dp = new double[N];
			dp[0] = d[0] / B[0];

			for (int i = 1; i < N; i++)
			{
				dp[i] = (d[i] - dp[i - 1] * A[i]) / (B[i] - cp[i - 1] * A[i]);
			}

			// Back substitution
			double[] x = new double[N];
			x[N - 1] = dp[N - 1];

			for (int i = N - 2; i >= 0; i--)
			{
				x[i] = dp[i] - cp[i] * x[i + 1];
			}

			return x;
		}

		public override string ToString()
		{
			var sb = new StringBuilder("Tridiagonal Matrix " + N + ":\n");

			for (int x = 0; x < N; x++)
			{
				for (int y = 0; y < N; y++)
				{
					sb.Append(this[x, y].ToString("0.000") + "\t");

				}

				sb.AppendLine();
			}

			return sb.ToString();
		}

		public static void Test()
		{
			TridiagonalMatrix m = new(10);

			for (int i = 0; i < m.N; i++)
			{
				m.A[i] = 1.111111;
				m.B[i] = 2.222222;
				m.C[i] = 3.333333;
			}

			Console.WriteLine(m.ToString());

			for (int i = 0; i < m.N; i++)
			{
				m[i, i] = 4.4444;
			}

			Console.WriteLine(m.ToString());

			// Solve
			Random rand = new(1);
			double[] d = new double[m.N];

			for (int i = 0; i < d.Length; i++)
			{
				d[i] = (float)rand.NextDouble();
			}

			double[] x = m.Solve(d);

			Console.WriteLine("Solve returned: ");

			for (int i = 0; i < x.Length; i++)
			{
				Console.Write("{0:0.0000}, ", x[i]);
			}

			Console.WriteLine();
		}
	}
}

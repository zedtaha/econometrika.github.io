/*************************************************************************
 *  Matrix library créer par Zaghdoudi Taha 02/01/2012
 *  Le contenu de cette librairie est FREE donc vous pouvez le changer 
 *  Enjoy programming with Taha 
 *  QRDecomp decp = new QRDecomp(Matrix A);
 *  this is a transformation of JAMA java matrix library
 *************************************************************************/
using System;

namespace TMatrix
{
	

	/// <summary>
	///	  QR decomposition for a rectangular matrix.
	/// </summary>
	/// <remarks>
	///   For an m-by-n matrix <c>A</c> with <c>m &gt;= n</c>, the QR decomposition is an m-by-n
	///   orthogonal matrix <c>Q</c> and an n-by-n upper triangular 
	///   matrix <c>R</c> so that <c>A = Q * R</c>.
	///   The QR decompostion always exists, even if the matrix does not have
	///   full rank, so the constructor will never fail.  The primary use of the
	///   QR decomposition is in the least squares solution of nonsquare systems
	///   of simultaneous linear equations.
	///   This will fail if <see cref="IsFullRank"/> returns <see langword="false"/>.
	/// </remarks>
	public class QrDecomp
	{
		private Matrix QR;
		private double[] Rdiag;

		/// <summary>Construct a QR decomposition.</summary>	
		public QrDecomp(Matrix A)
		{
			QR = (Matrix) A.Clone();
			double[][] qr = QR.array;
			int m = A.nrows;
			int n = A.ncols;
			Rdiag = new double[n];
	
			for (int k = 0; k < n; k++) 
			{
				// Compute 2-norm of k-th column without under/overflow.
				double nrm = 0;
				for (int i = k; i < m; i++)
					nrm = this.Hypotenuse(nrm,qr[i][k]);
				 
				if (nrm != 0.0) 
				{
					// Form k-th Householder vector.
					if (qr[k][k] < 0)
						nrm = -nrm;
					for (int i = k; i < m; i++)
						qr[i][k] /= nrm;
					qr[k][k] += 1.0;
	
					// Apply transformation to remaining columns.
					for (int j = k+1; j < n; j++) 
					{
						double s = 0.0; 
						for (int i = k; i < m; i++)
							s += qr[i][k]*qr[i][j];
						s = -s/qr[k][k];
						for (int i = k; i < m; i++)
							qr[i][j] += s*qr[i][k];
					}
				}
				Rdiag[k] = -nrm;
			}
		}
	
		/// <summary>Least squares solution of <c>A * X = B</c></summary>
		/// <param name="rhs">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
		/// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
		/// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
		/// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
		public Matrix Solve(Matrix rhs)
		{
			if (rhs.nrows != QR.nrows) throw new ArgumentException("Matrix row dimensions must agree.");
			if (!IsFullRank) throw new InvalidOperationException("Matrix is rank deficient.");
				
			// Copy right hand side
			int count = rhs.ncols;
			Matrix X = rhs.Clone();
			int m = QR.nrows;
			int n = QR.ncols;
			double[][] qr = QR.array;
			
			// Compute Y = transpose(Q)*B
			for (int k = 0; k < n; k++) 
			{
				for (int j = 0; j < count; j++) 
				{
					double s = 0.0; 
					for (int i = k; i < m; i++)
						s += qr[i][k] * X[i,j];
					s = -s / qr[k][k];
					for (int i = k; i < m; i++)
						X[i,j] += s * qr[i][k];
				}
			}
				
			// Solve R*X = Y;
			for (int k = n-1; k >= 0; k--) 
			{
				for (int j = 0; j < count; j++) 
					X[k,j] /= Rdiag[k];
	
				for (int i = 0; i < k; i++) 
					for (int j = 0; j < count; j++) 
						X[i,j] -= X[k,j] * qr[i][k];
			}
	
			return X.SubMatrix(0, n-1, 0, count-1);
		}

		/// <summary>Shows if the matrix <c>A</c> is of full rank.</summary>
		/// <value>The value is <see langword="true"/> if <c>R</c>, and hence <c>A</c>, has full rank.</value>
		public bool IsFullRank
		{
			get
			{
				int columns = QR.ncols;
				for (int j = 0; j < columns; j++) 
					if (Rdiag[j] == 0)
						return false;
				return true;
			}			
		}
		public Matrix Inverse()
        {
  	  if (!IsFullRank) throw new InvalidOperationException("Matrix is rank deficient.");

            // Copy right hand side
            int m = QR.nrows;
			int n = QR.ncols;
            double[][] qr = QR.array;
            Matrix X = new Matrix(m,m);

            // Compute Y = transpose(Q)
            for (int k = n - 1; k >= 0; k--)
            {
                for (int i = 0; i < m; i++)
                	X.array[k][i] = 0.0;

                X.array[k][ k] = 1.0;
                for (int j = k; j < n; j++)
                {
                	if (qr[k][ k] != 0)
                    {
                        double s = 0.0;

                        for (int i = k; i < m; i++)
                        	s += qr[i][ k] * X.array[j][ i];

                        s = -s / qr[k][ k];

                        for (int i = k; i < m; i++)
                        	X.array[j][ i] += s * qr[i][ k];
                    }
                }
            }

            // Solve R*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < m; j++)
                	X.array[k][ j] /= Rdiag[k];

                for (int i = 0; i < k; i++)
                    for (int j = 0; j < m; j++)
                		X.array[i][ j] -= X.array[k][ j] * qr[i][ k];
            }

  	 return X;
        
  }
		/// <summary>Returns the upper triangular factor <c>R</c>.</summary>
		public Matrix UpperTriangularFactor
		{
			get
			{
				int n = QR.ncols;
				Matrix X = new Matrix(n, n);
				double[][] x = X.array;
				double[][] qr = QR.array;
				for (int i = 0; i < n; i++) 
					for (int j = 0; j < n; j++) 
						if (i < j)
							x[i][j] = qr[i][j];
						else if (i == j) 
							x[i][j] = Rdiag[i];
						else
							x[i][j] = 0.0;
	
				return X;
			}
		}

		/// <summary>Returns the orthogonal factor <c>Q</c>.</summary>
		public Matrix OrthogonalFactor
		{
			get
			{
				Matrix X = new Matrix(QR.nrows, QR.ncols);
				double[][] x = X.array;
				double[][] qr = QR.array;
				for (int k = QR.ncols - 1; k >= 0; k--) 
				{
					for (int i = 0; i < QR.nrows; i++)
					{
						x[i][k] = 0.0;
					}

					x[k][k] = 1.0;
					for (int j = k; j < QR.ncols; j++) 
					{
						if (qr[k][k] != 0) 
						{
							double s = 0.0;
							for (int i = k; i < QR.nrows; i++)
							{
								s += qr[i][k] * x[i][j];
							}

							s = -s / qr[k][k];
							for (int i = k; i < QR.nrows; i++)
							{
								x[i][j] += s * qr[i][k];
							}
						}
					}
				}

				return X;
			}
		}

		private double Hypotenuse(double a, double b) 
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				double r = b / a;
				return Math.Abs(a) * Math.Sqrt(1 + r * r);
			}

			if (b != 0)
			{
				double r = a / b;
				return Math.Abs(b) * Math.Sqrt(1 + r * r);
			}

			return 0.0;
		}
	}
}

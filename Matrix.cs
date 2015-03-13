/*************************************************************************
 *  Matrix library créer par Zaghdoudi Taha 02/01/2012
 *  Le contenu de cette librairie est FREE donc vous pouvez le changer 
 *  Enjoy programming with Taha 
 *  Matrix A = new Matrix(double[][] data);
 * Cette libraire est fortement inspirée de JAMA java matrix librairy 
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
namespace TMatrix
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Matrix
	{
		// rows = nombres de lignes__ cols = nombres de colonnes
		public int rows, cols;
		// les donners Jagged 
		public double[][] data;
		
	
		// création d'une (r,c) matrice vide 
		public Matrix(int r, int c){
			this.rows=r;
			this.cols=c;
			this.data= new double[rows][];
			for(int i=0;i<rows;i++){
				this.data[i]= new double[cols];
				for(int j=0;j<cols;j++){
					data[i][j]=0.0;
				}
			}
		}
		// création d'une matrice
		public Matrix (int r, int c, double value){
			this.rows=r;
			this.cols=c;
			data = new double[rows][];
			for(int i=0;i<rows;i++){
				this.data[i]= new double[cols];
				for(int j=0;j<cols;j++){
					data[i][j]=value;
				}
			}
		}
		// création d'une matrice avce des données jagged
		public Matrix (double[][] data){
			this.rows= data.Length;
			this.cols= data[0].Length;
			this.data= data;
		}
		// création d'une matrice à partir de données double[] vals
		public Matrix (double[] vals, int r){
			this.rows = r;
			cols = (r != 0?vals.Length / r:0);
			if (r * cols != vals.Length)
			{
				throw new System.ArgumentException("Array length must be a multiple of rows.");
			}
			data = new double[r][];
			
			for (int i = 0; i < r; i++)
			{
				data[i]= new double[cols];
				for (int j = 0; j < cols; j++)
				{
					data[i][j] = vals[i + j * r];
				}
			}
		}
		// création d'une matrice à partir d'une autre matrice
		public Matrix (Matrix m){
			rows= m.rows;
			cols=m.cols;
			data= new double[rows][];
			for(int i=0;i<rows;i++){
				data[i]= new double[cols];
				for(int j=0;j<cols;j++){
					data[i][j]=m.data[i][j];
				}
			}
		}
		// retour de données en double[][] 
		public double[][] array{
			get{
				return this.data;
			}
		}
		public double this[int r,int c]{
			
			get
            {
            if (r < 0 || r > rows)
            {
             throw new Exception("m-th row is out of range!");
            }
             if (c < 0 || c > cols)
            {
           throw new Exception("n-th col is out of range!");
             }
             return this.data[r][ c];
             }
			set { this.data[r][ c] = value; }
			}
		// retourne le nombre de ligne de la matrice 
		public int nrows{
			get{
				return rows;
			}
		}
		// retourne le nombre de colonnes de la matrice
		public int ncols{
			get{
				return cols;
			}
		}
		// 
		public bool IsSquare
		{
			get 
			{ 
				return (rows == cols); 
			}
		}
		public bool IsSymmetric
		{
			get
			{
				if (this.IsSquare)
				{
					for (int i = 0; i < rows; i++)
					{
						for (int j = 0; j <= i; j++)
						{
							if (data[i][j] != data[j][i])
							{
								return false;
							}
						}
					}

					return true;
				}

				return false;
			}
		}
		// avoir une copie de la matrice 
		public Matrix Clone(){
			Matrix m = new Matrix(data);
			m.data= (double[][])data.Clone();
			return m;
		}
		// changer un élement de la matrice
		public void SetElement(int i,int j, double s){
			data[i][j]=s;
		}
		// créer une sous-matrice à partir de la matrice principale
		public Matrix SubMatrix(int[] r, int j0, int j1)
		{
			if ((j0 > j1) || (j0 < 0) || (j0 >= ncols) || (j1 < 0) || (j1 >= ncols)) 
			{ 
				throw new ArgumentException(); 
			} 
			
			Matrix X = new Matrix(r.Length, j1-j0+1);
			
			for (int i = 0; i < r.Length; i++)
			{
				for (int j = j0; j <= j1; j++) 
				{
					if ((r[i] < 0) || (r[i] >= this.rows))
					{
						throw new ArgumentException();
					}

					X[i,j - j0] = data[r[i]][j];
				}
			}

			return X;
		}
		public Matrix SubMatrix(int r1, int r2, int c1, int c2){
			 Matrix A = new Matrix(r2 - r1 + 1, c2 - c1 + 1);
			 for (int i = r1; i<=r2; i++) {
			 	 for (int j = c1; j<=c2; j++) {
			 		A.array[i - r1][j - c1] = this.data[i][j];
			 	}
			 }
			 return A;
		}
		// avoir un vecteur de ligne de la matrice principale 
		public Vector getRowVector(int m){
			if (m < 0 || m > nrows){
				throw new Exception("m-th row is out of range!");
			}
			Vector rv = new Vector(ncols);
			for (int i = 0; i < ncols; i++){
				rv[i]=data[m][i];
			}
			return rv;
		}
		// avoir un vecteur de colonne de la matrice principale 
		public Vector getColVector(int m){
			if (m < 0 || m > ncols){
				throw new Exception("m-th row is out of range!");
			}
			Vector cv = new Vector(nrows);
			for (int i = 0; i < nrows; i++){
				cv[i]=data[i][m];
			}
			return cv;
		}
		// remplacer une ligne de la matrice principale 
		public Matrix ReplaceRow(Vector a, int m){

     if (m < 0 || m > nrows){

         throw new Exception("m-th row is out of range!");
		 }

         if (a.Vsize != ncols){

           throw new Exception("Vector ndim is out of range!");
		   }

           for (int i = 0; i <ncols; i++){

				data[m][ i] = a[i];
		  }

         return new Matrix(data);
		}
		/// <summary>
		/// remplacer une colonne de la matrice principale 
		/// </summary>
		
		
		public Matrix ReplaceCol(Vector a, int m){

     if (m < 0 || m > ncols){

         throw new Exception("m-th col is out of range!");
		 }

         if (a.Vsize != nrows){

           throw new Exception("Vector ndim is out of range!");
		   }

           for (int i = 0; i <nrows; i++){

				data[i][ m] = a[i];
		  }

         return new Matrix(data);
		}
		// transposition de matrice 
		public Matrix Transpose(){
			Matrix c = new Matrix(ncols, nrows);
			for(int i=0;i<nrows;i++){
				for(int j=0;j<ncols;j++){
					c.array[j][i]=data[i][j];
				}
			}
			return c;
		}
       // somme de deux matrices 
		public static Matrix operator +(Matrix a){
			return a;
		}
		public static Matrix operator +(Matrix a, Matrix b){
			int Rows= a.rows;
			int Cols= a.cols;
			if ((Rows != b.rows) || (Cols != b.cols))
			{
				throw new ArgumentException("Matrix dimension do not match.");
			}
			Matrix c = new Matrix(Rows, Cols);
			for(int i=0;i<Rows;i++){
				for(int j=0;j<Cols;j++){
					c.array[i][j]=a.array[i][j]+b.array[i][j];
				}
			}
			return c;
		}
		// soustraction de deux matrices 
		public static Matrix operator -(Matrix a){
			return -a;
		}
		public static Matrix operator -(Matrix a, Matrix b){
			int Rows= a.rows;
			int Cols= a.cols;
			if ((Rows != b.rows) || (Cols != b.cols))
			{
				throw new ArgumentException("Matrix dimension do not match.");
			}
			Matrix c = new Matrix(Rows, Cols);
			for(int i=0;i<Rows;i++){
				for(int j=0;j<Cols;j++){
					c.array[i][j]=a.array[i][j]-b.array[i][j];
				}
			}
			return c;
		}
		// opération de multiplication de matrices 
		public static Matrix operator *(Matrix a, double s){
			Matrix c = new Matrix(a.nrows, a.ncols);
			for(int i=0;i<c.nrows;i++){
				for(int j=0;j<c.ncols;j++){
					c.array[i][j]=a.array[i][j]*s;
				}
			}
			return c;
		}
		
		public static Matrix operator *(double s, Matrix a){
			Matrix c = new Matrix(a.nrows, a.ncols);
			for(int i=0;i<c.nrows;i++){
				for(int j=0;j<c.ncols;j++){
					c.array[i][j]=a.array[i][j]*s;
				}
			}
			return c;
		}
		public static Matrix operator*(Matrix a, Matrix b){
			if(a.ncols != b.nrows){
				throw new Exception("The numbers of columns of the first matrix must be equal to the number of rows of the second matrix!");
			}
			double temp;
			Matrix c = new Matrix(a.nrows, b.ncols);
			for(int i=0;i<a.nrows;i++){
				for(int j=0;j<b.ncols;j++){
					temp = c.array[i][j];
				 for(int k = 0; k < c.nrows; k++){
						temp+=a.array[i][k]*b.array[k][j];
					}
					c.array[i][j]=temp;
				}
			}
			return c;
		}
		// division de matrices 
		public static Matrix operator /(double s, Matrix a){
			Matrix c = new Matrix(a.nrows, a.ncols);
			for(int i=0;i<c.nrows;i++){
				for(int j=0;j<c.ncols;j++){
					c.array[i][j]=a.array[i][j]/s;
				}
			}
			return c;
		}
		public static Matrix operator /(Matrix a, double s){
			Matrix c = new Matrix(a.nrows, a.ncols);
			for(int i=0;i<c.nrows;i++){
				for(int j=0;j<c.ncols;j++){
					c.array[i][j]=a.array[i][j]/s;
				}
			}
			return c;
		}
		public static Matrix crossprod(Matrix a, Matrix b){
			return a.Transpose()*b;
		}
		public static Matrix Divide(Matrix x, Matrix y){
			
			return (crossprod(x,x).Inverse)*crossprod(x,y);
		}
		 public static Matrix operator /(Matrix a, Matrix b)
        {
            return Divide(a, b);
        }
			
		// trace de matrice 
		public double Trace(){
			double sumdiag=0.0;
			for(int i=0;i<nrows;i++){
				if(i<ncols){
					sumdiag += data[i][i];
				}
			}
			return sumdiag;
		}
		public double Norm(){
			double l=0;
			for(int i=0;i<nrows;i++){
				for(int j=0;j<ncols;j++){
					l=l+this.data[i][j]*this.data[i][j];
				}
			}
			l=Math.Pow(l,0.5);
			return l;
		}
		
	// création de matrice identitaire
		public static Matrix Identity(int n){
			Matrix a = new Matrix(n,n);
			for(int i=0;i<n;i++){
				a.array[i][i]=1.0D;
			}
			return a;
		}
		
	public static Matrix Identity(int r, int c){
    
        Matrix a = Zeros(r, c);
        for (int i = 0; i < Math.Min(r, c); i++){
        	a.array[i][ i] = 1;
        }
        return a;
    }
			// création d'une matrice zero
	public static Matrix Zeros(int r, int c){
				
        Matrix a = new Matrix(r, c);
        for (int i = 0; i < r; i++){
        	for (int j = 0; j < c; j++){
        		a.array[i][ j] = 0;
        	}
        }
        return a;
    }
			// matrice unitaire 
			public static Matrix ones(int rows, int cols){
			Matrix temp = new Matrix(rows,cols);
			for(int i=0;i<rows;i++){
				for(int j=0;j<cols;j++){
					temp.array[i][j]=1;
				}
			}
			return temp;
		}	
		
		// retourne le vecteur de solution LHS si la matrice est carée sinon les MCO 
	   public Matrix Solve(Matrix rhs)
		{
			return (rows == cols) ? new LuDecomp(this).Solve(rhs) : new QrDecomp(this).Solve(rhs);
		}
	   // inversion d'une matrice carrée sinon on utilise une psedoinversion
	   public Matrix Inverse
		{
			get 
			{ 
				return this.Solve(Diagonal(rows, rows, 1.0)); 
			}
		}
	   // calcul du determinant de la matrice 
	   public double Determinant
		{
			get 
			{ 
				return new LuDecomp(this).Determinant; 
			}
		}
	   // vecteur de la diagonale de la matrice pour les valeurs specifiques 
		public static Matrix Diagonal(int rows, int columns, double value)
		{
			Matrix X = new Matrix(rows, columns);
			double[][] x = X.array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = ((i == j) ? value : 0.0);
				}
			}
			return X;
		}
		
		
	}
}

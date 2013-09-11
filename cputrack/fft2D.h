
#pragma once
#include <stdlib.h>
#include <complex>

// See http://paulbourke.net/miscellaneous/dft/

namespace fft2d {

	inline bool Powerof2(int n, int* e) {
		int m = 1;
		int p = 0;
		while (m < n) { p++; m*=2; }
		if (e) *e=p;
		return m == n;
	}

	/*-------------------------------------------------------------------------
	   This computes an in-place complex-to-complex FFT
	   x and y are the real and imaginary arrays of 2^m points.
	   dir =  1 gives forward transform
	   dir = -1 gives reverse transform

		 Formula: forward
					  N-1
					  ---
				  1   \          - j k 2 pi n / N
		  X(n) = ---   >   x(k) e                    = forward transform
				  N   /                                n=0..N-1
					  ---
					  k=0

		  Formula: reverse
					  N-1
					  ---
					  \          j k 2 pi n / N
		  X(n) =       >   x(k) e                    = forward transform
					  /                                n=0..N-1
					  ---
					  k=0
	*/
	template<typename T>
	int FFT(int dir,int m,T *x,T *y)
	{
	   long nn,i,i1,j,k,i2,l,l1,l2;
	   T c1,c2,tx,ty,t1,t2,u1,u2,z;

	   /* Calculate the number of points */
	   nn = 1;
	   for (i=0;i<m;i++)
		  nn *= 2;

	   /* Do the bit reversal */
	   i2 = nn >> 1;
	   j = 0;

	   for (i=0;i<nn-1;i++) {
		  if (i < j) {
			 tx = x[i];
			 ty = y[i];
			 x[i] = x[j];
			 y[i] = y[j];
			 x[j] = tx;
			 y[j] = ty;
		  }
		  k = i2;
		  while (k <= j) {
			 j -= k;
			 k >>= 1;
		  }
		  j += k;
	   }

	   // Compute the FFT 
	   c1 = -1.0;
	   c2 = 0.0;
	   l2 = 1;
	   for (l=0;l<m;l++) {
		  l1 = l2;
		  l2 <<= 1;
		  u1 = 1.0;
		  u2 = 0.0;
		  for (j=0;j<l1;j++) {
			 for (i=j;i<nn;i+=l2) {
				i1 = i + l1;
				t1 = u1 * x[i1] - u2 * y[i1];
				t2 = u1 * y[i1] + u2 * x[i1];
				x[i1] = x[i] - t1;
				y[i1] = y[i] - t2;
				x[i] += t1;
				y[i] += t2;
			 }
			 z =  u1 * c1 - u2 * c2;
			 u2 = u1 * c2 + u2 * c1;
			 u1 = z;
		  }
		  c2 = sqrt((1.0f - c1) / 2.0);
		  if (dir == 1)
			 c2 = -c2;
		  c1 = sqrt((1.0f + c1) / 2.0);
	   }

	   /* Scaling for forward transform */
	   if (dir == 1) {
		  for (i=0;i<nn;i++) {
			 x[i] /= (T)nn;
			 y[i] /= (T)nn;
		  }
	   }

	   /*
int FFT(int dir,int m,double *x,double *y)
{
   long nn,i,i1,j,k,i2,l,l1,l2;
   double c1,c2,tx,ty,t1,t2,u1,u2,z;

   // Calculate the number of points 
   nn = 1;
   for (i=0;i<m;i++)
      nn *= 2;

   // Do the bit reversal 
   i2 = nn >> 1;
   j = 0;
   for (i=0;i<nn-1;i++) {
      if (i < j) {
         tx = x[i];
         ty = y[i];
         x[i] = x[j];
         y[i] = y[j];
         x[j] = tx;
         y[j] = ty;
      }
      k = i2;
      while (k <= j) {
         j -= k;
         k >>= 1;
      }
      j += k;
   }

   // Compute the FFT 
   c1 = -1.0;
   c2 = 0.0;
   l2 = 1;
   for (l=0;l<m;l++) {
      l1 = l2;
      l2 <<= 1;
      u1 = 1.0;
      u2 = 0.0;
      for (j=0;j<l1;j++) {
         for (i=j;i<nn;i+=l2) {
            i1 = i + l1;
            t1 = u1 * x[i1] - u2 * y[i1];
            t2 = u1 * y[i1] + u2 * x[i1];
            x[i1] = x[i] - t1;
            y[i1] = y[i] - t2;
            x[i] += t1;
            y[i] += t2;
         }
         z =  u1 * c1 - u2 * c2;
         u2 = u1 * c2 + u2 * c1;
         u1 = z;
      }
      c2 = sqrt((1.0 - c1) / 2.0);
      if (dir == 1)
         c2 = -c2;
      c1 = sqrt((1.0 + c1) / 2.0);
   }

   // Scaling for forward transform 
   if (dir == 1) {
      for (i=0;i<nn;i++) {
         x[i] /= (double)nn;
         y[i] /= (double)nn;
      }
   }*/

	   return true;
	}

	/*-------------------------------------------------------------------------
	   Perform a 2D FFT inplace given a complex 2D array
	   The direction dir, 1 for forward, -1 for reverse
	   The size of the array (nx,ny)
	   Return false if there are memory problems or
		  the dimensions are not powers of 2
	*/
	template<typename T>
	bool FFT2D(std::complex<T> *c,int nx,int ny,int dir)
	{
	   int i,j;
	   T *real,*imag;

	   /* Transform the rows */
	   real = new T[nx];
	   imag = new T[nx];
	   if (real == NULL || imag == NULL)
		  return false;
	   int m=0;
	   if (!Powerof2(nx, &m))
		   throw std::runtime_error("FFT2D: #Columns not power-of-two");
	   for (j=0;j<ny;j++) {
		  for (i=0;i<nx;i++) {
			 real[i] = c[j*nx+i].real();
			 imag[i] = c[j*nx+i].imag();
		  }
		  FFT(dir,m,real,imag);
		  for (i=0;i<nx;i++) {
			 c[j*nx+i] = std::complex<T>(real[i], imag[i]);
		  }
	   }
	   delete[] real;
	   delete[] imag;

	   /* Transform the columns */
	   real = new T[ny];
	   imag = new T[ny];
	   if (!Powerof2(ny, &m))
		   throw std::runtime_error("FFT2D: #Columns not power-of-two");
	   for (i=0;i<nx;i++) {
		  for (j=0;j<ny;j++) {
			 real[j] = c[j*nx+i].real();
			 imag[j] = c[j*nx+i].imag();
		  }
		  FFT(dir,m,real,imag);
		  for (j=0;j<ny;j++)
			 c[j*nx+i] = std::complex<T>(real[j], imag[j]);
	   }
	   delete[] real;
	   delete[] imag;
	   
	   return true;
	}

}
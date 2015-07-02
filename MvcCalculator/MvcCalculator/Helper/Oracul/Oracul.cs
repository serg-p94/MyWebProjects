using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcCalculator.Models;

namespace MvcCalculator.Helper.Oracul
{
    public class Oracul
    {
        private Dictionary<int, int> userIndexes;
        private Dictionary<int, int> movieIndexes;
        private Dictionary<int, int> movieIds;
        private short[,] marks;

        public Oracul(int[] users, int[] movies)
        {
            userIndexes = new Dictionary<int, int>(users.Length);
            movieIndexes = new Dictionary<int, int>(movies.Length);
            movieIds = new Dictionary<int, int>(movies.Length);
            for (int i = 0; i < users.Length; i++)
            {
                userIndexes.Add(users[i], i);
            }
            for (int i = 0; i < movies.Length; i++)
            {
                movieIndexes.Add(movies[i], i);
                movieIds.Add(i, movies[i]);
            }
            marks = new short[users.Length, movies.Length];
        }

        public void SetMark(int userId, int movieId, short mark)
        {
            marks[userIndexes[userId], movieIndexes[movieId]] = mark;
        }
        
        /*
         * Calculates coefficients of user's campatibility
         * Their sum is 1
         */
        public double[] CalculateUsersWeights(int mainUserId)
        {
            var dst = new double[userIndexes.Count];
            double dstSum = 0;
            var mainUserIndex = userIndexes[mainUserId];
            for (int i = 0; i < userIndexes.Count; i++)
            {
                dst[i] = 0;
                if (i == mainUserIndex)
                    continue;
                int nMarks = 0;
                for (int j = 0; j < movieIndexes.Count; j++)
                {
                    if (marks[i, j] != 0 && marks[mainUserIndex, j] != 0)
                    {
                        nMarks++;
                        dst[i] += Math.Pow(marks[i, j] - marks[mainUserIndex, j], 2);
                    }
                }
                if (nMarks != 0)
                    dst[i] = Math.Sqrt(dst[i]/nMarks);
                dst[i] = 1 - dst[i]/10;
                dstSum += dst[i];
            }
            for (int i = 0; i < dst.Length; i++)
            {
                if (dstSum != 0)
                    dst[i] /= dstSum;
            }
            return dst;
        }

        public Movie[] Sort(Movie[] movies, int mainUserId)
        {
            var usersWeights = CalculateUsersWeights(mainUserId);
            var weights = new Dictionary<int, double>(movieIndexes.Count);
            for (int i = 0; i < movieIndexes.Count; i++)
            {
                double weight = 0;
                for (int j = 0; j < userIndexes.Count; j++)
                {
                    weight += usersWeights[j]*marks[j, i];
                }

                // if user has seen the movie, it's weight is <5, else > 5
                weight /= 2;
                if (marks[userIndexes[mainUserId], i] == 0)
                    weight += 5;

                weights.Add(movieIds[i], weight);
            }
            Array.Sort(movies, new MovieComparator(weights).Descending);
            return movies;
        }


        private class MovieComparator
        {
            private Dictionary<int, double> weights;

            public MovieComparator(Dictionary<int, double> weights)
            {
                this.weights = weights;
            }

            public int Descending(Movie m1, Movie m2)
            {
                return weights[m2.MovieId].CompareTo(weights[m1.MovieId]);
            }
        }

    }

}
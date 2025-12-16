using System;

namespace project_chess_DB.Services
{
    public static class EloCalculator
    {
        // K-Factor : 32 est la valeur standard
        private const int K = 32;
        public static int CalculateNewRating(int currentRating, int opponentRating, double actualScore)
        {
            // 1. Calculer l'espérance de gain (Probability of winning)
            double expectedScore = 1.0 / (1.0 + Math.Pow(10.0, (opponentRating - currentRating) / 400.0));

            // 2. Calculer le nouveau ELO
            int newRating = (int)(currentRating + K * (actualScore - expectedScore));

            return newRating;
        }
    }
}
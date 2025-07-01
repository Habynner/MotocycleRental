using System;

namespace challange_bikeRental.Utils
{
    public enum RentalPlan
    {
        SevenDays = 30,   // 7 dias com R$30,00 por dia
        FifteenDays = 28, // 15 dias com R$28,00 por dia
        ThirtyDays = 22,  // 30 dias com R$22,00 por dia
        FortyFiveDays = 20, // 45 dias com R$20,00 por dia
        FiftyDays = 18    // 50 dias com R$18,00 por dia
    }

    public static class RentalPlanExtensions
    {
        /// <summary>
        /// Obtém o plano de locação com base no número de dias.
        /// </summary>
        /// <param name="days">Número de dias.</param>
        /// <returns>Plano de locação correspondente.</returns>
        public static RentalPlan GetPlanByDays(int days)
        {
            return days switch
            {
                7 => RentalPlan.SevenDays,
                15 => RentalPlan.FifteenDays,
                30 => RentalPlan.ThirtyDays,
                45 => RentalPlan.FortyFiveDays,
                50 => RentalPlan.FiftyDays,
                _ => throw new ArgumentException("Número de dias inválido para um plano de locação.")
            };
        }

        /// <summary>
        /// Calcula o custo total com base no número de dias.
        /// </summary>
        /// <param name="days">Número de dias.</param>
        /// <returns>Custo total em reais.</returns>
        public static decimal GetTotalCost(int days)
        {
            var plan = GetPlanByDays(days);
            return days * (int)plan;
        }

        /// <summary>
        /// Obtém o custo diário com base no número de dias.
        /// </summary>
        /// <param name="days">Número de dias.</param>
        /// <returns>Custo diário em reais.</returns>
        public static int GetDailyCost(int days)
        {
            var plan = GetPlanByDays(days);
            return (int)plan;
        }
    }
}

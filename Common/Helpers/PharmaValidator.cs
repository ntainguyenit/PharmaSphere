using System;
using System.Text.RegularExpressions;

namespace PharmaSphere.Common.Helpers
{
    /// <summary>
    /// Utility class for validating pharmaceutical specific data.
    /// </summary>
    public static class PharmaValidator
    {
        /// <summary>
        /// Validates if a string is a valid National Drug Code (NDC).
        /// Format: XXXX-XXXX-XX or XXXXX-XXXX-X or XXXXX-XXX-XX
        /// </summary>
        public static bool IsValidNDC(string ndc)
        {
            if (string.IsNullOrWhiteSpace(ndc)) return false;
            
            // Simplified NDC regex for demonstration
            var ndcRegex = new Regex(@"^\d{4,5}-\d{3,4}-\d{1,2}$");
            return ndcRegex.IsMatch(ndc);
        }

        /// <summary>
        /// Validates product ingredients for common allergens.
        /// </summary>
        public static bool ContainsAllergen(string ingredients, string allergen)
        {
            if (string.IsNullOrWhiteSpace(ingredients) || string.IsNullOrWhiteSpace(allergen)) 
                return false;

            return ingredients.Contains(allergen, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates if a dosage string is in a proper format (e.g. "500mg", "10ml").
        /// </summary>
        public static bool IsValidDosage(string dosage)
        {
            if (string.IsNullOrWhiteSpace(dosage)) return false;
            var dosageRegex = new Regex(@"^\d+(\.\d+)?\s*(mg|g|ml|l|mcg)$", RegexOptions.IgnoreCase);
            return dosageRegex.IsMatch(dosage);
        }
    }
}

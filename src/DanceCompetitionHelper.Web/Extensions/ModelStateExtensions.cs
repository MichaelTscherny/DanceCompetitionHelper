using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DanceCompetitionHelper.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetErrorMessages(
            this ModelStateDictionary fromModel,
            string separator = "\r\n")
        {
            if (fromModel == null
                || fromModel.Count <= 0)
            {
                return string.Empty;
            }

            return string.Join(
                separator,
                fromModel.Values
                    .Where(
                        x => x.ValidationState != ModelValidationState.Valid
                        && x.Errors.Count >= 1)
                    .SelectMany(
                        x => x.Errors)
                    .Select(
                        x => x.ErrorMessage));
        }
    }
}

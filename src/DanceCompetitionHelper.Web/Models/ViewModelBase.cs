using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DanceCompetitionHelper.Web.Models
{
    public abstract class ViewModelBase
    {
        public List<string> Errors { get; set; } = new List<string>();

        public void AddError(
            string errorString)
        {
            Errors.Add(
                errorString);
        }

        public void AddErrors(
            Exception exc)
        {
            var innerExc = exc.InnerException;

            if (innerExc != null)
            {
                Errors.Add(
                    innerExc.Message);
            }

            Errors.Add(exc.Message);
        }

        public void AddErrors(
            ModelStateDictionary fromModel)
        {
            Errors.AddRange(
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

using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using System.Globalization;

namespace DanceCompetitionHelper.Database.Data
{
    public class ConfigurationValueParser
    {
        public ConfigurationValue ConfigurationValue { get; }

        public ConfigurationScopeEnum Scope => ConfigurationValue.Scope;
        public string? Value
        {
            get => ConfigurationValue.Value;
            set => ConfigurationValue.Value = value;
        }
        public bool ValueAvailable => string.IsNullOrEmpty(Value) == false;

        public decimal? DefaultDecimal { get; private set; }
        public string? DefaultString { get; private set; }
        public TimeSpan? DefaultTimeSpan { get; private set; }

        public ConfigurationValueParser(
            ConfigurationValue configurationValue)
        {
            ConfigurationValue = configurationValue
                ?? throw new ArgumentNullException(
                    nameof(configurationValue));
        }

        #region Default values

        public ConfigurationValueParser SetDefaultDecimal(
            decimal? defaultDecimal)
        {
            DefaultDecimal = defaultDecimal;

            return this;
        }
        public ConfigurationValueParser SetDefaultString(
            string? defaultString)
        {
            DefaultString = defaultString;

            return this;
        }
        public ConfigurationValueParser SetDefaultTimeSpan(
            TimeSpan? defaultTimeSpan)
        {
            DefaultTimeSpan = defaultTimeSpan;

            return this;
        }

        #endregion Default values

        #region Set values

        public ConfigurationValueParser SetDecimal(
            decimal setDecimal)
        {
            Value = Convert.ToString(
                setDecimal,
                CultureInfo.InvariantCulture);

            return this;
        }
        public ConfigurationValueParser SetString(
            string? setString)
        {
            Value = setString;

            return this;
        }
        public ConfigurationValueParser SetTimeSpan(
            TimeSpan setTimeSpan)
        {
            Value = setTimeSpan.ToString("g");

            return this;
        }

        #endregion Set values

        #region Parser

        public decimal? AsDecimal()
        {
            if (ValueAvailable == false)
            {
                return DefaultDecimal;
            }

            try
            {
                return Convert.ToDecimal(
                    Value);
            }
            catch
            {
                return Convert.ToDecimal(
                    Value,
                    CultureInfo.InvariantCulture);
            }
        }

        public string? AsString()
        {
            if (ValueAvailable == false)
            {
                return DefaultString;
            }

            return Value;
        }

        public TimeSpan? AsTimeSpan()
        {
            if (ValueAvailable == false)
            {
                return DefaultTimeSpan;
            }

            return TimeSpan.Parse(
                Value ?? string.Empty);
        }

        #endregion Parser
    }
}

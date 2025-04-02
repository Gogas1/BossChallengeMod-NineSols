using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BossChallengeMod.Configuration.Fields {
    internal class FloatField : FieldBase<float> {
        public FloatField(string label, float maxWidth, string? tooltip, float defaultValue) : base(label, maxWidth, tooltip, defaultValue) {
        }

        protected override float ConvertToValue(string input) {
            if (float.TryParse(input, 
                NumberStyles.AllowLeadingWhite |
                NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out float value)) {
                return value;
            }
            else {
                return 0;
            }
        }

        protected override string ConvertToString(float input) {
            return input.ToString("F3", CultureInfo.InvariantCulture);
        }

        protected override bool Validate(string input) {
            return float.TryParse(input,
                NumberStyles.AllowLeadingWhite |
                NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result);
        }
    }
}

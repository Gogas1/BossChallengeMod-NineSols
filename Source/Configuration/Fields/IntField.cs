using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration.Fields {
    internal class IntField : FieldBase<int> {
        public IntField(string label, float maxWidth, string? tooltip, int defaultValue) : base(label, maxWidth, tooltip, defaultValue) {
        }

        protected override string ConvertToString(int input) {
            return input.ToString();
        }

        protected override int ConvertToValue(string input) {
            if (int.TryParse(input, out int value)) {
                return value;
            } else {
                return 0;
            }
        }

        protected override bool Validate(string input) {
            return int.TryParse(input, out int value);
        }
    }
}

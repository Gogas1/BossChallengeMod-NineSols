using System;

namespace BossChallengeMod.Configuration.Fields {
    internal interface IField {
        void SetMaxWidth(float maxWidth);
        void AddValueChangeHandler(Action<object?> handler);
        void Draw();

    }

    internal interface IField<T> : IField {

        void AddFieldValueChangeHandler(Action<T?> handler);
    }

    internal interface ILabeledField : IField {
        void SetLabel(string label);
        void SetTooltip(string tooltip);
    }

    internal interface ILabeledField<T> : IField<T>, ILabeledField {

    }
}

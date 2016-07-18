using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    // Should be applied to properties and fields.
    public class IntValidatorAttribute : ValidationAttribute
    {
        private int leftEdge;
        private int rightEdge;

        public IntValidatorAttribute(int leftEdge, int rightEdge)
        {
            this.leftEdge = leftEdge;
            this.rightEdge = rightEdge;
        }

        public override bool IsValid(object value)
        {
            if (!(value is int))
            {
                throw new ArgumentException(nameof(value), "The value must be typeof int");
            }

            int id = (int)value;

            return (id >= leftEdge && id <= rightEdge);
        }
    }
}

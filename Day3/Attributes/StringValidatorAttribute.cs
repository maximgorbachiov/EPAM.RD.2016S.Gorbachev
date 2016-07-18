using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    // Should be applied to properties and fields.
    public class StringValidatorAttribute : ValidationAttribute
    {
        private int wordLenght;

        public StringValidatorAttribute(int wordLenght)
        {
            this.wordLenght = wordLenght;
        }

        public override bool IsValid(object value)
        {
            string word;

            if (!(value is string))
            {
                throw new ArgumentException(nameof(value), "The value must be typeof string");
            }

            if ((word = (string)value) == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (word.Length <= wordLenght);
        }
    }
}

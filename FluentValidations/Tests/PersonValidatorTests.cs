namespace DotNetCoreKit.FluentValidations.Tests
{
    using DotNetCoreKit.FluentValidations;

    using FluentValidation.TestHelper;

    using NUnit.Framework;

    /// <summary>
    /// Sample test file, ideally we will have a extract this to a separate project into the main solution.
    /// </summary>
    [TestFixture]
    public class PersonValidatorTests
    {
        private PersonValidator _validator;

        [SetUp]
        public void Setup()
        {
            this._validator = new PersonValidator();
        }

        [Test]
        public void Should_have_error_when_Name_is_null()
        {
            this._validator.ShouldHaveValidationErrorFor(person => person.Name, null as string);
        }

        [Test]
        public void Should_not_have_error_when_name_is_specified()
        {
            var person = new Person { Name = "Jeremy" };
            this._validator.ShouldNotHaveValidationErrorFor(x => x.Name, person);
            this._validator.ShouldHaveChildValidator(x => x.Pets, typeof(int));
        }
    }
}
// -----------------------------------------------------------------------
// <copyright file="PersonValidatorTests.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices.FluentValidations.Tests
{
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    /// <summary>
    /// Sample test file, ideally we will have a extract this to a separate project into the main solution.
    /// </summary>
    [TestFixture]
    public class PersonValidatorTests
    {
        /// <summary>
        /// Gets or sets the _validator.
        /// </summary>
        private PersonValidator Validator { get; set; }

        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Validator = new PersonValidator();
        }

        /// <summary>
        /// The should_have_error_when_ name_is_null.
        /// </summary>
        [Test]
        public void Should_have_error_when_Name_is_null()
        {
            Validator.ShouldHaveValidationErrorFor(person => person.Name, null as string);
        }

        /// <summary>
        /// The should_not_have_error_when_name_is_specified.
        /// </summary>
        [Test]
        public void Should_not_have_error_when_name_is_specified()
        {
            var person = new Person { Name = "Jeremy" };
            Validator.ShouldNotHaveValidationErrorFor(x => x.Name, person);
            Validator.ShouldHaveChildValidator(x => x.Pets, typeof(int));
        }
    }
}
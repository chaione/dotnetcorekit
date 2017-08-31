namespace DotNetCoreKit.Dto
{
    using System.Collections.Generic;

    public class PeopleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<int> Pets { get; set; }
    }
}
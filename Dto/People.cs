namespace DotNetCoreKit.Dto
{
    using System.Collections.Generic;

    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<int> Pets { get; set; }
    }
}
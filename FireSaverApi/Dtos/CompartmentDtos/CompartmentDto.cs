using System.Collections.Generic;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.TestDtos;

namespace FireSaverApi.Dtos.CompartmentDtos
{
    public class CompartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SafetyRules { get; set; }
        public TestInputDto CompartmentTest { get; set; }
    }
}
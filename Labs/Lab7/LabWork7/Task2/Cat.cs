using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Task2
{
    public class Cat
    {
        [JsonPropertyName("fact")]
        public string Fact { get; set; } = null!;

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }
}
